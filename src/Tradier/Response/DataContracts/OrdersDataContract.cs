using System.Text.Json;
using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response.DataContracts
{
    /// <summary>
    /// Data contract for orders API responses.
    /// </summary>
    public class OrdersDataContract
    {
        /// <summary>
        /// Gets or sets the orders container from the API response.
        /// </summary>
        [JsonPropertyName("orders")]
        public OrdersContainer? Orders { get; set; }

        /// <summary>
        /// Container for order data that handles both single and multiple order responses.
        /// </summary>
        public class OrdersContainer
        {
            /// <summary>
            /// Gets or sets the order or list of orders.
            /// The Tradier API returns either a single order or an array depending on the result count.
            /// </summary>
            [JsonPropertyName("order")]
            public object? OrderData { get; set; }

            /// <summary>
            /// Gets the orders as a list, handling both single order and array responses.
            /// </summary>
            [JsonIgnore]
            public List<Order> OrdersList
            {
                get
                {
                    if (OrderData == null) return new List<Order>();

                    // Handle single order response
                    if (OrderData is JsonElement element)
                    {
                        if (element.ValueKind == JsonValueKind.Object)
                        {
                            var singleOrder = JsonSerializer.Deserialize<Order>(element.GetRawText());
                            return singleOrder != null ? new List<Order> { singleOrder } : new List<Order>();
                        }
                        else if (element.ValueKind == JsonValueKind.Array)
                        {
                            var orders = JsonSerializer.Deserialize<List<Order>>(element.GetRawText());
                            return orders ?? new List<Order>();
                        }
                    }

                    return new List<Order>();
                }
            }
        }
    }
}