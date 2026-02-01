using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class MarketOptionSymbolsResponse : TradierResponse
    {
        [JsonPropertyName("symbols")]
        public List<SymbolsContainer>? Data { get; set; }
        public class SymbolsContainer
        {
            [JsonPropertyName("rootSymbol")]
            public string? RootSymbol { get; set; }
            [JsonPropertyName("options")]
            public List<string> Options { get; set; } = [];
        }
        internal override void Deserialize()
        {
            this.Data = System.Text.Json.JsonSerializer.Deserialize<MarketOptionSymbolsResponse>(this.RawResponse)?.Data ?? new();
        }
    }
}