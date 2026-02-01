using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class Cash
    {
        [JsonPropertyName("cash_available")]
        public double? CashAvailable { get; set; }

        [JsonPropertyName("sweep")]
        public int? Sweep { get; set; }

        [JsonPropertyName("unsettled_funds")]
        public double? UnsettledFunds { get; set; }
    }
}
