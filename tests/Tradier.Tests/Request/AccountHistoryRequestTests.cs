using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tradier.Enumerations;
using Tradier.Request;

namespace Tradier.Tests.Request
{
    [TestClass]
    public class AccountHistoryRequestTests : RequestTestsBase
    {
        [TestMethod]
        public void ToQueryString_WithAllProperties_ReturnsCorrectString()
        {
            var request = new AccountHistoryRequest
            {
                Page = "2",
                Limit = "50",
                ActivityType = ActivityType.Trade,
                Symbol = "AAPL",
                ExactMatch = true,
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 12, 31)
            };

            AssertQueryStringContains(request, "page=2");
            AssertQueryStringContains(request, "limit=50");
            AssertQueryStringContains(request, "type=trade");
            AssertQueryStringContains(request, "symbol=AAPL");
            AssertQueryStringContains(request, "exact=true");
            AssertQueryStringContains(request, "start=2023-01-01");
            AssertQueryStringContains(request, "end=2023-12-31");
        }

        [TestMethod]
        public void ToQueryString_WithMinimalProperties_ReturnsCorrectString()
        {
            var request = new AccountHistoryRequest
            {
                ActivityType = ActivityType.Dividend
            };

            AssertQueryString(request, "type=dividend");
        }

        [TestMethod]
        public void ToQueryString_WithExactMatchFalse_DoesNotIncludeExactParameter()
        {
            var request = new AccountHistoryRequest
            {
                ActivityType = ActivityType.Trade,
                Symbol = "MSFT",
                ExactMatch = false
            };

            AssertQueryStringContains(request, "symbol=MSFT");
            AssertQueryStringNotContains(request, "exact=");
        }

        [TestMethod]
        public void ToQueryString_WithExactMatchTrue_IncludesExactParameter()
        {
            var request = new AccountHistoryRequest
            {
                ActivityType = ActivityType.Trade,
                Symbol = "TSLA",
                ExactMatch = true
            };

            AssertQueryStringContains(request, "symbol=TSLA");
            AssertQueryStringContains(request, "exact=true");
        }

        [TestMethod]
        public void ParseQueryString_ReturnsToQueryStringResult()
        {
            var request = new AccountHistoryRequest
            {
                ActivityType = ActivityType.Trade,
                Symbol = "NVDA",
                ExactMatch = true
            };

            Assert.AreEqual(request.ToQueryString(), request.ParseQueryString());
        }

        [TestMethod]
        public void ActivityType_IsCorrectlyConverted()
        {
            var testCases = new[]
            {
                (ActivityType.Trade, "trade"),
                (ActivityType.Option, "option"),
                (ActivityType.Ach, "ach"),
                (ActivityType.Wire, "wire"),
                (ActivityType.Dividend, "dividend"),
                (ActivityType.Fee, "fee"),
                (ActivityType.Tax, "tax"),
                (ActivityType.Journal, "journal"),
                (ActivityType.Check, "check"),
                (ActivityType.Transfer, "transfer"),
                (ActivityType.Adjustment, "adjustment"),
                (ActivityType.Interest, "interest")
            };

            foreach (var (activityType, expectedValue) in testCases)
            {
                var request = new AccountHistoryRequest { ActivityType = activityType };
                AssertQueryStringContains(request, $"type={expectedValue}");
            }
        }

        [TestMethod]
        public void Type_Property_ReturnsActivityTypeAsString()
        {
            var request = new AccountHistoryRequest { ActivityType = ActivityType.Trade };
            Assert.AreEqual("trade", request.Type);
            
            request.ActivityType = ActivityType.Dividend;
            Assert.AreEqual("dividend", request.Type);
        }

        [TestMethod]
        public void ToQueryString_InheritsFromPaginationRequest()
        {
            var request = new AccountHistoryRequest
            {
                Page = "1",
                Limit = "100",
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 1, 31),
                ActivityType = ActivityType.Trade
            };

            var queryString = request.ToQueryString();
            
            // Verify pagination properties work
            Assert.IsTrue(queryString.Contains("page=1"));
            Assert.IsTrue(queryString.Contains("limit=100"));
            Assert.IsTrue(queryString.Contains("start=2023-01-01"));
            Assert.IsTrue(queryString.Contains("end=2023-01-31"));
            Assert.IsTrue(queryString.Contains("type=trade"));
        }
    }
}