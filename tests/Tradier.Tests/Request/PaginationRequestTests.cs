using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tradier.Enumerations;
using Tradier.Request;

namespace Tradier.Tests.Request
{
    [TestClass]
    public class PaginationRequestTests : RequestTestsBase
    {
        [TestMethod]
        public void ToQueryString_WithAllProperties_ReturnsCorrectString()
        {
            var request = new PaginationRequest
            {
                Page = "2",
                Limit = "50",
                Type = "trade",
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 12, 31)
            };

            AssertQueryStringContains(request, "page=2");
            AssertQueryStringContains(request, "limit=50");
            AssertQueryStringContains(request, "type=trade");
            AssertQueryStringContains(request, "start=2023-01-01");
            AssertQueryStringContains(request, "end=2023-12-31");
        }

        [TestMethod]
        public void ToQueryString_WithNoProperties_ReturnsEmptyString()
        {
            var request = new PaginationRequest();
            AssertEmptyQueryString(request);
        }

        [TestMethod]
        public void ToQueryString_WithOnlyPage_ReturnsPageParameter()
        {
            var request = new PaginationRequest { Page = "1" };
            AssertQueryString(request, "page=1");
        }

        [TestMethod]
        public void ToQueryString_WithOnlyDates_ReturnsDateParameters()
        {
            var request = new PaginationRequest
            {
                Start = new DateTime(2023, 6, 15),
                End = new DateTime(2023, 6, 20)
            };

            var result = request.ToQueryString();
            Assert.IsTrue(result.Contains("start=2023-06-15"));
            Assert.IsTrue(result.Contains("end=2023-06-20"));
        }

        [TestMethod]
        public void ParseQueryString_ReturnsToQueryStringResult()
        {
            var request = new PaginationRequest 
            { 
                Page = "1", 
                Limit = "25" 
            };

            Assert.AreEqual(request.ToQueryString(), request.ParseQueryString());
        }

        [TestMethod]
        public void ToQueryString_WithSpecialCharacters_UrlEncodesCorrectly()
        {
            var request = new PaginationRequest { Type = "test type" };
            AssertQueryStringContains(request, "type=test+type");
        }
    }
}