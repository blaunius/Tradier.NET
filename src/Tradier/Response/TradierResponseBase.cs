using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Tradier.Converters;
using Tradier.Enumerations;

namespace Tradier.Response
{
    /// <summary>
    /// Base class for all Tradier API responses providing common functionality for HTTP response handling,
    /// JSON deserialization, error handling, and validation.
    /// </summary>
    /// <typeparam name="TData">The type of data contained in the response.</typeparam>
    public abstract class TradierResponseBase<TData> : ITradierResponse<TData>
        where TData : class
    {
        private readonly Dictionary<string, string> _headers = new();

        /// <summary>
        /// Gets the strongly-typed data from the API response.
        /// </summary>
        public TData? Data { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the API request was successful.
        /// </summary>
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// Gets the HTTP status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the error message if the request failed.
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Gets the response headers from the API call.
        /// </summary>
        public IReadOnlyDictionary<string, string> Headers => _headers.AsReadOnly();

        /// <summary>
        /// Gets a value indicating whether the response contains valid data.
        /// </summary>
        public bool HasData => Data != null;

        /// <summary>
        /// Gets validation errors if the data failed validation.
        /// </summary>
        public IEnumerable<ValidationResult>? ValidationErrors { get; private set; }

        /// <summary>
        /// Parses the HTTP response message asynchronously.
        /// </summary>
        /// <param name="response">The HTTP response message to parse.</param>
        /// <param name="options">Optional JSON serializer options.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>True if parsing was successful, false otherwise.</returns>
        public async Task<bool> ParseAsync(HttpResponseMessage response, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            if (response == null)
            {
                ErrorMessage = "HTTP response is null";
                return false;
            }

            try
            {
                // Capture response metadata
                StatusCode = response.StatusCode;
                IsSuccessful = response.IsSuccessStatusCode;

                // Extract relevant headers
                ExtractHeaders(response);

                // Handle non-successful responses
                if (!IsSuccessful)
                {
                    await HandleErrorResponseAsync(response, cancellationToken);
                    return false;
                }

                // Parse successful response
                return await ParseSuccessfulResponseAsync(response, options, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                ErrorMessage = "Operation was cancelled";
                return false;
            }
            catch (JsonException ex)
            {
                ErrorMessage = $"JSON deserialization failed: {ex.Message}";
                return false;
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = $"HTTP request failed: {ex.Message}";
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Unexpected error during response parsing: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Validates the parsed data using data annotations.
        /// </summary>
        /// <returns>True if data is valid, false otherwise.</returns>
        public bool ValidateData()
        {
            if (Data == null) return false;

            var validationContext = new ValidationContext(Data);
            var validationResults = new List<ValidationResult>();
            
            bool isValid = Validator.TryValidateObject(Data, validationContext, validationResults, validateAllProperties: true);
            
            if (!isValid)
            {
                ValidationErrors = validationResults;
            }

            return isValid;
        }

        /// <summary>
        /// Gets the data with validation, throwing an exception if invalid.
        /// </summary>
        /// <returns>The validated data.</returns>
        /// <exception cref="InvalidOperationException">Thrown when data is not available.</exception>
        /// <exception cref="ValidationException">Thrown when data fails validation.</exception>
        public TData GetValidatedData()
        {
            if (!HasData)
            {
                throw new InvalidOperationException("No data available in response. Check IsSuccessful and ErrorMessage properties.");
            }

            if (!ValidateData())
            {
                var errorMessages = ValidationErrors?.Select(v => v.ErrorMessage) ?? new[] { "Unknown validation error" };
                throw new ValidationException($"Response data validation failed: {string.Join("; ", errorMessages)}");
            }

            return Data!;
        }

        /// <summary>
        /// Extracts relevant headers from the HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        protected virtual void ExtractHeaders(HttpResponseMessage response)
        {
            // Standard headers
            foreach (var header in response.Headers)
            {
                _headers[header.Key] = string.Join(", ", header.Value);
            }

            // Content headers
            if (response.Content?.Headers != null)
            {
                foreach (var header in response.Content.Headers)
                {
                    _headers[header.Key] = string.Join(", ", header.Value);
                }
            }

            // Extract Tradier-specific headers
            if (response.Headers.TryGetValues("X-RateLimit-Available", out var rateLimitValues))
            {
                _headers["RateLimit-Available"] = rateLimitValues.First();
            }

            if (response.Headers.TryGetValues("X-RateLimit-Expiry", out var rateLimitExpiry))
            {
                _headers["RateLimit-Expiry"] = rateLimitExpiry.First();
            }
        }

        /// <summary>
        /// Handles error responses from the API.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        protected virtual async Task HandleErrorResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            try
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                
                // Try to parse structured error response
                if (!string.IsNullOrWhiteSpace(errorContent))
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(errorContent);
                        if (doc.RootElement.TryGetProperty("fault", out var fault))
                        {
                            if (fault.TryGetProperty("faultstring", out var faultString))
                            {
                                ErrorMessage = $"API Error: {faultString.GetString()}";
                                return;
                            }
                        }
                        
                        // Fallback to raw error content
                        ErrorMessage = $"API Error ({StatusCode}): {errorContent}";
                    }
                    catch (JsonException)
                    {
                        // If JSON parsing fails, use raw content
                        ErrorMessage = $"API Error ({StatusCode}): {errorContent}";
                    }
                }
                else
                {
                    ErrorMessage = $"API Error: {StatusCode} - {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to read error response: {ex.Message}";
            }
        }

        /// <summary>
        /// Parses a successful HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <param name="options">JSON serializer options.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>True if parsing was successful, false otherwise.</returns>
        protected virtual async Task<bool> ParseSuccessfulResponseAsync(HttpResponseMessage response, JsonSerializerOptions? options, CancellationToken cancellationToken)
        {
            // Handle empty responses
            if (response.Content.Headers.ContentLength == 0)
            {
                return HandleEmptyResponse();
            }

            // Get JSON serializer options
            var jsonOptions = options ?? GetDefaultJsonOptions();

            // Stream deserialization for better memory efficiency
            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            
            // Handle special null response cases that Tradier API sometimes returns
            var streamReader = new StreamReader(stream);
            var content = await streamReader.ReadToEndAsync(cancellationToken);
            
            if (IsNullResponse(content))
            {
                return HandleNullResponse();
            }

            // Reset stream position for deserialization
            stream.Position = 0;
            
            // Deserialize using the custom deserialization logic
            Data = await DeserializeResponseAsync(stream, jsonOptions, cancellationToken);
            
            return Data != null;
        }

        /// <summary>
        /// Deserializes the response stream to the target data type.
        /// Override this method to implement custom deserialization logic.
        /// </summary>
        /// <param name="stream">The response content stream.</param>
        /// <param name="options">JSON serializer options.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The deserialized data object.</returns>
        protected virtual async Task<TData?> DeserializeResponseAsync(Stream stream, JsonSerializerOptions options, CancellationToken cancellationToken)
        {
            return await JsonSerializer.DeserializeAsync<TData>(stream, options, cancellationToken);
        }

        /// <summary>
        /// Handles empty responses from the API.
        /// Override this method to provide custom empty response handling.
        /// </summary>
        /// <returns>True if empty response is acceptable, false otherwise.</returns>
        protected virtual bool HandleEmptyResponse()
        {
            // Most APIs return empty responses for DELETE operations or when no data is available
            return true;
        }

        /// <summary>
        /// Handles null responses that Tradier API sometimes returns (e.g., {"orders":"null"}).
        /// Override this method to provide custom null response handling.
        /// </summary>
        /// <returns>True if null response is acceptable, false otherwise.</returns>
        protected virtual bool HandleNullResponse()
        {
            // Create an empty data object for null responses
            try
            {
                Data = CreateEmptyDataObject();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the response content represents a null response from the API.
        /// </summary>
        /// <param name="content">The response content string.</param>
        /// <returns>True if the response is considered a null response.</returns>
        protected virtual bool IsNullResponse(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return true;
            
            // Handle Tradier's specific null response patterns
            return content.Trim() switch
            {
                "{\"orders\":\"null\"}" => true,
                "{\"history\":\"null\"}" => true,
                "{\"positions\":\"null\"}" => true,
                "{\"watchlists\":\"null\"}" => true,
                _ => false
            };
        }

        /// <summary>
        /// Creates an empty data object for null responses.
        /// Override this method to provide custom empty object creation.
        /// </summary>
        /// <returns>An empty data object.</returns>
        protected virtual TData? CreateEmptyDataObject()
        {
            return Activator.CreateInstance<TData>();
        }

        /// <summary>
        /// Gets the default JSON serializer options for this response type.
        /// Override this method to provide custom JSON serialization options.
        /// </summary>
        /// <returns>JSON serializer options.</returns>
        protected virtual JsonSerializerOptions GetDefaultJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                WriteIndented = false,
                Converters =
                {
                    new EnumStringConverter<AccountStatus>(),
                    new EnumStringConverter<AccountType>(),
                    new EnumStringConverter<AccountClassification>(),
                    new EnumStringConverter<OrderType>(),
                    new EnumStringConverter<OrderSide>(),
                    new EnumStringConverter<OrderStatus>(),
                    new EnumStringConverter<OrderDuration>(),
                    new EnumStringConverter<OrderClass>(),
                    new EnumStringConverter<SecurityType>(),
                    new EnumStringConverter<OptionType>(),
                    new UnixTimestampConverter()
                }
            };
        }
    }
}