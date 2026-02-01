using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tradier.Converters;
using Tradier.Enumerations;

namespace Tradier.Model
{
    /// <summary>
    /// Represents the balance information for a trading account, including cash, equity, and margin details.
    /// </summary>
    public class Balance : TradierModelBase
    {
        /// <summary>
        /// Gets or sets the total value of short option positions.
        /// </summary>
        [JsonPropertyName("option_short_value")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Option short value must be non-negative")]
        public decimal OptionShortValue { get; set; }

        /// <summary>
        /// Gets or sets the total equity in the account (cash + market value of positions).
        /// </summary>
        [JsonPropertyName("total_equity")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Total equity must be non-negative")]
        public decimal TotalEquity { get; set; }

        /// <summary>
        /// Gets or sets the account number this balance belongs to.
        /// </summary>
        [JsonPropertyName("account_number")]
        [Required(ErrorMessage = "Account number is required")]
        [RegularExpression(@"^[A-Z0-9]{8,12}$", ErrorMessage = "Account number must be 8-12 alphanumeric characters")]
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of account (Cash, Margin, IRA, etc.).
        /// </summary>
        [JsonPropertyName("account_type")]
        [JsonConverter(typeof(EnumStringConverter<AccountType>))]
        public AccountType AccountType { get; set; } = AccountType.Unknown;

        /// <summary>
        /// Gets or sets the realized profit/loss from closed positions.
        /// </summary>
        [JsonPropertyName("close_pl")]
        public decimal ClosedProfitLoss { get; set; }

        /// <summary>
        /// Gets or sets the current margin requirement.
        /// </summary>
        [JsonPropertyName("current_requirement")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Current requirement must be non-negative")]
        public decimal CurrentRequirement { get; set; }

        /// <summary>
        /// Gets or sets the total equity value.
        /// </summary>
        [JsonPropertyName("equity")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Equity must be non-negative")]
        public decimal Equity { get; set; }

        /// <summary>
        /// Gets or sets the market value of long positions.
        /// </summary>
        [JsonPropertyName("long_market_value")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Long market value must be non-negative")]
        public decimal LongMarketValue { get; set; }

        /// <summary>
        /// Gets or sets the total market value of all positions.
        /// </summary>
        [JsonPropertyName("market_value")]
        public decimal MarketValue { get; set; }

        /// <summary>
        /// Gets or sets the unrealized profit/loss from open positions.
        /// </summary>
        [JsonPropertyName("open_pl")]
        public decimal OpenProfitLoss { get; set; }

        /// <summary>
        /// Gets or sets the total value of long option positions.
        /// </summary>
        [JsonPropertyName("option_long_value")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Option long value must be non-negative")]
        public decimal OptionLongValue { get; set; }

        /// <summary>
        /// Gets or sets the margin requirement for option positions.
        /// </summary>
        [JsonPropertyName("option_requirement")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Option requirement must be non-negative")]
        public decimal OptionRequirement { get; set; }

        /// <summary>
        /// Gets or sets the number of pending orders.
        /// </summary>
        [JsonPropertyName("pending_orders_count")]
        [Range(0, int.MaxValue, ErrorMessage = "Pending orders count must be non-negative")]
        public int PendingOrdersCount { get; set; }

        /// <summary>
        /// Gets or sets the market value of short positions (typically negative).
        /// </summary>
        [JsonPropertyName("short_market_value")]
        public decimal ShortMarketValue { get; set; }

        /// <summary>
        /// Gets or sets the total value of long stock positions.
        /// </summary>
        [JsonPropertyName("stock_long_value")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Stock long value must be non-negative")]
        public decimal StockLongValue { get; set; }

        /// <summary>
        /// Gets or sets the total cash available in the account.
        /// </summary>
        [JsonPropertyName("total_cash")]
        public decimal TotalCash { get; set; }

        /// <summary>
        /// Gets or sets the amount of funds that have not yet cleared.
        /// </summary>
        [JsonPropertyName("uncleared_funds")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Uncleared funds must be non-negative")]
        public decimal UnclearedFunds { get; set; }

        /// <summary>
        /// Gets or sets the amount of cash that is pending settlement.
        /// </summary>
        [JsonPropertyName("pending_cash")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Pending cash must be non-negative")]
        public decimal PendingCash { get; set; }

        /// <summary>
        /// Gets or sets detailed margin information for the account.
        /// </summary>
        [JsonPropertyName("margin")]
        public Margin? Margin { get; set; }

        /// <summary>
        /// Gets or sets detailed cash information for the account.
        /// </summary>
        [JsonPropertyName("cash")]
        public Cash? Cash { get; set; }

        /// <summary>
        /// Gets or sets Pattern Day Trader information for the account.
        /// </summary>
        [JsonPropertyName("pdt")]
        public Pdt? PatternDayTrader { get; set; }

        /// <summary>
        /// Gets the total profit/loss (open + closed).
        /// </summary>
        [JsonIgnore]
        public decimal TotalProfitLoss => OpenProfitLoss + ClosedProfitLoss;

        /// <summary>
        /// Gets the available cash for trading (total cash - uncleared - pending).
        /// </summary>
        [JsonIgnore]
        public decimal AvailableCash => TotalCash - UnclearedFunds - PendingCash;

        /// <summary>
        /// Gets a value indicating whether this account has sufficient equity for day trading (PDT rule: $25,000 minimum).
        /// </summary>
        [JsonIgnore]
        public bool MeetsPatternDayTradingRequirement => TotalEquity >= 25000m;
    }
}
