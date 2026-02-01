namespace Tradier.Request
{
    public class PaginationRequest : TradierRequestBase
    {
        /// <summary>
        /// Used for paginated results. Page to start results.
        /// </summary>
        public string? Page { get; set; }
        /// <summary>
        /// Number of results to return per page.
        /// </summary>
        public string? Limit { get; set; }
        public virtual string? Type { get; set; }
        /// <summary>
        /// Start date
        /// </summary>
        public DateTime? Start { get; set; }
        /// <summary>
        /// End date
        /// </summary>
        public DateTime? End { get; set; }

        protected override void BuildParameters()
        {
            AddParameter("page", Page);
            AddParameter("limit", Limit);
            AddParameter("start", Start);
            AddParameter("end", End);
            AddParameter("type", Type);
        }

        [Obsolete("Use ToQueryString() instead")]
        public virtual string ParseQueryString() => ToQueryString();
    }
}
