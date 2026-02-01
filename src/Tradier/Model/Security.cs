using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tradier.Converters;
using Tradier.Enumerations;

namespace Tradier.Model
{
    /// <summary>
    /// Represents a tradeable security with its basic properties.
    /// </summary>
    public class Security : TradierModelBase
    {
        /// <summary>
        /// Gets or sets the trading symbol for this security.
        /// </summary>
        [JsonPropertyName("symbol")]
        [Required(ErrorMessage = "Security symbol is required")]
        [RegularExpression(@"^[A-Z0-9._]{1,32}$", ErrorMessage = "Symbol must contain only uppercase letters, numbers, dots, and underscores")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the human-readable description of this security.
        /// </summary>
        [JsonPropertyName("description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the exchange where this security is traded.
        /// </summary>
        [JsonPropertyName("exchange")]
        [StringLength(10, ErrorMessage = "Exchange name cannot exceed 10 characters")]
        public virtual string Exchange { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of security (Stock, Option, ETF, etc.).
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(EnumStringConverter<SecurityType>))]
        public SecurityType Type { get; set; } = SecurityType.Unknown;

        /// <summary>
        /// Gets a value indicating whether this is an options contract.
        /// </summary>
        [JsonIgnore]
        public bool IsOption => Type == SecurityType.Option;

        /// <summary>
        /// Gets a value indicating whether this is an equity security (stock or ETF).
        /// </summary>
        [JsonIgnore]
        public bool IsEquity => Type == SecurityType.Stock || Type == SecurityType.ETF;
    }
}
