namespace Tradier.Request
{
    public class MarketOptionExpirationRequest : TradierRequestBase
    {
        public bool IncludeAllRoots { get; set; }
        public bool ShowStrikes { get; set; }
        public bool ShowContractSize { get; set; }
        public bool ShowExpirationType { get; set; }

        protected override void BuildParameters()
        {
            AddParameter("includeAllRoots", IncludeAllRoots);
            AddParameter("strikes", ShowStrikes);
            AddParameter("contractSize", ShowContractSize);
            AddParameter("expirationType", ShowExpirationType);
        }

        [Obsolete("Use IncludeAllRoots property instead")]
        internal string includeAllRoots => IncludeAllRoots ? "true" : "false";
        
        [Obsolete("Use ShowStrikes property instead")]
        internal string showStrikes => ShowStrikes ? "true" : "false";
        
        [Obsolete("Use ShowContractSize property instead")]
        internal string showContractSize => ShowContractSize ? "true" : "false";
        
        [Obsolete("Use ShowExpirationType property instead")]
        internal string showExpirationType => ShowExpirationType ? "true" : "false";
    }
}