using System.Net;
using System.Text.Json;

namespace Tradier.Response
{
    /// <summary>
    /// Interface for all Tradier API response objects.
    /// </summary>
    public interface ITradierResponse
    {
        /// <summary>
        /// Gets a value indicating whether the API request was successful.
        /// </summary>
        bool IsSuccessful { get; }

        /// <summary>
        /// Gets the HTTP status code of the response.
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the error message if the request failed.
        /// </summary>
        string? ErrorMessage { get; }

        /// <summary>
        /// Gets the response headers from the API call.
        /// </summary>
        IReadOnlyDictionary<string, string> Headers { get; }

        /// <summary>
        /// Parses the HTTP response message asynchronously.
        /// </summary>
        /// <param name="response">The HTTP response message to parse.</param>
        /// <param name="options">Optional JSON serializer options.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>True if parsing was successful, false otherwise.</returns>
        Task<bool> ParseAsync(HttpResponseMessage response, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Generic interface for typed Tradier API responses.
    /// </summary>
    /// <typeparam name="TData">The type of data contained in the response.</typeparam>
    public interface ITradierResponse<out TData> : ITradierResponse
        where TData : class
    {
        /// <summary>
        /// Gets the strongly-typed data from the API response.
        /// </summary>
        TData? Data { get; }
    }
}