using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tradier.Enumerations;
using Tradier.Request;

namespace Tradier.Tests.Request
{
    [TestClass]
    public class MarketTimeAndSalesRequestTests : RequestTestsBase
    {
        [TestMethod]
        public void ToQueryString_WithAllProperties_ReturnsCorrectString()
        {
            var request = new MarketTimeAndSalesRequest
            {
                Interval = IntervalScope.OneMinute,
                Session = SessionFilter.All,
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 12, 31)
            };

            AssertQueryStringContains(request, "interval=1min");
            AssertQueryStringContains(request, "session_filter=all");
            AssertQueryStringContains(request, "start=2023-01-01");
            AssertQueryStringContains(request, "end=2023-12-31");
        }

        [TestMethod]
        public void ToQueryString_WithSpecialIntervals_ReturnsCorrectFormats()
        {
            var testCases = new[]
            {
                (IntervalScope.OneMinute, "1min"),
                (IntervalScope.FiveMinutes, "5min"),
                (IntervalScope.FifteenMinutes, "15min"),
                (IntervalScope.Tick, "tick")
            };

            foreach (var (interval, expectedValue) in testCases)
            {
                var request = new MarketTimeAndSalesRequest
                {
                    Interval = interval,
                    Session = SessionFilter.All
                };

                AssertQueryStringContains(request, $"interval={expectedValue}");
            }
        }

        [TestMethod]
        public void ToQueryString_WithMinimalProperties_ReturnsCorrectString()
        {
            var request = new MarketTimeAndSalesRequest
            {
                Interval = IntervalScope.OneMinute,
                Session = SessionFilter.Open
            };

            AssertQueryString(request, "interval=1min&session_filter=open");
        }

        [TestMethod]
        public void ToQueryString_WithOnlyDates_ReturnsDateParameters()
        {
            var request = new MarketTimeAndSalesRequest
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
            var request = new MarketTimeAndSalesRequest
            {
                Interval = IntervalScope.FiveMinutes,
                Session = SessionFilter.All
            };

            Assert.AreEqual(request.ToQueryString(), request.ParseQuery());
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
                var request = new MarketTimeAndSalesRequest
                {
                    Interval = IntervalScope.OneMinute,
                    Session = sessionFilter
                };

                AssertQueryStringContains(request, $"session_filter={expectedValue}");
            }
        }
    }
}