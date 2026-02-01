using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tradier.Converters;
using Tradier.Enumerations;

namespace Tradier.Model
{
    /// <summary>
    /// Represents a real-time or delayed quote for a security, including price, volume, and options-specific data.
    /// </summary>
    public class Quote : Security
    {
        /// <summary>
        /// Gets or sets the exchange where this security is traded (short form).
        /// </summary>
        [JsonPropertyName("exch")]
        [StringLength(10, ErrorMessage = "Exchange name cannot exceed 10 characters")]
        public override string Exchange { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last trade price.
        /// </summary>
        [JsonPropertyName("last")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "Last price must be non-negative")]
        public decimal? LastPrice { get; set; }

        /// <summary>
        /// Gets or sets the change in price from the previous close.
        /// </summary>
        [JsonPropertyName("change")]
        public decimal? Change { get; set; }

        /// <summary>
        /// Gets or sets the total volume traded today.
        /// </summary>
        [JsonPropertyName("volume")]
        [Range(0, long.MaxValue, ErrorMessage = "Volume must be non-negative")]
        public long Volume { get; set; }

        /// <summary>
        /// Gets or sets the opening price for the trading session.
        /// </summary>
        [JsonPropertyName("open")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "Open price must be non-negative")]
        public decimal? OpenPrice { get; set; }

        /// <summary>
        /// Gets or sets the highest price during the trading session.
        /// </summary>
        [JsonPropertyName("high")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "High price must be non-negative")]
        public decimal? HighPrice { get; set; }

        /// <summary>
        /// Gets or sets the lowest price during the trading session.
        /// </summary>
        [JsonPropertyName("low")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "Low price must be non-negative")]
        public decimal? LowPrice { get; set; }

        /// <summary>
        /// Gets or sets the closing price (may be null for intraday quotes).
        /// </summary>
        [JsonPropertyName("close")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "Close price must be non-negative")]
        public decimal? ClosePrice { get; set; }

        /// <summary>
        /// Gets or sets the current bid price.
        /// </summary>
        [JsonPropertyName("bid")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "Bid price must be non-negative")]
        public decimal? BidPrice { get; set; }

        /// <summary>
        /// Gets or sets the current ask price.
        /// </summary>
        [JsonPropertyName("ask")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "Ask price must be non-negative")]
        public decimal? AskPrice { get; set; }

        /// <summary>
        /// Gets or sets the percentage change from the previous close.
        /// </summary>
        [JsonPropertyName("change_percentage")]
        public decimal? ChangePercentage { get; set; }

        /// <summary>
        /// Gets or sets the average daily trading volume.
        /// </summary>
        [JsonPropertyName("average_volume")]
        [Range(0, long.MaxValue, ErrorMessage = "Average volume must be non-negative")]
        public long? AverageVolume { get; set; }

        /// <summary>
        /// Gets or sets the volume of the last trade.
        /// </summary>
        [JsonPropertyName("last_volume")]
        [Range(0, int.MaxValue, ErrorMessage = "Last volume must be non-negative")]
        public int? LastVolume { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the last trade.
        /// </summary>
        [JsonPropertyName("trade_date")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime? TradeDate { get; set; }

        /// <summary>
        /// Gets or sets the previous closing price.
        /// </summary>
        [JsonPropertyName("prevclose")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "Previous close must be non-negative")]
        public decimal? PreviousClose { get; set; }

        /// <summary>
        /// Gets or sets the 52-week high price.
        /// </summary>
        [JsonPropertyName("week_52_high")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "52-week high must be non-negative")]
        public decimal? Week52High { get; set; }

        /// <summary>
        /// Gets or sets the 52-week low price.
        /// </summary>
        [JsonPropertyName("week_52_low")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "52-week low must be non-negative")]
        public decimal? Week52Low { get; set; }

        /// <summary>
        /// Gets or sets the size of the current bid.
        /// </summary>
        [JsonPropertyName("bidsize")]
        [Range(0, int.MaxValue, ErrorMessage = "Bid size must be non-negative")]
        public int? BidSize { get; set; }

        /// <summary>
        /// Gets or sets the exchange providing the current bid.
        /// </summary>
        [JsonPropertyName("bidexch")]
        [StringLength(10, ErrorMessage = "Bid exchange name cannot exceed 10 characters")]
        public string? BidExchange { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the current bid.
        /// </summary>
        [JsonPropertyName("bid_date")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime? BidDate { get; set; }

        /// <summary>
        /// Gets or sets the size of the current ask.
        /// </summary>
        [JsonPropertyName("asksize")]
        [Range(0, int.MaxValue, ErrorMessage = "Ask size must be non-negative")]
        public int? AskSize { get; set; }

        /// <summary>
        /// Gets or sets the exchange providing the current ask.
        /// </summary>
        [JsonPropertyName("askexch")]
        [StringLength(10, ErrorMessage = "Ask exchange name cannot exceed 10 characters")]
        public string? AskExchange { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the current ask.
        /// </summary>
        [JsonPropertyName("ask_date")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime? AskDate { get; set; }

        // Options-specific properties

        /// <summary>
        /// Gets or sets the root symbols for options chains.
        /// </summary>
        [JsonPropertyName("root_symbols")]
        [StringLength(100, ErrorMessage = "Root symbols cannot exceed 100 characters")]
        public string? RootSymbols { get; set; }

        /// <summary>
        /// Gets or sets the underlying security symbol for options.
        /// </summary>
        [JsonPropertyName("underlying")]
        [RegularExpression(@"^[A-Z0-9._]{1,32}$", ErrorMessage = "Underlying symbol must contain only uppercase letters, numbers, dots, and underscores")]
        public string? UnderlyingSymbol { get; set; }

        /// <summary>
        /// Gets or sets the strike price for options contracts.
        /// </summary>
        [JsonPropertyName("strike")]
        [Range(typeof(decimal), "0.01", "99999.99", ErrorMessage = "Strike price must be greater than 0")]
        public decimal? StrikePrice { get; set; }

        /// <summary>
        /// Gets or sets the open interest for options contracts.
        /// </summary>
        [JsonPropertyName("open_interest")]
        [Range(0, int.MaxValue, ErrorMessage = "Open interest must be non-negative")]
        public int? OpenInterest { get; set; }

        /// <summary>
        /// Gets or sets the contract size (typically 100 for equity options).
        /// </summary>
        [JsonPropertyName("contract_size")]
        [Range(1, 10000, ErrorMessage = "Contract size must be between 1 and 10,000")]
        public int ContractSize { get; set; } = 100;

        /// <summary>
        /// Gets or sets the expiration date for options contracts.
        /// </summary>
        [JsonPropertyName("expiration_date")]
        public string? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration type (e.g., "standard", "weekly").
        /// </summary>
        [JsonPropertyName("expiration_type")]
        [StringLength(20, ErrorMessage = "Expiration type cannot exceed 20 characters")]
        public string? ExpirationType { get; set; }

        /// <summary>
        /// Gets or sets the option type (Call or Put).
        /// </summary>
        [JsonPropertyName("option_type")]
        [JsonConverter(typeof(EnumStringConverter<OptionType>))]
        public OptionType OptionType { get; set; } = OptionType.Unknown;

        /// <summary>
        /// Gets or sets the root symbol for the options chain.
        /// </summary>
        [JsonPropertyName("root_symbol")]
        [RegularExpression(@"^[A-Z0-9._]{1,32}$", ErrorMessage = "Root symbol must contain only uppercase letters, numbers, dots, and underscores")]
        public string? RootSymbol { get; set; }

        // Calculated properties

        /// <summary>
        /// Gets the bid-ask spread.
        /// </summary>
        [JsonIgnore]
        public decimal? BidAskSpread => AskPrice.HasValue && BidPrice.HasValue ? AskPrice - BidPrice : null;

        /// <summary>
        /// Gets the midpoint between bid and ask prices.
        /// </summary>
        [JsonIgnore]
        public decimal? MidPrice => AskPrice.HasValue && BidPrice.HasValue ? (AskPrice + BidPrice) / 2 : null;

        /// <summary>
        /// Gets a value indicating whether this quote represents an actively trading security.
        /// </summary>
        [JsonIgnore]
        public bool IsActivelyTrading => Volume > 0 && BidPrice.HasValue && AskPrice.HasValue;

        /// <summary>
        /// Gets a value indicating whether this is an in-the-money option.
        /// </summary>
        [JsonIgnore]
        public bool IsInTheMoney
        {
            get
            {
                if (!IsOption || !StrikePrice.HasValue || !LastPrice.HasValue) return false;
                
                // For options, we need the underlying price to determine ITM status
                // This is a simplified check - in reality, you'd compare strike to underlying price
                return LastPrice > 0;
            }
        }
    }
}
