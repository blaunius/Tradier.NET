using Tradier.Response.DataContracts;
using Tradier.Model;

namespace Tradier.Response
{
    /// <summary>
    /// Response for account orders API calls.
    /// </summary>
    public class AccountOrdersResponse : TradierResponseBase<OrdersDataContract>
    {
        /// <summary>
        /// Gets the list of orders from the response.
        /// </summary>
        public List<Order> Orders => Data?.Orders?.OrdersList ?? new List<Order>();

        /// <summary>
        /// Gets a value indicating whether any orders are present in the response.
        /// </summary>
        public bool HasOrders => Orders.Any();

        /// <summary>
        /// Gets the count of orders in the response.
        /// </summary>
        public int OrderCount => Orders.Count;

        protected override bool IsNullResponse(string content)
        {
            return base.IsNullResponse(content) || content.Trim() == "{\"orders\":\"null\"}";
        }

        protected override bool HandleNullResponse()
        {
            // Create empty orders data for null responses
            Data = new OrdersDataContract 
            { 
                Orders = new OrdersDataContract.OrdersContainer() 
            };
            return true;
        }
    }
}