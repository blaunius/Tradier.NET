using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tradier.Model;

namespace Tradier.Response
{
    public class CreateWatchlistResponse : TradierResponse
    {
        [JsonPropertyName("watchlist")]
        public WatchList? Data { get; set; }
        internal override void Deserialize()
        {
            this.Data = JsonSerializer.Deserialize<CreateWatchlistResponse>(this.RawResponse)?.Data ?? new();
        }
    }
    public class WatchlistByIdDeleteResponse : WatchlistQueryResponse
    {
    }
}
