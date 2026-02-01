using System.Net.Http.Headers;

namespace Tradier
{
    /// <summary>
    /// Handles authentication for Tradier API requests.
    /// </summary>
    public class TradierAuthentication
    {
        /// <summary>
        /// Creates authentication with the API key from your Tradier Account.
        /// </summary>
        /// <param name="apiKey">Your Tradier API key.</param>
        public TradierAuthentication(string apiKey)
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        /// <summary>
        /// Applies authentication headers to an HttpClient.
        /// </summary>
        /// <param name="client">The HttpClient to configure.</param>
        public void ApplyAuthentication(HttpClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// Gets the API access token.
        /// </summary>
        public string ApiKey { get; }
    }
}
