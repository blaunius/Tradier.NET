using System.Text.Json.Serialization;

namespace Tradier.Response
{
    /// <summary>
    /// Response from creating a streaming session.
    /// </summary>
    public class StreamingSessionResponse : TradierResponseBase<StreamingSession>
    {
        [JsonPropertyName("stream")]
        public StreamingSession? Stream { get => Data; set => Data = value; }
    }

    /// <summary>
    /// Streaming session information.
    /// </summary>
    public class StreamingSession
    {
        /// <summary>
        /// The session ID to use for streaming requests.
        /// </summary>
        [JsonPropertyName("sessionid")]
        public string? SessionId { get; set; }

        /// <summary>
        /// The URL to connect to for streaming.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
