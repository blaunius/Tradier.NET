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

        /// <summary>
        /// Creates a new service using the default client from <see cref="TradierConfig"/>.
        /// </summary>
        /// <remarks>
        /// This constructor uses the static TradierConfig which is not recommended for ASP.NET Core applications.
        /// For web applications, use dependency injection with <c>builder.Services.AddTradier()</c> instead.
        /// </remarks>
        public TradierService()
        {
            #pragma warning disable CS0618 // TradierConfig is obsolete but supported for simple scenarios
            this.client = TradierConfig.DefaultClient 
                ?? throw new InvalidOperationException(
                    "Default client is not set. Please initialize a TradierClient first, " +
                    "or use builder.Services.AddTradier() for dependency injection.");
            #pragma warning restore CS0618
        }
    }
}
