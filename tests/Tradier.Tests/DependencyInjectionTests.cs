using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tradier;
using Tradier.Extensions;
using Tradier.Services;

namespace Tradier.Tests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        private IConfigurationRoot _config = null!;
        private string _sandboxToken = null!;

        [TestInitialize]
        public void Init()
        {
            _config = new ConfigurationBuilder()
                .AddUserSecrets(typeof(DependencyInjectionTests).Assembly)
                .Build();
            
            _sandboxToken = _config["Tradier:AccessTokens:Sandbox"] 
                ?? throw new InvalidOperationException("Sandbox token not configured in user secrets");
        }

        [TestMethod]
        public void AddTradier_Sandbox_RegistersCorrectClient()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTradier(_sandboxToken, useSandbox: true);
            var provider = services.BuildServiceProvider();

            // Act
            var client = provider.GetRequiredService<ITradierClient>();

            // Assert
            Assert.IsNotNull(client);
            Assert.IsInstanceOfType(client, typeof(TradierSandboxClient));
        }

        [TestMethod]
        public void AddTradier_Production_RegistersCorrectClient()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTradier("test-token", useSandbox: false);
            var provider = services.BuildServiceProvider();

            // Act
            var client = provider.GetRequiredService<ITradierClient>();

            // Assert
            Assert.IsNotNull(client);
            Assert.IsInstanceOfType(client, typeof(TradierClient));
            Assert.IsNotInstanceOfType(client, typeof(TradierSandboxClient));
        }

        [TestMethod]
        public void AddTradier_RegistersMarketDataService()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTradier(_sandboxToken, useSandbox: true);
            var provider = services.BuildServiceProvider();

            // Act
            var service = provider.GetRequiredService<MarketDataService>();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public async Task AddTradier_Sandbox_GetClock_ReturnsValidResponse()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTradier(_sandboxToken, useSandbox: true);
            var provider = services.BuildServiceProvider();
            var service = provider.GetRequiredService<MarketDataService>();

            // Act
            var response = await service.GetClock();

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful, $"GetClock failed: {response.ErrorMessage}");
            Assert.IsNotNull(response.Clock);
        }

        [TestMethod]
        public async Task AddTradier_Sandbox_GetCalendar_ReturnsValidResponse()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTradier(_sandboxToken, useSandbox: true);
            var provider = services.BuildServiceProvider();
            var service = provider.GetRequiredService<MarketDataService>();

            // Act
            var response = await service.GetCalendar();

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccessful, $"GetCalendar failed: {response.ErrorMessage}");
        }

        [TestMethod]
        public void AddTradier_WithOptions_ConfiguresCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTradier(options =>
            {
                options.AccessToken = _sandboxToken;
                options.UseSandbox = true;
            });
            var provider = services.BuildServiceProvider();

            // Act
            var client = provider.GetRequiredService<ITradierClient>();
            var options = provider.GetRequiredService<TradierOptions>();

            // Assert
            Assert.IsNotNull(client);
            Assert.IsInstanceOfType(client, typeof(TradierSandboxClient));
            Assert.AreEqual(_sandboxToken, options.AccessToken);
            Assert.IsTrue(options.UseSandbox);
        }

        [TestMethod]
        public void AddTradier_WithoutToken_ThrowsArgumentException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                services.AddTradier(options =>
                {
                    options.AccessToken = "";
                    options.UseSandbox = true;
                });
            });
        }
    }
}
