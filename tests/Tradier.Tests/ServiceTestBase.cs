using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tradier.Services;
using Tradier.Response;

namespace Tradier.Tests
{
    [TestClass]
    public class ServiceTestBase
    {
        public IConfigurationRoot config;
        public TradierClient? Client { get; set; }
        public TradierSandboxClient? SandboxClient { get; set; }
        public ServiceTestBase()
        {
            config = new ConfigurationBuilder()
                .AddUserSecrets(typeof(ServiceTestBase).Assembly)
                .Build();
        }

        [TestInitialize()]
        public void Init()
        {
            TradierConfig.RedirectUri = config["Tradier:RedirectUri"];
            this.SandboxClient = new TradierSandboxClient(new TradierAuthentication(config["Tradier:AccessTokens:Sandbox"], TradierConfig.RedirectUri));
            this.Client = new TradierClient(new TradierAuthentication(config["Tradier:AccessTokens:Production"], TradierConfig.RedirectUri));
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
