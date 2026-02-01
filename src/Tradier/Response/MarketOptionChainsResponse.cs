using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class MarketOptionChainsResponse : TradierResponse
    {
        [JsonPropertyName("options")]
        public OptionsContainer? Data { get; set; }
        public class OptionsContainer
        {
            [JsonPropertyName("option")]
            public List<OptionChain> Options { get; set; } = new();
        }
        internal override void Deserialize()
        {
            this.Data = System.Text.Json.JsonSerializer.Deserialize<MarketOptionChainsResponse>(this.RawResponse)?.Data ?? new();
        }
    }
}
