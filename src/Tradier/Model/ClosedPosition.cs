using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class ClosedPosition
    {
        [JsonPropertyName("close_date")]
        public DateTime? CloseDate { get; set; }

        [JsonPropertyName("cost")]
        public double? Cost { get; set; }

        [JsonPropertyName("gain_loss")]
        public double? GainLoss { get; set; }

        [JsonPropertyName("gain_loss_percent")]
        public double? GainLossPercent { get; set; }

        [JsonPropertyName("open_date")]
        public DateTime? OpenDate { get; set; }

        [JsonPropertyName("proceeds")]
        public double? Proceeds { get; set; }

        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("term")]
        public int? Term { get; set; }
    }
}
