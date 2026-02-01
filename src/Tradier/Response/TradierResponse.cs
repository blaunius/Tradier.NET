using System.Net;
using System.Text.Json;

namespace Tradier.Response
{
    /// <summary>
    /// Base class for all Tradier API responses.
    /// Provides common functionality for parsing HTTP responses and error handling.
    /// </summary>
    public abstract class TradierResponse : ITradierResponse
    {
        private readonly Dictionary<string, string> _headers = new();
        
        public string RawResponse { get; internal set; } = string.Empty;
        public bool IsSuccessful { get; internal set; }
        public HttpStatusCode StatusCode { get; internal set; }
        public string? ErrorMessage { get; internal set; }
        public IReadOnlyDictionary<string, string> Headers => _headers.AsReadOnly();

        internal async Task Parse(HttpResponseMessage rs)
        {
            await ParseAsync(rs);
        }

        public async Task<bool> ParseAsync(HttpResponseMessage response, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                StatusCode = response.StatusCode;
                IsSuccessful = response.IsSuccessStatusCode;
                
                // Extract headers
                foreach (var header in response.Headers)
                {
                    _headers[header.Key] = string.Join(", ", header.Value);
                }

                RawResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                
                if (IsSuccessful)
                {
                    Deserialize();
                }
                else
                {
                    ErrorMessage = $"API Error ({StatusCode}): {RawResponse}";
                }
                
                return IsSuccessful;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Response parsing failed: {ex.Message}";
                return false;
            }
        }

        internal virtual void Deserialize()
        {
            // Default implementation for backward compatibility
            // Subclasses should override this method
        }
    }
}
