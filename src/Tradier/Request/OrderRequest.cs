using System.Text;
using Tradier.Enumerations;

namespace Tradier.Request
{
    /// <summary>
    /// Base request for placing orders.
    /// </summary>
    public abstract class OrderRequestBase : TradierRequestBase
    {
        /// <summary>
        /// Order class (equity, option, multileg, combo).
        /// </summary>
        public abstract OrderClass Class { get; }

        /// <summary>
        /// Trading symbol.
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Order side (buy, sell, buy_to_open, etc.).
        /// </summary>
        public OrderSide Side { get; set; }

        /// <summary>
        /// Number of shares or contracts.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Order type (market, limit, stop, stop_limit).
        /// </summary>
        public OrderType Type { get; set; }

        /// <summary>
        /// Duration (day, gtc, pre, post).
        /// </summary>
        public OrderDuration Duration { get; set; }

        /// <summary>
        /// Limit price (required for limit and stop_limit orders).
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Stop price (required for stop and stop_limit orders).
        /// </summary>
        public decimal? Stop { get; set; }

        /// <summary>
        /// Optional order tag for tracking.
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
        /// Converts the request to form data for POST.
        /// </summary>
        public virtual Dictionary<string, string> ToFormData()
        {
            var data = new Dictionary<string, string>
            {
                ["class"] = GetClassString(),
                ["symbol"] = Symbol,
                ["side"] = GetSideString(),
                ["quantity"] = Quantity.ToString(),
                ["type"] = GetTypeString(),
                ["duration"] = GetDurationString()
            };

            if (Price.HasValue)
                data["price"] = Price.Value.ToString("F2");
            if (Stop.HasValue)
                data["stop"] = Stop.Value.ToString("F2");
            if (!string.IsNullOrEmpty(Tag))
                data["tag"] = Tag;

            return data;
        }

        protected string GetClassString() => Class switch
        {
            OrderClass.Equity => "equity",
            OrderClass.Option => "option",
            OrderClass.MultiLeg => "multileg",
            OrderClass.Combo => "combo",
            _ => throw new ArgumentException("Invalid order class")
        };

        protected string GetSideString() => Side switch
        {
            OrderSide.Buy => "buy",
            OrderSide.Sell => "sell",
            OrderSide.BuyToOpen => "buy_to_open",
            OrderSide.BuyToClose => "buy_to_close",
            OrderSide.SellToOpen => "sell_to_open",
            OrderSide.SellToClose => "sell_to_close",
            _ => throw new ArgumentException("Invalid order side")
        };

        protected string GetTypeString() => Type switch
        {
            OrderType.Market => "market",
            OrderType.Limit => "limit",
            OrderType.Stop => "stop",
            OrderType.StopLimit => "stop_limit",
            OrderType.Debit => "debit",
            OrderType.Credit => "credit",
            OrderType.Even => "even",
            _ => throw new ArgumentException("Invalid order type")
        };

        protected string GetDurationString() => Duration switch
        {
            OrderDuration.Day => "day",
            OrderDuration.GoodTillCanceled => "gtc",
            OrderDuration.PreMarket => "pre",
            OrderDuration.PostMarket => "post",
            _ => "day"
        };
    }

    /// <summary>
    /// Request for placing equity orders.
    /// </summary>
    public class EquityOrderRequest : OrderRequestBase
    {
        public override OrderClass Class => OrderClass.Equity;
    }

    /// <summary>
    /// Request for placing option orders.
    /// </summary>
    public class OptionOrderRequest : OrderRequestBase
    {
        public override OrderClass Class => OrderClass.Option;

        /// <summary>
        /// The option symbol (OCC format: AAPL220617C00270000).
        /// </summary>
        public string OptionSymbol { get; set; } = string.Empty;

        public override Dictionary<string, string> ToFormData()
        {
            var data = base.ToFormData();
            data["option_symbol"] = OptionSymbol;
            return data;
        }
    }

    /// <summary>
    /// Represents a leg in a multi-leg order.
    /// </summary>
    public class OrderLeg
    {
        /// <summary>
        /// Option symbol for this leg.
        /// </summary>
        public string OptionSymbol { get; set; } = string.Empty;

        /// <summary>
        /// Side for this leg.
        /// </summary>
        public OrderSide Side { get; set; }

        /// <summary>
        /// Quantity for this leg.
        /// </summary>
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Request for placing multi-leg option orders (spreads, straddles, etc.).
    /// </summary>
    public class MultiLegOrderRequest : TradierRequestBase
    {
        public OrderClass Class => OrderClass.MultiLeg;

        /// <summary>
        /// Underlying symbol.
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Order type (market, debit, credit, even).
        /// </summary>
        public OrderType Type { get; set; }

        /// <summary>
        /// Duration (day, gtc).
        /// </summary>
        public OrderDuration Duration { get; set; }

        /// <summary>
        /// Price (net debit or credit).
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Order legs (2-4 legs).
        /// </summary>
        public List<OrderLeg> Legs { get; set; } = new();

        /// <summary>
        /// Optional order tag.
        /// </summary>
        public string? Tag { get; set; }

        public Dictionary<string, string> ToFormData()
        {
            var data = new Dictionary<string, string>
            {
                ["class"] = "multileg",
                ["symbol"] = Symbol,
                ["type"] = Type switch
                {
                    OrderType.Market => "market",
                    OrderType.Debit => "debit",
                    OrderType.Credit => "credit",
                    OrderType.Even => "even",
                    _ => "market"
                },
                ["duration"] = Duration switch
                {
                    OrderDuration.Day => "day",
                    OrderDuration.GoodTillCanceled => "gtc",
                    _ => "day"
                }
            };

            if (Price.HasValue)
                data["price"] = Price.Value.ToString("F2");
            if (!string.IsNullOrEmpty(Tag))
                data["tag"] = Tag;

            // Add legs
            for (int i = 0; i < Legs.Count; i++)
            {
                var leg = Legs[i];
                data[$"option_symbol[{i}]"] = leg.OptionSymbol;
                data[$"side[{i}]"] = leg.Side switch
                {
                    OrderSide.BuyToOpen => "buy_to_open",
                    OrderSide.BuyToClose => "buy_to_close",
                    OrderSide.SellToOpen => "sell_to_open",
                    OrderSide.SellToClose => "sell_to_close",
                    _ => "buy_to_open"
                };
                data[$"quantity[{i}]"] = leg.Quantity.ToString();
            }

            return data;
        }
    }

    /// <summary>
    /// Request for modifying an existing order.
    /// </summary>
    public class ModifyOrderRequest : TradierRequestBase
    {
        /// <summary>
        /// New order type (optional).
        /// </summary>
        public OrderType? Type { get; set; }

        /// <summary>
        /// New duration (optional).
        /// </summary>
        public OrderDuration? Duration { get; set; }

        /// <summary>
        /// New price (optional).
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// New stop price (optional).
        /// </summary>
        public decimal? Stop { get; set; }

        public Dictionary<string, string> ToFormData()
        {
            var data = new Dictionary<string, string>();

            if (Type.HasValue)
                data["type"] = Type.Value switch
                {
                    OrderType.Market => "market",
                    OrderType.Limit => "limit",
                    OrderType.Stop => "stop",
                    OrderType.StopLimit => "stop_limit",
                    _ => "limit"
                };

            if (Duration.HasValue)
                data["duration"] = Duration.Value switch
                {
                    OrderDuration.Day => "day",
                    OrderDuration.GoodTillCanceled => "gtc",
                    _ => "day"
                };

            if (Price.HasValue)
                data["price"] = Price.Value.ToString("F2");

            if (Stop.HasValue)
                data["stop"] = Stop.Value.ToString("F2");

            return data;
        }
    }
}
