using System.Text.Json;
using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    /// <summary>
    /// Response from placing an order.
    /// </summary>
#pragma warning disable CS0618 // Type or member is obsolete
    public class PlaceOrderResponse : TradierResponse
#pragma warning restore CS0618
    {
        [JsonPropertyName("order")]
        public PlacedOrder? Order { get; set; }

        internal override void Deserialize()
        {
            Order = JsonSerializer.Deserialize<PlaceOrderResponse>(RawResponse)?.Order;
        }
    }

    /// <summary>
    /// Represents a successfully placed order.
    /// </summary>
    public class PlacedOrder : TradierModelBase
    {
        /// <summary>
        /// The order ID assigned by Tradier.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Status of the order (ok, error).
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Partner ID (if applicable).
        /// </summary>
        [JsonPropertyName("partner_id")]
        public string? PartnerId { get; set; }
    }

    /// <summary>
    /// Response from previewing an order.
    /// </summary>
#pragma warning disable CS0618
    public class PreviewOrderResponse : TradierResponse
#pragma warning restore CS0618
    {
        [JsonPropertyName("order")]
        public OrderPreview? Order { get; set; }

        internal override void Deserialize()
        {
            Order = JsonSerializer.Deserialize<PreviewOrderResponse>(RawResponse)?.Order;
        }
    }

    /// <summary>
    /// Preview of an order before placement.
    /// </summary>
    public class OrderPreview : TradierModelBase
    {
        /// <summary>
        /// Estimated commission.
        /// </summary>
        [JsonPropertyName("commission")]
        public decimal Commission { get; set; }

        /// <summary>
        /// Estimated cost/proceeds.
        /// </summary>
        [JsonPropertyName("cost")]
        public decimal Cost { get; set; }

        /// <summary>
        /// Extended hours fee (if applicable).
        /// </summary>
        [JsonPropertyName("extended_hours")]
        public decimal? ExtendedHours { get; set; }

        /// <summary>
        /// Order fees.
        /// </summary>
        [JsonPropertyName("fees")]
        public decimal Fees { get; set; }

        /// <summary>
        /// Margin requirement.
        /// </summary>
        [JsonPropertyName("margin_change")]
        public decimal? MarginChange { get; set; }

        /// <summary>
        /// Order status.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Order result type.
        /// </summary>
        [JsonPropertyName("result_type")]
        public string? ResultType { get; set; }

        /// <summary>
        /// Warnings or messages.
        /// </summary>
        [JsonPropertyName("warnings")]
        public List<string>? Warnings { get; set; }
    }

    /// <summary>
    /// Response from modifying an order.
    /// </summary>
#pragma warning disable CS0618
    public class ModifyOrderResponse : TradierResponse
#pragma warning restore CS0618
    {
        [JsonPropertyName("order")]
        public ModifiedOrder? Order { get; set; }

        internal override void Deserialize()
        {
            Order = JsonSerializer.Deserialize<ModifyOrderResponse>(RawResponse)?.Order;
        }
    }

    /// <summary>
    /// Result of modifying an order.
    /// </summary>
    public class ModifiedOrder : TradierModelBase
    {
        /// <summary>
        /// The order ID.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Status of the modification.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response from cancelling an order.
    /// </summary>
#pragma warning disable CS0618
    public class CancelOrderResponse : TradierResponse
#pragma warning restore CS0618
    {
        [JsonPropertyName("order")]
        public CancelledOrder? Order { get; set; }

        internal override void Deserialize()
        {
            Order = JsonSerializer.Deserialize<CancelOrderResponse>(RawResponse)?.Order;
        }
    }

    /// <summary>
    /// Result of cancelling an order.
    /// </summary>
    public class CancelledOrder : TradierModelBase
    {
        /// <summary>
        /// The order ID.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Status of the cancellation.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
