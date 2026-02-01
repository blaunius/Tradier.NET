using System.Text.Json.Serialization;

namespace Tradier.Response
{
    public class MarketOptionStikesResponse : TradierResponse
    {
        [JsonPropertyName("strikes")]
        public StrikeData? Data { get; set; }
        public class StrikeData
        {
            [JsonPropertyName("strike")]
            public List<decimal?> Strikes { get; set; } = new();
        }
        internal override void Deserialize()
        {
            Data = System.Text.Json.JsonSerializer.Deserialize<MarketOptionStikesResponse>(this.RawResponse)?.Data ?? new();
        }
    }
}