using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Tradier.Response;

namespace Tradier.Services
{
    /// <summary>
    /// Service for streaming real-time market data and account events.
    /// Note: Streaming is only available with the production client, not sandbox.
    /// </summary>
    public class StreamingService : TradierService
    {
        private const string STREAM_BASE_URL = "https://stream.tradier.com/v1/";
        private const string WEBSOCKET_URL = "wss://ws.tradier.com/v1/";

        /// <summary>
        /// Creates a new StreamingService with the specified client.
        /// </summary>
        /// <param name="tradierClient">The Tradier API client to use (must be production client, not sandbox).</param>
        /// <exception cref="NotSupportedException">Thrown when attempting to use with a sandbox client.</exception>
        public StreamingService(ITradierClient tradierClient) : base(tradierClient)
        {
            if (tradierClient is TradierSandboxClient)
                throw new NotSupportedException("Streaming is only available with the production client, not sandbox.");
        }

        /// <summary>
        /// Creates a streaming session for market data.
        /// The returned session ID should be used immediately as it expires after 5 minutes.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A streaming session containing the session ID.</returns>
        public Task<StreamingSessionResponse> CreateMarketSession(CancellationToken token = default)
        {
            return client.Post<StreamingSessionResponse>("markets/events/session", null, token);
        }

        /// <summary>
        /// Creates a streaming session for account events.
        /// The returned session ID should be used immediately as it expires after 5 minutes.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A streaming session containing the session ID.</returns>
        public Task<StreamingSessionResponse> CreateAccountSession(CancellationToken token = default)
        {
            return client.Post<StreamingSessionResponse>("accounts/events/session", null, token);
        }

        /// <summary>
        /// Streams market events via HTTP streaming.
        /// This method returns an async enumerable that yields JSON strings for each market event.
        /// </summary>
        /// <param name="sessionId">The session ID from CreateMarketSession().</param>
        /// <param name="symbols">Symbols to stream (e.g., "AAPL", "MSFT").</param>
        /// <param name="filter">Event types to filter. Options: trade, quote, summary, timesale, tradex. Default is all.</param>
        /// <param name="linebreak">If true, events are separated by newlines. Recommended for easier parsing.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>An async enumerable of JSON event strings.</returns>
        public async IAsyncEnumerable<string> StreamMarketEvents(
            string sessionId,
            IEnumerable<string> symbols,
            IEnumerable<string>? filter = null,
            bool linebreak = true,
            [EnumeratorCancellation] CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(sessionId))
                throw new ArgumentNullException(nameof(sessionId));
            if (symbols == null || !symbols.Any())
                throw new ArgumentException("At least one symbol is required.", nameof(symbols));

            using var httpClient = new HttpClient();
            httpClient.Timeout = Timeout.InfiniteTimeSpan;

            var parameters = new Dictionary<string, string>
            {
                ["sessionid"] = sessionId,
                ["symbols"] = string.Join(",", symbols),
                ["linebreak"] = linebreak.ToString().ToLower()
            };

            if (filter != null && filter.Any())
            {
                parameters["filter"] = string.Join(",", filter);
            }

            var content = new FormUrlEncodedContent(parameters);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{STREAM_BASE_URL}markets/events")
            {
                Content = content
            };

            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(token);
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = await reader.ReadLineAsync(token)) != null && !token.IsCancellationRequested)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// Streams account events via HTTP streaming.
        /// This method returns an async enumerable that yields JSON strings for each account event.
        /// </summary>
        /// <param name="sessionId">The session ID from CreateAccountSession().</param>
        /// <param name="accountIds">Account IDs to stream events for.</param>
        /// <param name="linebreak">If true, events are separated by newlines. Recommended for easier parsing.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>An async enumerable of JSON event strings.</returns>
        public async IAsyncEnumerable<string> StreamAccountEvents(
            string sessionId,
            IEnumerable<string> accountIds,
            bool linebreak = true,
            [EnumeratorCancellation] CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(sessionId))
                throw new ArgumentNullException(nameof(sessionId));
            if (accountIds == null || !accountIds.Any())
                throw new ArgumentException("At least one account ID is required.", nameof(accountIds));

            using var httpClient = new HttpClient();
            httpClient.Timeout = Timeout.InfiniteTimeSpan;

            var parameters = new Dictionary<string, string>
            {
                ["sessionid"] = sessionId,
                ["account_id"] = string.Join(",", accountIds),
                ["linebreak"] = linebreak.ToString().ToLower()
            };

            var content = new FormUrlEncodedContent(parameters);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{STREAM_BASE_URL}accounts/events")
            {
                Content = content
            };

            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(token);
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = await reader.ReadLineAsync(token)) != null && !token.IsCancellationRequested)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// Gets the WebSocket URL for market data streaming.
        /// Use this URL with a WebSocket client and send a JSON payload with sessionid, symbols, and filter.
        /// </summary>
        /// <returns>The WebSocket URL for market streaming.</returns>
        public string GetMarketWebSocketUrl() => $"{WEBSOCKET_URL}markets/events";

        /// <summary>
        /// Gets the WebSocket URL for account data streaming.
        /// Use this URL with a WebSocket client and send a JSON payload with sessionid and account_id.
        /// </summary>
        /// <returns>The WebSocket URL for account streaming.</returns>
        public string GetAccountWebSocketUrl() => $"{WEBSOCKET_URL}accounts/events";

        /// <summary>
        /// Creates a JSON payload for WebSocket market streaming.
        /// </summary>
        /// <param name="sessionId">The session ID from CreateMarketSession().</param>
        /// <param name="symbols">Symbols to stream.</param>
        /// <param name="filter">Event types to filter. Options: trade, quote, summary, timesale, tradex.</param>
        /// <param name="linebreak">If true, events are separated by newlines.</param>
        /// <returns>A JSON string to send to the WebSocket.</returns>
        public string CreateMarketWebSocketPayload(
            string sessionId,
            IEnumerable<string> symbols,
            IEnumerable<string>? filter = null,
            bool linebreak = true)
        {
            var payload = new Dictionary<string, object>
            {
                ["sessionid"] = sessionId,
                ["symbols"] = symbols.ToArray(),
                ["linebreak"] = linebreak
            };

            if (filter != null && filter.Any())
            {
                payload["filter"] = filter.ToArray();
            }

            return JsonSerializer.Serialize(payload);
        }

        /// <summary>
        /// Creates a JSON payload for WebSocket account streaming.
        /// </summary>
        /// <param name="sessionId">The session ID from CreateAccountSession().</param>
        /// <param name="accountIds">Account IDs to stream events for.</param>
        /// <param name="linebreak">If true, events are separated by newlines.</param>
        /// <returns>A JSON string to send to the WebSocket.</returns>
        public string CreateAccountWebSocketPayload(
            string sessionId,
            IEnumerable<string> accountIds,
            bool linebreak = true)
        {
            var payload = new Dictionary<string, object>
            {
                ["sessionid"] = sessionId,
                ["account_id"] = accountIds.ToArray(),
                ["linebreak"] = linebreak
            };

            return JsonSerializer.Serialize(payload);
        }
    }

    /// <summary>
    /// Filter types for market data streaming.
    /// </summary>
    public static class StreamingFilters
    {
        /// <summary>Trade events.</summary>
        public const string Trade = "trade";
        
        /// <summary>Quote events (bid/ask).</summary>
        public const string Quote = "quote";
        
        /// <summary>Summary events (daily stats).</summary>
        public const string Summary = "summary";
        
        /// <summary>Time and sales events.</summary>
        public const string TimeSale = "timesale";
        
        /// <summary>Extended trade events.</summary>
        public const string TradeX = "tradex";
    }
}
