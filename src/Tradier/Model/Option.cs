using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class Option : Security
    {

        [JsonPropertyName("option_type")]
        public string? OptionType { get; set; }

        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }
    }
}
