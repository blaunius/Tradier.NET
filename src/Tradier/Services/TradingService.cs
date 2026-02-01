using Tradier.Enumerations;
using Tradier.Request;
using Tradier.Response;

namespace Tradier.Services
{
    /// <summary>
    /// Service for placing, modifying, and cancelling orders.
    /// </summary>
    public class TradingService : TradierService
    {
        public TradingService(ITradierClient client) : base(client) { }
        public TradingService() : base() { }

        #region Equity Orders

        /// <summary>
        /// Place an equity order (stocks, ETFs).
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="request">The order request.</param>
        /// <param name="preview">If true, preview the order without placing it.</param>
        /// <param name="token">Cancellation token.</param>
        public Task<PlaceOrderResponse> PlaceEquityOrder(
            string accountId,
            EquityOrderRequest request,
            bool preview = false,
            CancellationToken token = default)
        {
            var endpoint = preview
                ? $"accounts/{accountId}/orders?preview=true"
                : $"accounts/{accountId}/orders";

            return client.Post<PlaceOrderResponse>(endpoint, request.ToFormData(), token);
        }

        /// <summary>
        /// Place a market buy order for equities.
        /// </summary>
        public Task<PlaceOrderResponse> BuyStock(
            string accountId,
            string symbol,
            int quantity,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            return PlaceEquityOrder(accountId, new EquityOrderRequest
            {
                Symbol = symbol,
                Side = OrderSide.Buy,
                Quantity = quantity,
                Type = OrderType.Market,
                Duration = duration,
                Tag = tag
            }, false, token);
        }

        /// <summary>
        /// Place a market sell order for equities.
        /// </summary>
        public Task<PlaceOrderResponse> SellStock(
            string accountId,
            string symbol,
            int quantity,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            return PlaceEquityOrder(accountId, new EquityOrderRequest
            {
                Symbol = symbol,
                Side = OrderSide.Sell,
                Quantity = quantity,
                Type = OrderType.Market,
                Duration = duration,
                Tag = tag
            }, false, token);
        }

        /// <summary>
        /// Place a limit buy order for equities.
        /// </summary>
        public Task<PlaceOrderResponse> BuyStockLimit(
            string accountId,
            string symbol,
            int quantity,
            decimal limitPrice,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            return PlaceEquityOrder(accountId, new EquityOrderRequest
            {
                Symbol = symbol,
                Side = OrderSide.Buy,
                Quantity = quantity,
                Type = OrderType.Limit,
                Price = limitPrice,
                Duration = duration,
                Tag = tag
            }, false, token);
        }

        /// <summary>
        /// Place a limit sell order for equities.
        /// </summary>
        public Task<PlaceOrderResponse> SellStockLimit(
            string accountId,
            string symbol,
            int quantity,
            decimal limitPrice,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            return PlaceEquityOrder(accountId, new EquityOrderRequest
            {
                Symbol = symbol,
                Side = OrderSide.Sell,
                Quantity = quantity,
                Type = OrderType.Limit,
                Price = limitPrice,
                Duration = duration,
                Tag = tag
            }, false, token);
        }

        #endregion

        #region Option Orders

        /// <summary>
        /// Place an option order.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="request">The order request.</param>
        /// <param name="preview">If true, preview the order without placing it.</param>
        /// <param name="token">Cancellation token.</param>
        public Task<PlaceOrderResponse> PlaceOptionOrder(
            string accountId,
            OptionOrderRequest request,
            bool preview = false,
            CancellationToken token = default)
        {
            var endpoint = preview
                ? $"accounts/{accountId}/orders?preview=true"
                : $"accounts/{accountId}/orders";

            return client.Post<PlaceOrderResponse>(endpoint, request.ToFormData(), token);
        }

        /// <summary>
        /// Buy to open an option contract.
        /// </summary>
        public Task<PlaceOrderResponse> BuyToOpen(
            string accountId,
            string underlyingSymbol,
            string optionSymbol,
            int quantity,
            OrderType orderType = OrderType.Market,
            decimal? limitPrice = null,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            return PlaceOptionOrder(accountId, new OptionOrderRequest
            {
                Symbol = underlyingSymbol,
                OptionSymbol = optionSymbol,
                Side = OrderSide.BuyToOpen,
                Quantity = quantity,
                Type = orderType,
                Price = limitPrice,
                Duration = duration,
                Tag = tag
            }, false, token);
        }

        /// <summary>
        /// Sell to close an option contract.
        /// </summary>
        public Task<PlaceOrderResponse> SellToClose(
            string accountId,
            string underlyingSymbol,
            string optionSymbol,
            int quantity,
            OrderType orderType = OrderType.Market,
            decimal? limitPrice = null,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            return PlaceOptionOrder(accountId, new OptionOrderRequest
            {
                Symbol = underlyingSymbol,
                OptionSymbol = optionSymbol,
                Side = OrderSide.SellToClose,
                Quantity = quantity,
                Type = orderType,
                Price = limitPrice,
                Duration = duration,
                Tag = tag
            }, false, token);
        }

        /// <summary>
        /// Sell to open an option contract (write).
        /// </summary>
        public Task<PlaceOrderResponse> SellToOpen(
            string accountId,
            string underlyingSymbol,
            string optionSymbol,
            int quantity,
            OrderType orderType = OrderType.Market,
            decimal? limitPrice = null,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            return PlaceOptionOrder(accountId, new OptionOrderRequest
            {
                Symbol = underlyingSymbol,
                OptionSymbol = optionSymbol,
                Side = OrderSide.SellToOpen,
                Quantity = quantity,
                Type = orderType,
                Price = limitPrice,
                Duration = duration,
                Tag = tag
            }, false, token);
        }

        /// <summary>
        /// Buy to close an option contract.
        /// </summary>
        public Task<PlaceOrderResponse> BuyToClose(
            string accountId,
            string underlyingSymbol,
            string optionSymbol,
            int quantity,
            OrderType orderType = OrderType.Market,
            decimal? limitPrice = null,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            return PlaceOptionOrder(accountId, new OptionOrderRequest
            {
                Symbol = underlyingSymbol,
                OptionSymbol = optionSymbol,
                Side = OrderSide.BuyToClose,
                Quantity = quantity,
                Type = orderType,
                Price = limitPrice,
                Duration = duration,
                Tag = tag
            }, false, token);
        }

        #endregion

        #region Multi-Leg Orders

        /// <summary>
        /// Place a multi-leg option order (spreads, straddles, etc.).
        /// </summary>
        public Task<PlaceOrderResponse> PlaceMultiLegOrder(
            string accountId,
            MultiLegOrderRequest request,
            bool preview = false,
            CancellationToken token = default)
        {
            var endpoint = preview
                ? $"accounts/{accountId}/orders?preview=true"
                : $"accounts/{accountId}/orders";

            return client.Post<PlaceOrderResponse>(endpoint, request.ToFormData(), token);
        }

        /// <summary>
        /// Place a vertical spread (bull/bear call/put spread).
        /// </summary>
        /// <param name="accountId">Account ID.</param>
        /// <param name="underlyingSymbol">Underlying symbol (e.g., "AAPL").</param>
        /// <param name="buyLegSymbol">Option symbol to buy.</param>
        /// <param name="sellLegSymbol">Option symbol to sell.</param>
        /// <param name="quantity">Number of contracts.</param>
        /// <param name="netPrice">Net debit (positive) or credit (negative).</param>
        /// <param name="duration">Order duration.</param>
        /// <param name="tag">Optional order tag for tracking.</param>
        /// <param name="token">Cancellation token.</param>
        public Task<PlaceOrderResponse> PlaceVerticalSpread(
            string accountId,
            string underlyingSymbol,
            string buyLegSymbol,
            string sellLegSymbol,
            int quantity,
            decimal netPrice,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            var request = new MultiLegOrderRequest
            {
                Symbol = underlyingSymbol,
                Type = netPrice >= 0 ? OrderType.Debit : OrderType.Credit,
                Duration = duration,
                Price = Math.Abs(netPrice),
                Tag = tag,
                Legs = new List<OrderLeg>
                {
                    new OrderLeg { OptionSymbol = buyLegSymbol, Side = OrderSide.BuyToOpen, Quantity = quantity },
                    new OrderLeg { OptionSymbol = sellLegSymbol, Side = OrderSide.SellToOpen, Quantity = quantity }
                }
            };

            return PlaceMultiLegOrder(accountId, request, false, token);
        }

        /// <summary>
        /// Place an iron condor.
        /// </summary>
        public Task<PlaceOrderResponse> PlaceIronCondor(
            string accountId,
            string underlyingSymbol,
            string buyPutSymbol,      // Lower put (protection)
            string sellPutSymbol,     // Higher put (short)
            string sellCallSymbol,    // Lower call (short)
            string buyCallSymbol,     // Higher call (protection)
            int quantity,
            decimal netCredit,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            var request = new MultiLegOrderRequest
            {
                Symbol = underlyingSymbol,
                Type = OrderType.Credit,
                Duration = duration,
                Price = netCredit,
                Tag = tag,
                Legs = new List<OrderLeg>
                {
                    new OrderLeg { OptionSymbol = buyPutSymbol, Side = OrderSide.BuyToOpen, Quantity = quantity },
                    new OrderLeg { OptionSymbol = sellPutSymbol, Side = OrderSide.SellToOpen, Quantity = quantity },
                    new OrderLeg { OptionSymbol = sellCallSymbol, Side = OrderSide.SellToOpen, Quantity = quantity },
                    new OrderLeg { OptionSymbol = buyCallSymbol, Side = OrderSide.BuyToOpen, Quantity = quantity }
                }
            };

            return PlaceMultiLegOrder(accountId, request, false, token);
        }

        /// <summary>
        /// Place a straddle (buy call and put at same strike).
        /// </summary>
        public Task<PlaceOrderResponse> PlaceStraddle(
            string accountId,
            string underlyingSymbol,
            string callSymbol,
            string putSymbol,
            int quantity,
            decimal netDebit,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            var request = new MultiLegOrderRequest
            {
                Symbol = underlyingSymbol,
                Type = OrderType.Debit,
                Duration = duration,
                Price = netDebit,
                Tag = tag,
                Legs = new List<OrderLeg>
                {
                    new OrderLeg { OptionSymbol = callSymbol, Side = OrderSide.BuyToOpen, Quantity = quantity },
                    new OrderLeg { OptionSymbol = putSymbol, Side = OrderSide.BuyToOpen, Quantity = quantity }
                }
            };

            return PlaceMultiLegOrder(accountId, request, false, token);
        }

        /// <summary>
        /// Place a strangle (buy call and put at different strikes).
        /// </summary>
        public Task<PlaceOrderResponse> PlaceStrangle(
            string accountId,
            string underlyingSymbol,
            string callSymbol,
            string putSymbol,
            int quantity,
            decimal netDebit,
            OrderDuration duration = OrderDuration.Day,
            string? tag = null,
            CancellationToken token = default)
        {
            // Same as straddle in terms of order structure
            return PlaceStraddle(accountId, underlyingSymbol, callSymbol, putSymbol, quantity, netDebit, duration, tag, token);
        }

        #endregion

        #region Order Management

        /// <summary>
        /// Preview an order without placing it.
        /// </summary>
        public Task<PreviewOrderResponse> PreviewOrder(
            string accountId,
            EquityOrderRequest request,
            CancellationToken token = default)
        {
            return client.Post<PreviewOrderResponse>(
                $"accounts/{accountId}/orders?preview=true",
                request.ToFormData(),
                token);
        }

        /// <summary>
        /// Modify an existing order.
        /// </summary>
        public Task<ModifyOrderResponse> ModifyOrder(
            string accountId,
            long orderId,
            ModifyOrderRequest request,
            CancellationToken token = default)
        {
            return client.Put<ModifyOrderResponse>(
                $"accounts/{accountId}/orders/{orderId}",
                request.ToFormData(),
                token);
        }

        /// <summary>
        /// Cancel an existing order.
        /// </summary>
        public Task<CancelOrderResponse> CancelOrder(
            string accountId,
            long orderId,
            CancellationToken token = default)
        {
            return client.Delete<CancelOrderResponse>($"accounts/{accountId}/orders/{orderId}", token);
        }

        #endregion
    }
}
