using System.Text.Json.Serialization;

namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents the current status of a trading order.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order status is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Order has been submitted but not yet processed.
        /// </summary>
        [JsonPropertyName("pending")]
        Pending,

        /// <summary>
        /// Order has been accepted and is active in the market.
        /// </summary>
        [JsonPropertyName("open")]
        Open,

        /// <summary>
        /// Order has been partially executed.
        /// </summary>
        [JsonPropertyName("partially_filled")]
        PartiallyFilled,

        /// <summary>
        /// Order has been completely executed.
        /// </summary>
        [JsonPropertyName("filled")]
        Filled,

        /// <summary>
        /// Order has been cancelled.
        /// </summary>
        [JsonPropertyName("canceled")]
        Canceled,

        /// <summary>
        /// Order has been rejected by the exchange or broker.
        /// </summary>
        [JsonPropertyName("rejected")]
        Rejected,

        /// <summary>
        /// Order has expired without being filled.
        /// </summary>
        [JsonPropertyName("expired")]
        Expired,

        /// <summary>
        /// Order is being processed for submission.
        /// </summary>
        [JsonPropertyName("submitted")]
        Submitted
    }
}