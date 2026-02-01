using System.Text.Json.Serialization;

namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents the class or category of a trading order.
    /// </summary>
    public enum OrderClass
    {
        /// <summary>
        /// Order class is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Equity order (stocks, ETFs).
        /// </summary>
        [JsonPropertyName("equity")]
        Equity,

        /// <summary>
        /// Options order.
        /// </summary>
        [JsonPropertyName("option")]
        Option,

        /// <summary>
        /// Multi-leg options order.
        /// </summary>
        [JsonPropertyName("multileg")]
        MultiLeg,

        /// <summary>
        /// Combo order (combination of equity and options).
        /// </summary>
        [JsonPropertyName("combo")]
        Combo
    }
}