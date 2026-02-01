using Microsoft.Extensions.DependencyInjection;
using Tradier.Services;

namespace Tradier
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
        /// <param name="apiKey">Your Tradier API key.</param>
        /// <param name="useSandbox">If true, uses sandbox (paper trading). Default is true for safety.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddTradier(
            this IServiceCollection services,
            string apiKey,
            bool useSandbox = true)
        {
            return services.AddTradier(options =>
            {
                options.ApiKey = apiKey;
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

            if (string.IsNullOrEmpty(options.ApiKey))
                throw new ArgumentException("ApiKey is required.", nameof(configure));

            // Register options
            services.AddSingleton(options);

            // Create authentication
            var auth = new TradierAuthentication(options.ApiKey);

            // Register the appropriate client based on sandbox setting using factory
            if (options.UseSandbox)
            {
                services.AddHttpClient("TradierSandboxClient", client =>
                {
                    client.BaseAddress = new Uri("https://sandbox.tradier.com/v1/");
                    auth.ApplyAuthentication(client);
                });

                services.AddScoped<ITradierClient>(sp =>
                {
                    var factory = sp.GetRequiredService<IHttpClientFactory>();
                    var httpClient = factory.CreateClient("TradierSandboxClient");
                    return new TradierSandboxClient(httpClient, auth);
                });
            }
            else
            {
                services.AddHttpClient("TradierProductionClient", client =>
                {
                    client.BaseAddress = new Uri("https://api.tradier.com/v1/");
                    auth.ApplyAuthentication(client);
                });

                services.AddScoped<ITradierClient>(sp =>
                {
                    var factory = sp.GetRequiredService<IHttpClientFactory>();
                    var httpClient = factory.CreateClient("TradierProductionClient");
                    return new TradierClient(httpClient, auth);
                });
            }

            // Register services
            services.AddScoped<MarketDataService>();
            services.AddScoped<AccountService>();
            services.AddScoped<TradingService>();
            services.AddScoped<WatchlistService>();
            
            // StreamingService only works with production client
            if (!options.UseSandbox)
            {
                services.AddScoped<StreamingService>();
            }

            return services;
        }
    }

    /// <summary>
    /// Configuration options for Tradier API services.
    /// </summary>
    public class TradierOptions
    {
        /// <summary>
        /// Your Tradier API key.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// If true, uses sandbox environment (paper trading). Default is true for safety.
        /// Set to false for live trading with real money.
        /// </summary>
        public bool UseSandbox { get; set; } = true;
    }
}
