using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class Event
    {
        [JsonPropertyName("amount")]
        public double? Amount { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("trade")]
        public Trade? Trade { get; set; }

        [JsonPropertyName("adjustment")]
        public Adjustment? Adjustment { get; set; }

        [JsonPropertyName("option")]
        public Option? Option { get; set; }

        [JsonPropertyName("journal")]
        public Journal? Journal { get; set; }
    }
}
