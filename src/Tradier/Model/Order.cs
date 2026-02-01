using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tradier.Converters;
using Tradier.Enumerations;

namespace Tradier.Model
{
    /// <summary>
    /// Represents a trading order with its status, execution details, and metadata.
    /// </summary>
    public class Order : TradierModelBase
    {
        /// <summary>
        /// Gets or sets the unique identifier for this order.
        /// </summary>
        [JsonPropertyName("id")]
        [Required(ErrorMessage = "Order ID is required")]
        [Range(1, long.MaxValue, ErrorMessage = "Order ID must be a positive number")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the type of order (Market, Limit, Stop, etc.).
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(EnumStringConverter<OrderType>))]
        public OrderType Type { get; set; } = OrderType.Unknown;

        /// <summary>
        /// Gets or sets the trading symbol for this order.
        /// </summary>
        [JsonPropertyName("symbol")]
        [Required(ErrorMessage = "Order symbol is required")]
        [RegularExpression(@"^[A-Z0-9._]{1,32}$", ErrorMessage = "Symbol must contain only uppercase letters, numbers, dots, and underscores")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the side of the order (Buy, Sell, Buy to Open, etc.).
        /// </summary>
        [JsonPropertyName("side")]
        [JsonConverter(typeof(EnumStringConverter<OrderSide>))]
        public OrderSide Side { get; set; } = OrderSide.Unknown;

        /// <summary>
        /// Gets or sets the quantity of shares or contracts to trade.
        /// </summary>
        [JsonPropertyName("quantity")]
        [Required(ErrorMessage = "Order quantity is required")]
        [Range(typeof(decimal), "0.01", "999999999.99", ErrorMessage = "Quantity must be greater than 0")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Gets or sets the current status of the order.
        /// </summary>
        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumStringConverter<OrderStatus>))]
        public OrderStatus Status { get; set; } = OrderStatus.Unknown;

        /// <summary>
        /// Gets or sets how long the order should remain active.
        /// </summary>
        [JsonPropertyName("duration")]
        [JsonConverter(typeof(EnumStringConverter<OrderDuration>))]
        public OrderDuration Duration { get; set; } = OrderDuration.Unknown;

        /// <summary>
        /// Gets or sets the limit price for limit orders (null for market orders).
        /// </summary>
        [JsonPropertyName("price")]
        [Range(typeof(decimal), "0.01", "999999.99", ErrorMessage = "Price must be greater than 0")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the average fill price for executed portions of the order.
        /// </summary>
        [JsonPropertyName("avg_fill_price")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "Average fill price must be non-negative")]
        public decimal? AverageFillPrice { get; set; }

        /// <summary>
        /// Gets or sets the total quantity that has been executed.
        /// </summary>
        [JsonPropertyName("exec_quantity")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Executed quantity must be non-negative")]
        public decimal ExecutedQuantity { get; set; }

        /// <summary>
        /// Gets or sets the price of the most recent fill.
        /// </summary>
        [JsonPropertyName("last_fill_price")]
        [Range(typeof(decimal), "0", "999999.99", ErrorMessage = "Last fill price must be non-negative")]
        public decimal? LastFillPrice { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the most recent fill.
        /// </summary>
        [JsonPropertyName("last_fill_quantity")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Last fill quantity must be non-negative")]
        public decimal? LastFillQuantity { get; set; }

        /// <summary>
        /// Gets or sets the quantity remaining to be executed.
        /// </summary>
        [JsonPropertyName("remaining_quantity")]
        [Range(typeof(decimal), "0", "999999999.99", ErrorMessage = "Remaining quantity must be non-negative")]
        public decimal RemainingQuantity { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the order was created.
        /// </summary>
        [JsonPropertyName("create_date")]
        [Required(ErrorMessage = "Create date is required")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the most recent transaction for this order.
        /// </summary>
        [JsonPropertyName("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the class of the order (Equity, Option, MultiLeg, etc.).
        /// </summary>
        [JsonPropertyName("class")]
        [JsonConverter(typeof(EnumStringConverter<OrderClass>))]
        public OrderClass Class { get; set; } = OrderClass.Unknown;

        // Calculated properties

        /// <summary>
        /// Gets a value indicating whether this order is still active (open or partially filled).
        /// </summary>
        [JsonIgnore]
        public bool IsActive => Status == OrderStatus.Open || Status == OrderStatus.PartiallyFilled || Status == OrderStatus.Pending;

        /// <summary>
        /// Gets a value indicating whether this order has been completely filled.
        /// </summary>
        [JsonIgnore]
        public bool IsCompleted => Status == OrderStatus.Filled;

        /// <summary>
        /// Gets a value indicating whether this order was cancelled or rejected.
        /// </summary>
        [JsonIgnore]
        public bool IsTerminated => Status == OrderStatus.Canceled || Status == OrderStatus.Rejected || Status == OrderStatus.Expired;

        /// <summary>
        /// Gets the fill percentage (0-100).
        /// </summary>
        [JsonIgnore]
        public decimal FillPercentage => Quantity > 0 ? (ExecutedQuantity / Quantity) * 100 : 0;

        /// <summary>
        /// Gets a value indicating whether this is a buy order.
        /// </summary>
        [JsonIgnore]
        public bool IsBuyOrder => Side == OrderSide.Buy || Side == OrderSide.BuyToOpen || Side == OrderSide.BuyToClose;

        /// <summary>
        /// Gets a value indicating whether this is a sell order.
        /// </summary>
        [JsonIgnore]
        public bool IsSellOrder => Side == OrderSide.Sell || Side == OrderSide.SellToOpen || Side == OrderSide.SellToClose;

        /// <summary>
        /// Gets a value indicating whether this is a market order.
        /// </summary>
        [JsonIgnore]
        public bool IsMarketOrder => Type == OrderType.Market;

        /// <summary>
        /// Gets a value indicating whether this is a limit order.
        /// </summary>
        [JsonIgnore]
        public bool IsLimitOrder => Type == OrderType.Limit || Type == OrderType.StopLimit;

        /// <summary>
        /// Gets the total value of executed portions (quantity * average fill price).
        /// </summary>
        [JsonIgnore]
        public decimal? ExecutedValue => ExecutedQuantity > 0 && AverageFillPrice.HasValue ? ExecutedQuantity * AverageFillPrice.Value : null;
    }
}
