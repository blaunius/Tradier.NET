using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tradier.Converters;
using Tradier.Enumerations;

namespace Tradier.Model
{
    /// <summary>
    /// Represents a Tradier trading account with its properties and metadata.
    /// </summary>
    public class Account : TradierModelBase
    {
        /// <summary>
        /// Gets or sets the unique account number.
        /// </summary>
        [JsonPropertyName("account_number")]
        [Required(ErrorMessage = "Account number is required")]
        [RegularExpression(@"^[A-Z0-9]{8,12}$", ErrorMessage = "Account number must be 8-12 alphanumeric characters")]
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account classification (Individual, Joint, Corporate, etc.).
        /// </summary>
        [JsonPropertyName("classification")]
        [JsonConverter(typeof(EnumStringConverter<AccountClassification>))]
        public AccountClassification Classification { get; set; } = AccountClassification.Unknown;

        /// <summary>
        /// Gets or sets the date when the account was created.
        /// </summary>
        [JsonPropertyName("date_created")]
        [Required(ErrorMessage = "Date created is required")]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this account is flagged as a day trading account.
        /// </summary>
        [JsonPropertyName("day_trader")]
        public bool IsDayTrader { get; set; }

        /// <summary>
        /// Gets or sets the options trading level (0-5, where higher levels allow more complex strategies).
        /// </summary>
        [JsonPropertyName("option_level")]
        [Range(0, 5, ErrorMessage = "Option level must be between 0 and 5")]
        public int OptionLevel { get; set; }

        /// <summary>
        /// Gets or sets the current status of the account.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumStringConverter<AccountStatus>))]
        public AccountStatus Status { get; set; } = AccountStatus.Unknown;

        /// <summary>
        /// Gets or sets the type of account (Cash, Margin, IRA, etc.).
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(EnumStringConverter<AccountType>))]
        public AccountType Type { get; set; } = AccountType.Unknown;

        /// <summary>
        /// Gets or sets the date when the account was last updated.
        /// </summary>
        [JsonPropertyName("last_update_date")]
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// Gets a value indicating whether this account is active and operational.
        /// </summary>
        [JsonIgnore]
        public bool IsActive => Status == AccountStatus.Active;

        /// <summary>
        /// Gets a value indicating whether this account supports options trading.
        /// </summary>
        [JsonIgnore]
        public bool SupportsOptions => OptionLevel > 0;

        /// <summary>
        /// Gets a value indicating whether this account supports margin trading.
        /// </summary>
        [JsonIgnore]
        public bool SupportsMargin => Type == AccountType.Margin;
    }
}
