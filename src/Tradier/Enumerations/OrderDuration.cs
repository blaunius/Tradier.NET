using System.Text.Json.Serialization;

namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents how long an order should remain active.
    /// </summary>
    public enum OrderDuration
    {
        /// <summary>
        /// Duration is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Good for the current trading day only.
        /// </summary>
        [JsonPropertyName("day")]
        Day,

        /// <summary>
        /// Good until cancelled by the trader.
        /// </summary>
        [JsonPropertyName("gtc")]
        GoodTillCanceled,

        /// <summary>
        /// Pre-market order.
        /// </summary>
        [JsonPropertyName("pre")]
        PreMarket,

        /// <summary>
        /// After-hours order.
        /// </summary>
        [JsonPropertyName("post")]
        PostMarket
    }
}