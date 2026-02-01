using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class MarketTimeAndSalesResponse : TradierResponse
    {
        [JsonPropertyName("series")]
        public TimeAndSalesContainer? Data { get; set; }
        public class TimeAndSalesContainer
        {
            [JsonPropertyName("data")]
            public List<TimeAndSales> TimeAndSales { get; set; } = new();
        }
        internal override void Deserialize()
        {
            this.Data = System.Text.Json.JsonSerializer.Deserialize<MarketTimeAndSalesResponse>(this.RawResponse)?.Data ?? new();
        }
    }
}