using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class MarketClockResponse : TradierResponse
    {
        [JsonPropertyName("clock")]
        public Clock? Clock { get; set; }
        internal override void Deserialize()
        {
            this.Clock = System.Text.Json.JsonSerializer.Deserialize<MarketClockResponse>(this.RawResponse)?.Clock ?? new();
        }
    }
}