using Tradier.Enumerations;

namespace Tradier.Request
{
    public class MarketTimeAndSalesRequest : TradierRequestBase
    {
        public IntervalScope Interval { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public SessionFilter Session { get; set; }

        private string FormatInterval()
        {
            return Interval switch
            {
                IntervalScope.OneMinute => "1min",
                IntervalScope.FiveMinutes => "5min",
                IntervalScope.FifteenMinutes => "15min",
                _ => Interval.ToString().ToLowerInvariant()
            };
        }

        protected override void BuildParameters()
        {
            AddParameter("interval", FormatInterval());
            AddParameter("session_filter", (SessionFilter?)Session);
            AddParameter("start", Start);
            AddParameter("end", End);
        }

        [Obsolete("Use ToQueryString() instead")]
        public string ParseQuery() => ToQueryString();
    }
}