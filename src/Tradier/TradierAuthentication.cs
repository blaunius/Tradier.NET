using System.Net.Http.Headers;
using System.Text.Json;

namespace Tradier
{
    /// <summary>
    /// Handles authentication for Tradier API requests.
    /// </summary>
    public class TradierAuthentication
    {
        /// <summary>
        /// Creates authentication with just an access token.
        /// This is the simplest way to authenticate for API usage.
        /// </summary>
        /// <param name="accessToken">Your Tradier API access token.</param>
        public TradierAuthentication(string accessToken)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        }

        /// <summary>
        /// Creates authentication with access token and redirect URI.
        /// The redirect URI is only needed if you plan to use OAuth token exchange.
        /// </summary>
        /// <param name="accessToken">Your Tradier API access token.</param>
        /// <param name="redirectUri">OAuth redirect URI (optional for API-only usage).</param>
        public TradierAuthentication(string accessToken, string? redirectUri)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            
            if (!string.IsNullOrEmpty(redirectUri))
            {
                RedirectUri = new Uri(redirectUri);
            }
        }

        /// <summary>
        /// Gets or sets the API access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets the OAuth redirect URI. Only needed for OAuth token exchange flows.
        /// </summary>
        public Uri? RedirectUri { get; }

        /// <summary>
        /// Gets or sets the OAuth client ID.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the OAuth client secret.
        /// </summary>
        public string? ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the OAuth authorization code.
        /// </summary>
        public string? AuthorizationCode { get; set; }

        /// <summary>
        /// Gets or sets the OAuth refresh token.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the access token expiry time.
        /// </summary>
        public DateTime? AccessTokenExpiry { get; set; }

        private const string AUTH_ENDPOINT = "https://api.tradier.com/v1/oauth/authorize";
        private const string TOKEN_ENDPOINT = "https://api.tradier.com/v1/oauth/token";

        /// <summary>
        /// Exchanges an authorization code for an access token.
        /// Requires RedirectUri, ClientId, ClientSecret, and AuthorizationCode to be set.
        /// </summary>
        public async Task ExchangeCodeForTokenAsync()
        {
            if (RedirectUri == null)
                throw new InvalidOperationException("RedirectUri must be set for OAuth token exchange.");
            if (string.IsNullOrEmpty(ClientId))
                throw new InvalidOperationException("ClientId must be set for OAuth token exchange.");
            if (string.IsNullOrEmpty(ClientSecret))
                throw new InvalidOperationException("ClientSecret must be set for OAuth token exchange.");

            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", AuthorizationCode ?? string.Empty),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", RedirectUri.ToString())
            });

            var response = await client.PostAsync(TOKEN_ENDPOINT, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

            AccessToken = tokenResponse?.access_token 
                ?? throw new InvalidOperationException("Token exchange failed: no access token returned.");
            RefreshToken = tokenResponse?.refresh_token;
            AccessTokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse?.expires_in ?? 0);
        }

        /// <summary>
        /// Refreshes the access token using the refresh token.
        /// Requires ClientId, ClientSecret, and RefreshToken to be set.
        /// </summary>
        public async Task RefreshAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(ClientId))
                throw new InvalidOperationException("ClientId must be set for token refresh.");
            if (string.IsNullOrEmpty(ClientSecret))
                throw new InvalidOperationException("ClientSecret must be set for token refresh.");
            if (string.IsNullOrEmpty(RefreshToken))
                throw new InvalidOperationException("RefreshToken must be set for token refresh.");

            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", RefreshToken),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret)
            });

            var response = await client.PostAsync(TOKEN_ENDPOINT, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

            AccessToken = tokenResponse?.access_token 
                ?? throw new InvalidOperationException("Token refresh failed: no access token returned.");
            AccessTokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse?.expires_in ?? 0);
        }

        /// <summary>
        /// Applies authentication headers to an HttpClient.
        /// </summary>
        /// <param name="client">The HttpClient to configure.</param>
        public void ApplyAuthentication(HttpClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private class TokenResponse
        {
            public string? access_token { get; set; }
            public string? refresh_token { get; set; }
            public int expires_in { get; set; }
        }
    }
}
