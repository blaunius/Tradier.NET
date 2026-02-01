using System.Text.Json.Serialization;

namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents the different types of securities that can be traded.
    /// </summary>
    public enum SecurityType
    {
        /// <summary>
        /// Security type is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Common stock equity security.
        /// </summary>
        [JsonPropertyName("stock")]
        Stock,

        /// <summary>
        /// Exchange-traded fund.
        /// </summary>
        [JsonPropertyName("etf")]
        ETF,

        /// <summary>
        /// Options contract.
        /// </summary>
        [JsonPropertyName("option")]
        Option,

        /// <summary>
        /// Mutual fund.
        /// </summary>
        [JsonPropertyName("mutual_fund")]
        MutualFund,

        /// <summary>
        /// Bond or fixed income security.
        /// </summary>
        [JsonPropertyName("bond")]
        Bond,

        /// <summary>
        /// Index security.
        /// </summary>
        [JsonPropertyName("index")]
        Index
    }
}