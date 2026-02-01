using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class Pdt
    {
        [JsonPropertyName("fed_call")]
        public int? FedCall { get; set; }

        [JsonPropertyName("maintenance_call")]
        public int? MaintenanceCall { get; set; }

        [JsonPropertyName("option_buying_power")]
        public double? OptionBuyingPower { get; set; }

        [JsonPropertyName("stock_buying_power")]
        public double? StockBuyingPower { get; set; }

        [JsonPropertyName("stock_short_value")]
        public int? StockShortValue { get; set; }
    }
}
