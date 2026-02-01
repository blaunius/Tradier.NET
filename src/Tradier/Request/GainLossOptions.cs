namespace Tradier.Request
{
    public class GainLossOptions : PaginationRequest
    {
        public Enumerations.SortResult? SortBy { get; set; }
        public Enumerations.SortDirection? Sort { get; set; }
        public string? Symbol { get; set; }

        protected override void BuildParameters()
        {
            base.BuildParameters();
            AddParameter("sort_by", SortBy);
            AddParameter("sort", Sort);
            AddParameter("symbol", Symbol);
        }

        [Obsolete("Use ToQueryString() instead")]
        public override string ParseQueryString() => ToQueryString();
    }
}
