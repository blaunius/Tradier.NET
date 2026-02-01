using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tradier;
using Tradier.Services;

namespace Tradier.Tests
{
    [TestClass]
    public class ConstructorTests
    {
        private IConfigurationRoot _config = null!;
        private string _sandboxToken = null!;

        [TestInitialize]
        public void Init()
        {
            _config = new ConfigurationBuilder()
                .AddUserSecrets(typeof(ConstructorTests).Assembly)
                .Build();
            
            _sandboxToken = _config["Tradier:AccessTokens:Sandbox"] 
                ?? throw new InvalidOperationException("Sandbox token not configured in user secrets");
        }

        #region TradierAuthentication Tests

        [TestMethod]
        public void TradierAuthentication_WithAccessToken_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            Assert.IsNotNull(auth);
            Assert.AreEqual(_sandboxToken, auth.ApiKey);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TradierAuthentication_WithNullToken_ThrowsArgumentNullException()
        {
            new TradierAuthentication(null!);
        }

        [TestMethod]
        public void TradierAuthentication_WithEmptyToken_CreatesInstance()
        {
            // Empty string is allowed (will fail at API call time)
            var auth = new TradierAuthentication("");
            Assert.IsNotNull(auth);
            Assert.AreEqual("", auth.ApiKey);
        }

        [TestMethod]
        public void TradierAuthentication_ApplyAuthentication_SetsHeader()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            var httpClient = new HttpClient();
            
            auth.ApplyAuthentication(httpClient);
            
            Assert.IsTrue(httpClient.DefaultRequestHeaders.Contains("Authorization"));
            Assert.AreEqual("application/json", httpClient.DefaultRequestHeaders.Accept.First().MediaType);
        }

        #endregion

        #region TradierClient Tests

        [TestMethod]
        public void TradierClient_WithAuthentication_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            using var client = new TradierClient(auth);
            
            Assert.IsNotNull(client);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TradierClient_WithNullAuthentication_ThrowsArgumentNullException()
        {
            new TradierClient((TradierAuthentication)null!);
        }

        [TestMethod]
        public void TradierClient_WithHttpClientAndAuthentication_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            var httpClient = new HttpClient();
            using var client = new TradierClient(httpClient, auth);
            
            Assert.IsNotNull(client);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TradierClient_WithNullHttpClient_ThrowsArgumentNullException()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            new TradierClient((HttpClient)null!, auth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TradierClient_WithHttpClientAndNullAuth_ThrowsArgumentNullException()
        {
            var httpClient = new HttpClient();
            new TradierClient(httpClient, null!);
        }

        #endregion

        #region TradierSandboxClient Tests

        [TestMethod]
        public void TradierSandboxClient_WithAuthentication_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            using var client = new TradierSandboxClient(auth);
            
            Assert.IsNotNull(client);
        }

        [TestMethod]
        public void TradierSandboxClient_WithHttpClientAndAuthentication_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            var httpClient = new HttpClient();
            using var client = new TradierSandboxClient(httpClient, auth);
            
            Assert.IsNotNull(client);
        }

        #endregion

        #region Service Constructor Tests

        [TestMethod]
        public void MarketDataService_WithClient_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            using var client = new TradierSandboxClient(auth);
            var service = new MarketDataService(client);
            
            Assert.IsNotNull(service);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MarketDataService_WithNullClient_ThrowsArgumentNullException()
        {
            new MarketDataService(null!);
        }

        [TestMethod]
        public void AccountService_WithClient_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            using var client = new TradierSandboxClient(auth);
            var service = new AccountService(client);
            
            Assert.IsNotNull(service);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AccountService_WithNullClient_ThrowsArgumentNullException()
        {
            new AccountService(null!);
        }

        [TestMethod]
        public void TradingService_WithClient_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            using var client = new TradierSandboxClient(auth);
            var service = new TradingService(client);
            
            Assert.IsNotNull(service);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TradingService_WithNullClient_ThrowsArgumentNullException()
        {
            new TradingService(null!);
        }

        [TestMethod]
        public void WatchlistService_WithClient_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            using var client = new TradierSandboxClient(auth);
            var service = new WatchlistService(client);
            
            Assert.IsNotNull(service);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WatchlistService_WithNullClient_ThrowsArgumentNullException()
        {
            new WatchlistService(null!);
        }

        [TestMethod]
        public void StreamingService_WithProductionClient_CreatesInstance()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            using var client = new TradierClient(auth); // Must be production client
            var service = new StreamingService(client);
            
            Assert.IsNotNull(service);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void StreamingService_WithSandboxClient_ThrowsNotSupportedException()
        {
            var auth = new TradierAuthentication(_sandboxToken);
            using var client = new TradierSandboxClient(auth);
            new StreamingService(client);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StreamingService_WithNullClient_ThrowsArgumentNullException()
        {
            new StreamingService(null!);
        }

        #endregion
    }
}
