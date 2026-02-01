using System.Text.Json;
using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response.DataContracts
{
    /// <summary>
    /// Data contract for watchlist query API responses.
    /// </summary>
    public class WatchlistsDataContract
    {
        /// <summary>
        /// Gets or sets the watchlists container from the API response.
        /// </summary>
        [JsonPropertyName("watchlists")]
        public WatchlistsContainer? Watchlists { get; set; }

        /// <summary>
        /// Container for watchlist data.
        /// </summary>
        public class WatchlistsContainer
        {
            /// <summary>
            /// Gets or sets the watchlist or list of watchlists.
            /// The Tradier API returns either a single watchlist or an array.
            /// </summary>
            [JsonPropertyName("watchlist")]
            public object? WatchlistData { get; set; }

            /// <summary>
            /// Gets the watchlists as a list, handling both single watchlist and array responses.
            /// </summary>
            [JsonIgnore]
            public List<WatchList> WatchlistsList
            {
                get
                {
                    if (WatchlistData == null) return new List<WatchList>();

                    // Handle single watchlist response
                    if (WatchlistData is JsonElement element)
                    {
                        if (element.ValueKind == JsonValueKind.Object)
                        {
                            var singleWatchlist = JsonSerializer.Deserialize<WatchList>(element.GetRawText());
                            return singleWatchlist != null ? new List<WatchList> { singleWatchlist } : new List<WatchList>();
                        }
                        else if (element.ValueKind == JsonValueKind.Array)
                        {
                            var watchlists = JsonSerializer.Deserialize<List<WatchList>>(element.GetRawText());
                            return watchlists ?? new List<WatchList>();
                        }
                    }

                    return new List<WatchList>();
                }
            }
        }
    }

    /// <summary>
    /// Data contract for single watchlist API responses.
    /// </summary>
    public class WatchlistDataContract
    {
        /// <summary>
        /// Gets or sets the watchlist container from the API response.
        /// </summary>
        [JsonPropertyName("watchlist")]
        public WatchList? Watchlist { get; set; }
    }

    /// <summary>
    /// Data contract for watchlist creation API responses.
    /// </summary>
    public class CreateWatchlistDataContract
    {
        /// <summary>
        /// Gets or sets the watchlist container from the API response.
        /// </summary>
        [JsonPropertyName("watchlist")]
        public CreatedWatchlistInfo? Watchlist { get; set; }

        /// <summary>
        /// Information about a newly created watchlist.
        /// </summary>
        public class CreatedWatchlistInfo
        {
            /// <summary>
            /// Gets or sets the ID of the created watchlist.
            /// </summary>
            [JsonPropertyName("id")]
            public string? Id { get; set; }

            /// <summary>
            /// Gets or sets the name of the created watchlist.
            /// </summary>
            [JsonPropertyName("name")]
            public string? Name { get; set; }
        }
    }
}