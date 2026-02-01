using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class HistoricalQuote
    {
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("open")]
        public double? Open { get; set; }

        [JsonPropertyName("high")]
        public double? High { get; set; }

        [JsonPropertyName("low")]
        public double? Low { get; set; }

        [JsonPropertyName("close")]
        public double? Close { get; set; }

        [JsonPropertyName("volume")]
        public int? Volume { get; set; }
    }
}