
using Tradier.Response;

namespace Tradier
{
    /// <summary>
    /// Interface for Tradier HTTP client providing methods to interact with the Tradier API.
    /// </summary>
    public interface ITradierClient
    {
        /// <summary>
        /// Sends a GET request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TData">The response type.</typeparam>
        /// <param name="endpoint">The API endpoint to request.</param>
        /// <param name="token">Cancellation token for the request.</param>
        /// <returns>The typed response from the API.</returns>
        Task<TData> Get<TData>(
              string endpoint, 
              CancellationToken token = default) where TData : ITradierResponse, new();

        /// <summary>
        /// Sends a POST request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TData">The response type.</typeparam>
        /// <param name="endpoint">The API endpoint to request.</param>
        /// <param name="body">Optional form data to send with the request.</param>
        /// <param name="token">Cancellation token for the request.</param>
        /// <returns>The typed response from the API.</returns>
        Task<TData> Post<TData>(
              string endpoint, 
              Dictionary<string, string>? body = null, 
              CancellationToken token = default) where TData : ITradierResponse, new();

        /// <summary>
        /// Sends a PUT request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TData">The response type.</typeparam>
        /// <param name="endpoint">The API endpoint to request.</param>
        /// <param name="body">Optional form data to send with the request.</param>
        /// <param name="token">Cancellation token for the request.</param>
        /// <returns>The typed response from the API.</returns>
        Task<TData> Put<TData>(
              string endpoint,
              Dictionary<string, string>? body = null,
              CancellationToken token = default) where TData : ITradierResponse, new();

        /// <summary>
        /// Sends a DELETE request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TData">The response type.</typeparam>
        /// <param name="endpoint">The API endpoint to request.</param>
        /// <param name="token">Cancellation token for the request.</param>
        /// <returns>The typed response from the API.</returns>
        Task<TData> Delete<TData>(
              string endpoint,
              CancellationToken token = default) where TData : ITradierResponse, new();
    }
}