using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tradier.Model;
using Tradier.Response;

namespace Tradier.Services
{
    /// <summary>
    /// Create and update custom watchlists.
    /// </summary>
    public class WatchlistService : TradingService
    {
        /// <summary>
        /// <inheritdoc cref="WatchlistService"/>
        /// </summary>
        public WatchlistService(ITradierClient client) : base(client) { }
        /// <summary>
        /// <inheritdoc cref="WatchlistService"/>
        /// </summary>
        public WatchlistService() : base() { }

        /// <summary>
        /// Retrieve all of a users watchlists.
        /// </summary>
        public Task<WatchlistQueryResponse> GetWatchlists(CancellationToken token = default)
        {
            return client.Get<WatchlistQueryResponse>("watchlists", token);
        }
        /// <summary>
        /// Retrieve a specific watchlist by id.
        /// </summary>
        public Task<WatchlistByIdQueryResponse> GetWatchlist(string watchlistId, CancellationToken token = default)
        {
            return client.Get<WatchlistByIdQueryResponse>($"watchlists/{watchlistId}", token);
        }
        /// <summary>
        /// Create a new watchlist. The new watchlist created will use the specified name and optional symbols upon creation.
        /// </summary>
        public Task<CreateWatchlistResponse> CreateWatchlist(string name, IEnumerable<string> symbols, CancellationToken token = default)
        {
            return client.Post<CreateWatchlistResponse>("watchlists",
                new Dictionary<string, string>()
                {
                    { "name", name },
                    { "symbols", string.Join(",", symbols) }
                },
                token);
        }
        /// <summary>
        /// Update an existing watchlist. This request will override the existing watchlist information with the parameters sent in the body.
        /// </summary>
        public Task<CreateWatchlistResponse> UpdateWatchlist(string id, string name, IEnumerable<string> symbols, CancellationToken token = default)
        {
            return client.Put<CreateWatchlistResponse>($"watchlists/{id}",
                new Dictionary<string, string>()
                {
                    { "name", name },
                    { "symbols", string.Join(",", symbols) }
                },
                token);
        }
        /// <summary>
        /// Delete a specific watchlist.
        /// </summary>
        public Task<WatchlistByIdDeleteResponse> DeleteWatchlist(string watchlistId, CancellationToken token = default)
        {
            return client.Delete<WatchlistByIdDeleteResponse>($"watchlists/{watchlistId}", token);
        }
        /// <summary>
        /// Add symbols to an existing watchlist. If the symbol exists, it will be over-written.
        /// </summary>
        public Task<WatchlistQueryResponse> AddSymbols(string watchlistId, IEnumerable<string> symbols, CancellationToken token = default)
        {
            return client.Post<WatchlistQueryResponse>($"watchlists/{watchlistId}/symbols",
                new Dictionary<string, string>()
                {
                    { "symbols", string.Join(",", symbols) }
                },
                token);
        }
        /// <summary>
        /// Remove a symbol from a specific watchlist.
        /// </summary>
        public Task<WatchlistByIdQueryResponse> RemoveSymbol(string watchlistId, string symbol, CancellationToken token = default)
        {

            return client.Delete<WatchlistByIdQueryResponse>($"watchlists/{watchlistId}/symbols/{symbol}", token);
        }
    }
}