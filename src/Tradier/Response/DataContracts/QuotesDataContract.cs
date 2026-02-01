using System.Text.Json;
using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response.DataContracts
{
    /// <summary>
    /// Data contract for market quotes API responses.
    /// </summary>
    public class QuotesDataContract
    {
        /// <summary>
        /// Gets or sets the quotes container from the API response.
        /// </summary>
        [JsonPropertyName("quotes")]
        public QuotesContainer? Quotes { get; set; }

        /// <summary>
        /// Container for quote data that handles both single and multiple quote responses.
        /// </summary>
        public class QuotesContainer
        {
            /// <summary>
            /// Gets or sets the quote or list of quotes.
            /// The Tradier API returns either a single quote or an array depending on the request.
            /// </summary>
            [JsonPropertyName("quote")]
            public object? QuoteData { get; set; }

            /// <summary>
            /// Gets the quotes as a list, handling both single quote and array responses.
            /// </summary>
            [JsonIgnore]
            public List<Quote> QuotesList
            {
                get
                {
                    if (QuoteData == null) return new List<Quote>();

                    // Handle single quote response
                    if (QuoteData is JsonElement element)
                    {
                        if (element.ValueKind == JsonValueKind.Object)
                        {
                            var singleQuote = JsonSerializer.Deserialize<Quote>(element.GetRawText());
                            return singleQuote != null ? new List<Quote> { singleQuote } : new List<Quote>();
                        }
                        else if (element.ValueKind == JsonValueKind.Array)
                        {
                            var quotes = JsonSerializer.Deserialize<List<Quote>>(element.GetRawText());
                            return quotes ?? new List<Quote>();
                        }
                    }

                    return new List<Quote>();
                }
            }
        }
    }
}