using Microsoft.Extensions.Configuration;
using Tradier.Services;
using Tradier.Response;

namespace Tradier.Tests
{
    [TestClass]
    public class ServiceTestBase
    {
        public IConfigurationRoot config = null!;
        public TradierClient? Client { get; set; }
        public TradierSandboxClient? SandboxClient { get; set; }
        
        public ServiceTestBase()
        {
            config = new ConfigurationBuilder()
                .AddUserSecrets(typeof(ServiceTestBase).Assembly)
                .Build();
        }

        [TestInitialize]
        public void Init()
        {
            var sandboxToken = config["Tradier:AccessTokens:Sandbox"];
            var productionToken = config["Tradier:AccessTokens:Production"];
            
            if (!string.IsNullOrEmpty(sandboxToken))
            {
                SandboxClient = new TradierSandboxClient(new TradierAuthentication(sandboxToken));
            }
            
            if (!string.IsNullOrEmpty(productionToken))
            {
                Client = new TradierClient(new TradierAuthentication(productionToken));
            }
            
            SetService();
        }

        public virtual void SetService()
        {
        }

        public void AssertResponse(ITradierResponse rs)
        {
            Assert.IsNotNull(rs);
            Assert.IsTrue(rs.IsSuccessful, $"Response was not successful for {rs.GetType().Name}: {rs.ErrorMessage}");
            Assert.IsTrue(rs.StatusCode == System.Net.HttpStatusCode.OK, $"Response status was not OK for {rs.GetType().Name}: {rs.StatusCode}");
        }
    }
}
