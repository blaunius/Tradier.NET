using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class MarketETBSecuritiesResponse : TradierResponse
    {
        [JsonPropertyName("securities")]
        public MarketETBContainer? Data { get; set; }
        public class MarketETBContainer
        {
            [JsonPropertyName("security")]
            public List<ETBSecurity> ETBSecurities { get; set; } = new List<ETBSecurity>();

        }
        internal override void Deserialize()
        {
            this.Data = System.Text.Json.JsonSerializer.Deserialize<MarketETBSecuritiesResponse>(this.RawResponse)?.Data ?? new();
        }
    }
}