using Tradier.Response.DataContracts;
using Tradier.Model;

namespace Tradier.Response
{
    /// <summary>
    /// Response for account balances API calls.
    /// </summary>
    public class AccountBalancesResponse : TradierResponseBase<BalanceDataContract>
    {
        /// <summary>
        /// Gets the balance information from the response.
        /// </summary>
        public Balance? Balance => Data?.Balances;

        /// <summary>
        /// Gets a value indicating whether balance data is available.
        /// </summary>
        public bool HasBalance => Balance != null;

        protected override bool HandleNullResponse()
        {
            // Create empty balance data for null responses
            Data = new BalanceDataContract { Balances = new Balance() };
            return true;
        }
    }
}
