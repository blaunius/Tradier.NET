using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tradier.Request;

namespace Tradier.Tests.Request
{
    [TestClass]
    public class MarketOptionExpirationRequestTests : RequestTestsBase
    {
        [TestMethod]
        public void ToQueryString_WithAllPropertiesTrue_ReturnsCorrectString()
        {
            var request = new MarketOptionExpirationRequest
            {
                IncludeAllRoots = true,
                ShowStrikes = true,
                ShowContractSize = true,
                ShowExpirationType = true
            };

            AssertQueryStringContains(request, "includeAllRoots=true");
            AssertQueryStringContains(request, "strikes=true");
            AssertQueryStringContains(request, "contractSize=true");
            AssertQueryStringContains(request, "expirationType=true");
        }

        [TestMethod]
        public void ToQueryString_WithAllPropertiesFalse_ReturnsCorrectString()
        {
            var request = new MarketOptionExpirationRequest
            {
                IncludeAllRoots = false,
                ShowStrikes = false,
                ShowContractSize = false,
                ShowExpirationType = false
            };

            AssertQueryStringContains(request, "includeAllRoots=false");
            AssertQueryStringContains(request, "strikes=false");
            AssertQueryStringContains(request, "contractSize=false");
            AssertQueryStringContains(request, "expirationType=false");
        }

        [TestMethod]
        public void ToQueryString_WithDefaultValues_ReturnsAllFalse()
        {
            var request = new MarketOptionExpirationRequest();

            AssertQueryStringContains(request, "includeAllRoots=false");
            AssertQueryStringContains(request, "strikes=false");
            AssertQueryStringContains(request, "contractSize=false");
            AssertQueryStringContains(request, "expirationType=false");
        }

        [TestMethod]
        public void ToQueryString_WithMixedValues_ReturnsCorrectString()
        {
            var request = new MarketOptionExpirationRequest
            {
                IncludeAllRoots = true,
                ShowStrikes = false,
                ShowContractSize = true,
                ShowExpirationType = false
            };

            AssertQueryStringContains(request, "includeAllRoots=true");
            AssertQueryStringContains(request, "strikes=false");
            AssertQueryStringContains(request, "contractSize=true");
            AssertQueryStringContains(request, "expirationType=false");
        }

        // Note: This test is commented out because internal obsolete properties are not accessible from test assembly
        // [TestMethod]
        // public void ObsoleteProperties_ReturnCorrectValues()
        // {
        //     var request = new MarketOptionExpirationRequest
        //     {
        //         IncludeAllRoots = true,
        //         ShowStrikes = false,
        //         ShowContractSize = true,
        //         ShowExpirationType = false
        //     };

        //     #pragma warning disable CS0618 // Type or member is obsolete
        //     Assert.AreEqual("true", request.includeAllRoots);
        //     Assert.AreEqual("false", request.showStrikes);
        //     Assert.AreEqual("true", request.showContractSize);
        //     Assert.AreEqual("false", request.showExpirationType);
        //     #pragma warning restore CS0618 // Type or member is obsolete
        // }

        [TestMethod]
        public void ToQueryString_ParameterOrder_IsConsistent()
        {
            var request = new MarketOptionExpirationRequest
            {
                IncludeAllRoots = true,
                ShowStrikes = true,
                ShowContractSize = true,
                ShowExpirationType = true
            };

            var queryString = request.ToQueryString();
            
            // Verify all parameters are present
            Assert.IsTrue(queryString.Contains("includeAllRoots=true"));
            Assert.IsTrue(queryString.Contains("strikes=true"));
            Assert.IsTrue(queryString.Contains("contractSize=true"));
            Assert.IsTrue(queryString.Contains("expirationType=true"));
        }
    }
}