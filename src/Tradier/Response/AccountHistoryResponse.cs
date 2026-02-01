using Tradier.Response.DataContracts;
using Tradier.Model;

namespace Tradier.Response
{
    /// <summary>
    /// Response for account history API calls.
    /// </summary>
    public class AccountHistoryResponse : TradierResponseBase<HistoryDataContract>
    {
        /// <summary>
        /// Gets the list of historical events from the response.
        /// </summary>
        public List<Event> Events => Data?.History?.Events ?? new List<Event>();

        /// <summary>
        /// Gets a value indicating whether any events are present in the response.
        /// </summary>
        public bool HasEvents => Events.Any();

        /// <summary>
        /// Gets the count of events in the response.
        /// </summary>
        public int EventCount => Events.Count;

        protected override bool IsNullResponse(string content)
        {
            return base.IsNullResponse(content) || content.Trim() == "{\"history\":\"null\"}";
        }

        protected override bool HandleNullResponse()
        {
            // Create empty history data for null responses
            Data = new HistoryDataContract 
            { 
                History = new HistoryDataContract.HistoryContainer() 
            };
            return true;
        }
    }
}
