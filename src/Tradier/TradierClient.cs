using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tradier.Response;
using Tradier.Services;

namespace Tradier
{
    public partial class TradierClient : ITradierClient
    {
        public async Task<TResponse> Get<TResponse>(string endpoint, CancellationToken token = default) where TResponse : ITradierResponse, new()
        {
            try
            {
                var response = new TResponse();
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(client.BaseAddress!, endpoint));

                using var httpResponse = await client.SendAsync(request, token);
                await response.ParseAsync(httpResponse, GetJsonSerializerOptions(), token);

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"HTTP request failed for endpoint '{endpoint}': {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new OperationCanceledException($"Request to endpoint '{endpoint}' was cancelled or timed out.", ex);
            }
        }

        public async Task<TResponse> Post<TResponse>(string endpoint, Dictionary<string, string>? body = null, CancellationToken token = default) where TResponse : ITradierResponse, new()
        {
            try
            {
                var response = new TResponse();
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(client.BaseAddress!, endpoint));

                if (body != null && body.Any())
                {
                    request.Content = new FormUrlEncodedContent(body);
                }

                using var httpResponse = await client.SendAsync(request, token);
                await response.ParseAsync(httpResponse, GetJsonSerializerOptions(), token);

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"HTTP request failed for endpoint '{endpoint}': {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new OperationCanceledException($"Request to endpoint '{endpoint}' was cancelled or timed out.", ex);
            }
        }

        public async Task<TResponse> Put<TResponse>(string endpoint, Dictionary<string, string>? body = null, CancellationToken token = default) where TResponse : ITradierResponse, new()
        {
            try
            {
                var response = new TResponse();
                var request = new HttpRequestMessage(HttpMethod.Put, new Uri(client.BaseAddress!, endpoint));

                if (body != null && body.Any())
                {
                    request.Content = new FormUrlEncodedContent(body);
                }

                using var httpResponse = await client.SendAsync(request, token);
                await response.ParseAsync(httpResponse, GetJsonSerializerOptions(), token);

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"HTTP request failed for endpoint '{endpoint}': {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new OperationCanceledException($"Request to endpoint '{endpoint}' was cancelled or timed out.", ex);
            }
        }

        public async Task<TResponse> Delete<TResponse>(string endpoint, CancellationToken token = default) where TResponse : ITradierResponse, new()
        {
            try
            {
                var response = new TResponse();
                var request = new HttpRequestMessage(HttpMethod.Delete, new Uri(client.BaseAddress!, endpoint));

                using var httpResponse = await client.SendAsync(request, token);
                await response.ParseAsync(httpResponse, GetJsonSerializerOptions(), token);

                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"HTTP request failed for endpoint '{endpoint}': {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new OperationCanceledException($"Request to endpoint '{endpoint}' was cancelled or timed out.", ex);
            }
        }
    }

    /// <summary>
    /// Production client for the Tradier Brokerage API.
    /// For paper trading, use <see cref="TradierSandboxClient"/> instead.
    /// </summary>
    public partial class TradierClient : IDisposable
    {
        private readonly HttpClient client;
        private TradierAuthentication auth = null!;

        internal virtual string BaseAddress => "https://api.tradier.com/v1/";
        internal virtual string StreamAddress => "https://stream.tradier.com/v1/";
        private readonly bool _disposeHttpClient = false;


        /// <summary>
        /// Creates a new Tradier client with explicit authentication. The HttpClient will be created internally and disposed by this client.
        /// </summary>
        /// <param name="authentication">The authentication credentials to use.</param>
        public TradierClient(TradierAuthentication authentication)
        {
            this.client = new HttpClient();
            _disposeHttpClient = true;
            this.auth = authentication ?? throw new ArgumentNullException(nameof(authentication));
            InitializeClient(authentication.AccessToken);
        }

        /// <summary>
        /// Creates a new Tradier client with a custom HttpClient and explicit authentication.
        /// </summary>
        /// <param name="client">The HttpClient to use for requests.</param>
        /// <param name="authentication">The authentication credentials to use.</param>
        public TradierClient(HttpClient client, TradierAuthentication authentication)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.auth = authentication ?? throw new ArgumentNullException(nameof(authentication));
            InitializeClient(authentication.AccessToken);
        }

        internal void InitializeClient(string accessToken)
        {
            this.client.BaseAddress ??= new Uri(BaseAddress);
            this.auth ??= new TradierAuthentication(accessToken);
            this.auth.ApplyAuthentication(this.client);
        }

        /// <summary>
        /// Gets the JSON serializer options optimized for Tradier API responses.
        /// </summary>
        /// <returns>Configured JSON serializer options.</returns>
        protected virtual JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                WriteIndented = false,
                Converters =
                {
                    new Converters.EnumStringConverter<Enumerations.AccountStatus>(),
                    new Converters.EnumStringConverter<Enumerations.AccountType>(),
                    new Converters.EnumStringConverter<Enumerations.AccountClassification>(),
                    new Converters.EnumStringConverter<Enumerations.OrderType>(),
                    new Converters.EnumStringConverter<Enumerations.OrderSide>(),
                    new Converters.EnumStringConverter<Enumerations.OrderStatus>(),
                    new Converters.EnumStringConverter<Enumerations.OrderDuration>(),
                    new Converters.EnumStringConverter<Enumerations.OrderClass>(),
                    new Converters.EnumStringConverter<Enumerations.SecurityType>(),
                    new Converters.EnumStringConverter<Enumerations.OptionType>(),
                    new Converters.UnixTimestampConverter()
                }
            };
        }

        public void Dispose()
        {
            if (_disposeHttpClient)
            {
                client.Dispose();
            }
        }
    }
}
