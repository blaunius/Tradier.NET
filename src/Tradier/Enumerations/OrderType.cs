using System.Text.Json.Serialization;

namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents the different types of trading orders.
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Order type is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Market order - executes immediately at current market price.
        /// </summary>
        [JsonPropertyName("market")]
        Market,

        /// <summary>
        /// Limit order - executes only at specified price or better.
        /// </summary>
        [JsonPropertyName("limit")]
        Limit,

        /// <summary>
        /// Stop order - becomes market order when stop price is reached.
        /// </summary>
        [JsonPropertyName("stop")]
        Stop,

        /// <summary>
        /// Stop-limit order - becomes limit order when stop price is reached.
        /// </summary>
        [JsonPropertyName("stop_limit")]
        StopLimit,

        /// <summary>
        /// Debit spread order for options.
        /// </summary>
        [JsonPropertyName("debit")]
        Debit,

        /// <summary>
        /// Credit spread order for options.
        /// </summary>
        [JsonPropertyName("credit")]
        Credit,

        /// <summary>
        /// Even spread order for options.
        /// </summary>
        [JsonPropertyName("even")]
        Even
    }
}