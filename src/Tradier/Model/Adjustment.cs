using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class Adjustment
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }
    }
}
