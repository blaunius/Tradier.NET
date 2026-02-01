using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class Trade
    {
        [JsonPropertyName("commission")]
        public double? Commission { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("price")]
        public double? Price { get; set; }

        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("trade_type")]
        public string? TradeType { get; set; }
    }
}
