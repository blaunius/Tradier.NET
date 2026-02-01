using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class MarketLookupSymbolResponse : TradierResponse
    {
        [JsonPropertyName("securities")]
        public LookupContainer? Data { get; set; }
        public class LookupContainer
        {
            [JsonPropertyName("security")]
            public List<Security> Securities { get; set; } = new();
        }
        internal override void Deserialize()
        {
            this.Data = System.Text.Json.JsonSerializer.Deserialize<MarketLookupSymbolResponse>(this.RawResponse)?.Data ?? new();
        }
    }
}