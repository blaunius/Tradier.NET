namespace Tradier.Request
{
    public class MarketHistoricalQuotesRequest : TradierRequestBase
    {
        public Enumerations.IntervalType Interval { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        /// <summary>
        /// Specify to retrieve aggregate data for all hours of the day (All) or only regular trading sessions (Open)
        /// </summary>
        public Enumerations.SessionFilter SessionFilter { get; set; }

        protected override void BuildParameters()
        {
            AddParameter("interval", (Enumerations.IntervalType?)Interval);
            AddParameter("session_filter", (Enumerations.SessionFilter?)SessionFilter);
            AddParameter("start", Start);
            AddParameter("end", End);
        }

        [Obsolete("Use ToQueryString() instead")]
        public string ParseQuery() => ToQueryString();
    }
}