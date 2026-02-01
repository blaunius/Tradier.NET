using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class MarketSearchCompaniesResponse : TradierResponse
    {
        [JsonPropertyName("securities")]
        public SecuritiesContainer? Data { get; set; }
        public class SecuritiesContainer
        {
            [JsonPropertyName("security")]
            public List<Security> Securities { get; set; } = new();

        }
        internal override void Deserialize()
        {
            this.Data = System.Text.Json.JsonSerializer.Deserialize<MarketSearchCompaniesResponse>(this.RawResponse)?.Data ?? new();
        }
    }
}