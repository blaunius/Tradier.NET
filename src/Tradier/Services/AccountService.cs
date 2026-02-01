using System.ComponentModel.DataAnnotations;
using Tradier.Model;
using Tradier.Request;
using Tradier.Response;

namespace Tradier.Services
{
    /// <summary>
    /// Fetch positions, balances and other account related details.
    /// </summary>
    public class AccountService : TradierService
    {
        /// <summary>
        /// <inheritdoc cref="AccountService"/>
        /// </summary>
        public AccountService() : base()
        {
        }
        /// <summary>
        /// <inheritdoc cref="AccountService"/>
        /// </summary>
        public AccountService(ITradierClient tradierClient) : base(tradierClient)
        {
        }

        /// <summary>
        /// The user’s profile contains information pertaining to the user and his/her accounts. In addition to listing all the accounts a user has, this call should be used to create a personalized experience for your customers (i.e. displaying their name when they log in).
        /// </summary>
        public Task<AccountProfileResponse> GetUserProfile(CancellationToken token = default)
        {
            if (this.client is TradierSandboxClient)
                throw new NotSupportedException("User Profile information can't be used in the paper trading/sandbox client.");
            return client.Get<AccountProfileResponse>("user/profile", token);
        }
        /// <summary>
        /// Get balances information for a specific user account. Account balances are calculated on each request during market hours. Each night, balance figures are reconciled with the Tradier clearing firm and used as starting point for the following market session.
        /// </summary>
        public Task<AccountBalancesResponse> GetBalances(string accountId, CancellationToken token = default)
        {
            return client.Get<AccountBalancesResponse>($"accounts/{accountId}/balances", token);
        }
        /// <summary>
        /// Get the current positions being held in an account. These positions are updated intraday via trading.
        /// </summary>
        public Task<AccountPositionsResponse> GetPositions(string accountId, CancellationToken token = default)
        {
            return client.Get<AccountPositionsResponse>($"accounts/{accountId}/positions", token);
        }
        /// <summary>
        /// Get historical activity for an account. This data originates with our clearing firm and inherently has a few limitations:
        /// <para>Updated nightly(not intraday)</para>
        /// <para>Will not include specific time(hours/minutes) a position or order was created or closed</para>
        /// <para>Will not include order numbers</para>
        /// </summary>
        public Task<AccountHistoryResponse> GetHistory(string accountId, AccountHistoryRequest? request = null, CancellationToken token = default)
        {
            if (this.client is TradierSandboxClient)
                throw new NotSupportedException("Historical activity can't be used in the paper trading/sandbox client.");
            request ??= new AccountHistoryRequest();
            return client.Get<AccountHistoryResponse>($"accounts/{accountId}/history?{request.ToQueryString()}", token);
        }
        /// <summary>
        /// Get cost basis information for a specific user account. This includes information for all closed positions. Cost basis information is updated through a nightly batch reconciliation process with our clearing firm.
        /// </summary>
        public Task<AccountGainLossResponse> GetGainLoss(string accountId, GainLossOptions? query = null, CancellationToken token = default)
        {
            query ??= new GainLossOptions();
            return client.Get<AccountGainLossResponse>($"accounts/{accountId}/gainloss?{query.ToQueryString()}", token);
        }
        /// <summary>
        /// Retrieve orders placed within an account. This API will return orders placed for the market session of the present calendar day.
        /// </summary>
        public Task<AccountOrdersResponse> GetOrders(string accountId, bool includeTags = false, int? page = null, CancellationToken token = default)
        {
            return client.Get<AccountOrdersResponse>($"accounts/{accountId}/orders?include_tags={includeTags.ToString().ToLowerInvariant()}{(page.HasValue ? $"&page={page.Value}" : "")}", token);
        }
        /// <summary>
        /// Get detailed information about a previously placed order.
        /// </summary>
        public Task<AccountOrderResponse> GetOrder(string accountId, string id, bool includeTags = false, CancellationToken token = default)
        {
            return client.Get<AccountOrderResponse>($"accounts/{accountId}/orders/{id}?include_tags={includeTags.ToString().ToLowerInvariant()}", token);
        }
    }
}