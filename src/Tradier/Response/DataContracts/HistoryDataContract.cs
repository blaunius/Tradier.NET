using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response.DataContracts
{
    /// <summary>
    /// Data contract for account history API responses.
    /// </summary>
    public class HistoryDataContract
    {
        /// <summary>
        /// Gets or sets the history container from the API response.
        /// </summary>
        [JsonPropertyName("history")]
        public HistoryContainer? History { get; set; }

        /// <summary>
        /// Container for history event data.
        /// </summary>
        public class HistoryContainer
        {
            /// <summary>
            /// Gets or sets the events from the API response.
            /// </summary>
            [JsonPropertyName("event")]
            public List<Event> Events { get; set; } = new();
        }
    }

    /// <summary>
    /// Data contract for historical quotes API responses.
    /// </summary>
    public class HistoricalQuotesDataContract
    {
        /// <summary>
        /// Gets or sets the historical data container from the API response.
        /// </summary>
        [JsonPropertyName("history")]
        public HistoricalDataContainer? History { get; set; }

        /// <summary>
        /// Container for historical quote data.
        /// </summary>
        public class HistoricalDataContainer
        {
            /// <summary>
            /// Gets or sets the daily historical quotes.
            /// </summary>
            [JsonPropertyName("day")]
            public List<HistoricalQuote> DailyQuotes { get; set; } = new();
        }
    }
}