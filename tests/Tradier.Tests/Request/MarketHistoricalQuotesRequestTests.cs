using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tradier.Enumerations;
using Tradier.Request;

namespace Tradier.Tests.Request
{
    [TestClass]
    public class MarketHistoricalQuotesRequestTests : RequestTestsBase
    {
        [TestMethod]
        public void ToQueryString_WithAllProperties_ReturnsCorrectString()
        {
            var request = new MarketHistoricalQuotesRequest
            {
                Interval = IntervalType.Daily,
                SessionFilter = SessionFilter.All,
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 12, 31)
            };

            AssertQueryStringContains(request, "interval=daily");
            AssertQueryStringContains(request, "session_filter=all");
            AssertQueryStringContains(request, "start=2023-01-01");
            AssertQueryStringContains(request, "end=2023-12-31");
        }

        [TestMethod]
        public void ToQueryString_WithMinimalProperties_ReturnsCorrectString()
        {
            var request = new MarketHistoricalQuotesRequest
            {
                Interval = IntervalType.Weekly,
                SessionFilter = SessionFilter.Open
            };

            AssertQueryString(request, "interval=weekly&session_filter=open");
        }

        [TestMethod]
        public void ToQueryString_WithOnlyDates_ReturnsDateParameters()
        {
            var request = new MarketHistoricalQuotesRequest
            {
                Start = new DateTime(2023, 6, 15),
                End = new DateTime(2023, 6, 20)
            };

            var result = request.ToQueryString();
            Assert.IsTrue(result.Contains("start=2023-06-15"));
            Assert.IsTrue(result.Contains("end=2023-06-20"));
        }

        [TestMethod]
        public void ParseQuery_ReturnsToQueryStringResult()
        {
            var request = new MarketHistoricalQuotesRequest
            {
                Interval = IntervalType.Monthly,
                SessionFilter = SessionFilter.All
            };

            #pragma warning disable CS0618 // Type or member is obsolete
            Assert.AreEqual(request.ToQueryString(), request.ParseQuery());
            #pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        public void ToQueryString_IntervalTypesAreCorrectlyFormatted()
        {
            var testCases = new[]
            {
                (IntervalType.Daily, "daily"),
                (IntervalType.Weekly, "weekly"),
                (IntervalType.Monthly, "monthly")
            };

            foreach (var (intervalType, expectedValue) in testCases)
            {
                var request = new MarketHistoricalQuotesRequest
                {
                    Interval = intervalType,
                    SessionFilter = SessionFilter.All
                };

                AssertQueryStringContains(request, $"interval={expectedValue}");
            }
        }

        [TestMethod]
        public void ToQueryString_SessionFilterTypesAreCorrectlyFormatted()
        {
            var testCases = new[]
            {
                (SessionFilter.All, "all"),
                (SessionFilter.Open, "open")
            };

            foreach (var (sessionFilter, expectedValue) in testCases)
            {
                var request = new MarketHistoricalQuotesRequest
                {
                    Interval = IntervalType.Daily,
                    SessionFilter = sessionFilter
                };

                AssertQueryStringContains(request, $"session_filter={expectedValue}");
            }
        }
    }
}