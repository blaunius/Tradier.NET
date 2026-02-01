using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class MarketCalendarResponse : TradierResponse
    {
        [JsonPropertyName("calendar")]
        public Calendar? Calendar { get; set; }
        internal override void Deserialize()
        {
            this.Calendar = System.Text.Json.JsonSerializer.Deserialize<MarketCalendarResponse>(this.RawResponse)?.Calendar ?? new Calendar();
        }
    }
}