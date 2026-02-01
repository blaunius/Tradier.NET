using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class Clock
    {
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("timestamp")]
        public int? Timestamp { get; set; }

        [JsonPropertyName("next_change")]
        public string? NextChange { get; set; }

        [JsonPropertyName("next_state")]
        public string? NextState { get; set; }
    }
}