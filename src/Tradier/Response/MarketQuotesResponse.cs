using Tradier.Response.DataContracts;
using Tradier.Model;

namespace Tradier.Response
{
    /// <summary>
    /// Response for market quotes API calls.
    /// </summary>
    public class MarketQuotesResponse : TradierResponseBase<QuotesDataContract>
    {
        /// <summary>
        /// Gets the list of quotes from the response.
        /// </summary>
        public List<Quote> Quotes => Data?.Quotes?.QuotesList ?? new List<Quote>();

        /// <summary>
        /// Gets a value indicating whether any quotes are present in the response.
        /// </summary>
        public bool HasQuotes => Quotes.Any();

        /// <summary>
        /// Gets the count of quotes in the response.
        /// </summary>
        public int QuoteCount => Quotes.Count;

        /// <summary>
        /// Gets the first quote from the response, if available.
        /// </summary>
        public Quote? FirstQuote => Quotes.FirstOrDefault();

        protected override bool HandleNullResponse()
        {
            // Create empty quotes data for null responses
            Data = new QuotesDataContract 
            { 
                Quotes = new QuotesDataContract.QuotesContainer() 
            };
            return true;
        }
    }
}
