using System.Text.Json.Serialization;

namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents the side of a trading order (buy or sell).
    /// </summary>
    public enum OrderSide
    {
        /// <summary>
        /// Order side is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Buy order - opening a long position or closing a short position.
        /// </summary>
        [JsonPropertyName("buy")]
        Buy,

        /// <summary>
        /// Sell order - closing a long position or opening a short position.
        /// </summary>
        [JsonPropertyName("sell")]
        Sell,

        /// <summary>
        /// Buy to open - opening a new long position.
        /// </summary>
        [JsonPropertyName("buy_to_open")]
        BuyToOpen,

        /// <summary>
        /// Buy to close - closing an existing short position.
        /// </summary>
        [JsonPropertyName("buy_to_close")]
        BuyToClose,

        /// <summary>
        /// Sell to open - opening a new short position.
        /// </summary>
        [JsonPropertyName("sell_to_open")]
        SellToOpen,

        /// <summary>
        /// Sell to close - closing an existing long position.
        /// </summary>
        [JsonPropertyName("sell_to_close")]
        SellToClose
    }
}