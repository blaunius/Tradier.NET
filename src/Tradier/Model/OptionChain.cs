using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class OptionChain
    {
        /// <summary>
        /// Option contract symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; init; }

        /// <summary>
        /// Description of the contract.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; init; }

        /// <summary>
        /// Exchange code.
        /// </summary>
        [JsonPropertyName("exch")]
        public string? Exch { get; init; }

        /// <summary>
        /// Security type (should be "option").
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; init; }

        /// <summary>
        /// Last traded price.
        /// </summary>
        [JsonPropertyName("last")]
        public decimal? Last { get; init; }

        /// <summary>
        /// Change in price from previous close.
        /// </summary>
        [JsonPropertyName("change")]
        public decimal? Change { get; init; }

        /// <summary>
        /// Trading volume.
        /// </summary>
        [JsonPropertyName("volume")]
        public long? Volume { get; init; }

        /// <summary>
        /// Opening price for the day.
        /// </summary>
        [JsonPropertyName("open")]
        public decimal? Open { get; init; }

        /// <summary>
        /// Highest price of the day.
        /// </summary>
        [JsonPropertyName("high")]
        public decimal? High { get; init; }

        /// <summary>
        /// Lowest price of the day.
        /// </summary>
        [JsonPropertyName("low")]
        public decimal? Low { get; init; }

        /// <summary>
        /// Closing price.
        /// </summary>
        [JsonPropertyName("close")]
        public decimal? Close { get; init; }

        /// <summary>
        /// Current bid price.
        /// </summary>
        [JsonPropertyName("bid")]
        public decimal? Bid { get; init; }

        /// <summary>
        /// Current ask price.
        /// </summary>
        [JsonPropertyName("ask")]
        public decimal? Ask { get; init; }

        /// <summary>
        /// Underlying security symbol.
        /// </summary>
        [JsonPropertyName("underlying")]
        public string? Underlying { get; init; }

        /// <summary>
        /// Strike price.
        /// </summary>
        [JsonPropertyName("strike")]
        public decimal? Strike { get; init; }

        /// <summary>
        /// Percentage change from previous close.
        /// </summary>
        [JsonPropertyName("change_percentage")]
        public decimal? ChangePercentage { get; init; }

        /// <summary>
        /// Average daily volume.
        /// </summary>
        [JsonPropertyName("average_volume")]
        public long? AverageVolume { get; init; }

        /// <summary>
        /// Volume of last trade.
        /// </summary>
        [JsonPropertyName("last_volume")]
        public long? LastVolume { get; init; }

        /// <summary>
        /// Trade date/time (epoch).
        /// </summary>
        [JsonPropertyName("trade_date")]
        public long? TradeDate { get; init; }

        /// <summary>
        /// Previous closing price.
        /// </summary>
        [JsonPropertyName("prevclose")]
        public decimal? PrevClose { get; init; }

        /// <summary>
        /// 52-week high price.
        /// </summary>
        [JsonPropertyName("week_52_high")]
        public decimal? Week52High { get; init; }

        /// <summary>
        /// 52-week low price.
        /// </summary>
        [JsonPropertyName("week_52_low")]
        public decimal? Week52Low { get; init; }

        /// <summary>
        /// Size of bid (in hundreds).
        /// </summary>
        [JsonPropertyName("bidsize")]
        public int? BidSize { get; init; }

        /// <summary>
        /// Bid exchange code.
        /// </summary>
        [JsonPropertyName("bidexch")]
        public string? BidExch { get; init; }

        /// <summary>
        /// Bid timestamp (epoch).
        /// </summary>
        [JsonPropertyName("bid_date")]
        public long? BidDate { get; init; }

        /// <summary>
        /// Size of ask (in hundreds).
        /// </summary>
        [JsonPropertyName("asksize")]
        public int? AskSize { get; init; }

        /// <summary>
        /// Ask exchange code.
        /// </summary>
        [JsonPropertyName("askexch")]
        public string? AskExch { get; init; }

        /// <summary>
        /// Ask timestamp (epoch).
        /// </summary>
        [JsonPropertyName("ask_date")]
        public long? AskDate { get; init; }

        /// <summary>
        /// Current open interest.
        /// </summary>
        [JsonPropertyName("open_interest")]
        public long? OpenInterest { get; init; }

        /// <summary>
        /// Contract size.
        /// </summary>
        [JsonPropertyName("contract_size")]
        public int? ContractSize { get; init; }

        /// <summary>
        /// Expiration date.
        /// </summary>
        [JsonPropertyName("expiration_date")]
        public string? ExpirationDate { get; init; }

        /// <summary>
        /// Expiration type (e.g., standard, weekly).
        /// </summary>
        [JsonPropertyName("expiration_type")]
        public string? ExpirationType { get; init; }

        /// <summary>
        /// Option type (put or call).
        /// </summary>
        [JsonPropertyName("option_type")]
        public string? OptionType { get; init; }

        /// <summary>
        /// Root symbol.
        /// </summary>
        [JsonPropertyName("root_symbol")]
        public string? RootSymbol { get; init; }

        /// <summary>
        /// Option Greeks data.
        /// </summary>
        [JsonPropertyName("greeks")]
        public Greek? Greeks { get; init; }
    }
}
