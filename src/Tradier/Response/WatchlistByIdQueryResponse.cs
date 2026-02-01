using System.Text.Json;
using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class WatchlistByIdQueryResponse : TradierResponse
    {
        [JsonPropertyName("watchlists")]
        public WatchlistByIdContainer? Data { get; set; }
        public class WatchlistByIdContainer
        {
            [JsonPropertyName("watchlist")]
            public WatchList Watchlist { get; set; } = new();
        }
        internal override void Deserialize()
        {
            this.Data = JsonSerializer.Deserialize<WatchlistByIdContainer>(this.RawResponse);
        }
    }
}