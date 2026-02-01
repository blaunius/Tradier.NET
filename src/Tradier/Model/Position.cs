using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class Position
    {
        [JsonPropertyName("cost_basis")]
        public double? CostBasis { get; set; }

        [JsonPropertyName("date_acquired")]
        public DateTime? DateAcquired { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
    }
}
