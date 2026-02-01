namespace Tradier.Services
{
    /// <summary>
    /// Base class for all Tradier API services.
    /// </summary>
    public abstract class TradierService
    {
        /// <summary>
        /// The Tradier API client used for making requests.
        /// </summary>
        protected readonly ITradierClient client;

        /// <summary>
        /// Creates a new service with the specified client.
        /// </summary>
        /// <param name="client">The Tradier API client to use.</param>
        public TradierService(ITradierClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }
    }
}
