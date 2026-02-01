using Microsoft.Extensions.DependencyInjection;
using Tradier.Services;

namespace Tradier.Extensions
{
    /// <summary>
    /// Extension methods for configuring Tradier services with dependency injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Tradier API services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="accessToken">Your Tradier API access token.</param>
        /// <param name="useSandbox">If true, uses sandbox (paper trading). Default is true for safety.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddTradier(
            this IServiceCollection services,
            string accessToken,
            bool useSandbox = true)
        {
            return services.AddTradier(options =>
            {
                options.AccessToken = accessToken;
                options.UseSandbox = useSandbox;
            });
        }

        /// <summary>
        /// Adds Tradier API services to the service collection with configuration options.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Action to configure Tradier options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddTradier(
            this IServiceCollection services,
            Action<TradierOptions> configure)
        {
            var options = new TradierOptions();
            configure(options);

            if (string.IsNullOrEmpty(options.AccessToken))
                throw new ArgumentException("AccessToken is required.", nameof(configure));

            // Register options
            services.AddSingleton(options);

            // Register the appropriate client based on sandbox setting
            if (options.UseSandbox)
            {
                services.AddHttpClient<ITradierClient, TradierSandboxClient>((sp, client) =>
                {
                    var auth = new TradierAuthentication(options.AccessToken);
                    auth.ApplyAuthentication(client);
                });
            }
            else
            {
                services.AddHttpClient<ITradierClient, TradierClient>((sp, client) =>
                {
                    var auth = new TradierAuthentication(options.AccessToken);
                    auth.ApplyAuthentication(client);
                });
            }

            // Register services
            services.AddScoped<MarketDataService>();
            services.AddScoped<AccountService>();
            services.AddScoped<TradingService>();
            services.AddScoped<WatchlistService>();

            return services;
        }
    }

    /// <summary>
    /// Configuration options for Tradier API services.
    /// </summary>
    public class TradierOptions
    {
        /// <summary>
        /// Your Tradier API access token.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// If true, uses sandbox environment (paper trading). Default is true for safety.
        /// Set to false for live trading with real money.
        /// </summary>
        public bool UseSandbox { get; set; } = true;
    }
}
