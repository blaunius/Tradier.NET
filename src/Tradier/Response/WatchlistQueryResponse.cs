using Tradier.Response.DataContracts;
using Tradier.Model;

namespace Tradier.Response
{
    /// <summary>
    /// Response for watchlist query API calls.
    /// </summary>
    public class WatchlistQueryResponse : TradierResponseBase<WatchlistsDataContract>
    {
        /// <summary>
        /// Gets the list of watchlists from the response.
        /// </summary>
        public List<WatchList> Watchlists => Data?.Watchlists?.WatchlistsList ?? new List<WatchList>();

        /// <summary>
        /// Gets a value indicating whether any watchlists are present in the response.
        /// </summary>
        public bool HasWatchlists => Watchlists.Any();

        /// <summary>
        /// Gets the count of watchlists in the response.
        /// </summary>
        public int WatchlistCount => Watchlists.Count;

        protected override bool IsNullResponse(string content)
        {
            return base.IsNullResponse(content) || content.Trim() == "{\"watchlists\":\"null\"}";
        }

        protected override bool HandleNullResponse()
        {
            // Create empty watchlists data for null responses
            Data = new WatchlistsDataContract 
            { 
                Watchlists = new WatchlistsDataContract.WatchlistsContainer() 
            };
            return true;
        }
    }
}
