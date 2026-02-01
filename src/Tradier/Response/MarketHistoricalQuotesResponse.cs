using Tradier.Response.DataContracts;
using Tradier.Model;

namespace Tradier.Response
{
    /// <summary>
    /// Response for historical market quotes API calls.
    /// </summary>
    public class MarketHistoricalQuotesResponse : TradierResponseBase<HistoricalQuotesDataContract>
    {
        /// <summary>
        /// Gets the list of historical quotes from the response.
        /// </summary>
        public List<HistoricalQuote> HistoricalQuotes => Data?.History?.DailyQuotes ?? new List<HistoricalQuote>();

        /// <summary>
        /// Gets a value indicating whether any historical quotes are present in the response.
        /// </summary>
        public bool HasHistoricalQuotes => HistoricalQuotes.Any();

        /// <summary>
        /// Gets the count of historical quotes in the response.
        /// </summary>
        public int HistoricalQuoteCount => HistoricalQuotes.Count;

        /// <summary>
        /// Gets the date range of the historical quotes.
        /// </summary>
        public (DateTime? Start, DateTime? End) DateRange
        {
            get
            {
                if (!HasHistoricalQuotes) return (null, null);
                
                var dates = HistoricalQuotes
                    .Where(q => !string.IsNullOrEmpty(q.Date))
                    .Select(q => DateTime.TryParse(q.Date, out var date) ? date : (DateTime?)null)
                    .Where(d => d.HasValue)
                    .Select(d => d!.Value)
                    .ToList();
                    
                if (!dates.Any()) return (null, null);
                
                return (dates.Min(), dates.Max());
            }
        }

        protected override bool HandleNullResponse()
        {
            // Create empty historical quotes data for null responses
            Data = new HistoricalQuotesDataContract 
            { 
                History = new HistoricalQuotesDataContract.HistoricalDataContainer() 
            };
            return true;
        }
    }
}