using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tradier.Enumerations;
using Tradier.Request;

namespace Tradier.Tests.Request
{
    [TestClass]
    public class GainLossOptionsTests : RequestTestsBase
    {
        [TestMethod]
        public void ToQueryString_WithAllProperties_ReturnsCorrectString()
        {
            var request = new GainLossOptions
            {
                Page = "2",
                Limit = "50",
                Start = new DateTime(2023, 1, 1),
                End = new DateTime(2023, 12, 31),
                SortBy = SortResult.CloseDate,
                Sort = SortDirection.Desc,
                Symbol = "AAPL"
            };

            AssertQueryStringContains(request, "page=2");
            AssertQueryStringContains(request, "limit=50");
            AssertQueryStringContains(request, "start=2023-01-01");
            AssertQueryStringContains(request, "end=2023-12-31");
            AssertQueryStringContains(request, "sort_by=closedate");
            AssertQueryStringContains(request, "sort=desc");
            AssertQueryStringContains(request, "symbol=AAPL");
        }

        [TestMethod]
        public void ToQueryString_WithMinimalProperties_ReturnsCorrectString()
        {
            var request = new GainLossOptions
            {
                Symbol = "MSFT"
            };

            AssertQueryString(request, "symbol=MSFT");
        }

        [TestMethod]
        public void ToQueryString_WithOnlySortProperties_ReturnsCorrectString()
        {
            var request = new GainLossOptions
            {
                SortBy = SortResult.OpenDate,
                Sort = SortDirection.Asc
            };

            AssertQueryString(request, "sort_by=opendate&sort=asc");
        }

        [TestMethod]
        public void ToQueryString_WithNullableProperties_DoesNotIncludeNullValues()
        {
            var request = new GainLossOptions
            {
                Symbol = "TSLA",
                SortBy = null,
                Sort = null
            };

            AssertQueryString(request, "symbol=TSLA");
        }

        [TestMethod]
        public void ParseQueryString_ReturnsToQueryStringResult()
        {
            var request = new GainLossOptions
            {
                Symbol = "NVDA",
                SortBy = SortResult.OpenDate,
                Sort = SortDirection.Asc
            };

            Assert.AreEqual(request.ToQueryString(), request.ParseQueryString());
        }

        [TestMethod]
        public void SortResult_EnumValues_AreCorrectlyFormatted()
        {
            var testCases = new[]
            {
                (SortResult.OpenDate, "opendate"),
                (SortResult.CloseDate, "closedate"),
            };

            foreach (var (sortResult, expectedValue) in testCases)
            {
                var request = new GainLossOptions 
                { 
                    SortBy = sortResult,
                    Sort = SortDirection.Asc 
                };

                AssertQueryStringContains(request, $"sort_by={expectedValue}");
            }
        }

        [TestMethod]
        public void SortDirection_EnumValues_AreCorrectlyFormatted()
        {
            var testCases = new[]
            {
                (SortDirection.Asc, "asc"),
                (SortDirection.Desc, "desc")
            };

            foreach (var (sortDirection, expectedValue) in testCases)
            {
                var request = new GainLossOptions 
                { 
                    SortBy = SortResult.OpenDate,
                    Sort = sortDirection 
                };

                AssertQueryStringContains(request, $"sort={expectedValue}");
            }
        }

        [TestMethod]
        public void ToQueryString_InheritsFromPaginationRequest()
        {
            var request = new GainLossOptions
            {
                Page = "3",
                Limit = "25",
                Start = new DateTime(2023, 6, 1),
                End = new DateTime(2023, 6, 30),
                Symbol = "AMZN"
            };

            var queryString = request.ToQueryString();
            
            // Verify pagination properties work
            Assert.IsTrue(queryString.Contains("page=3"));
            Assert.IsTrue(queryString.Contains("limit=25"));
            Assert.IsTrue(queryString.Contains("start=2023-06-01"));
            Assert.IsTrue(queryString.Contains("end=2023-06-30"));
            Assert.IsTrue(queryString.Contains("symbol=AMZN"));
        }

        [TestMethod]
        public void ToQueryString_WithSpecialCharactersInSymbol_UrlEncodesCorrectly()
        {
            var request = new GainLossOptions { Symbol = "BRK.A" };
            AssertQueryStringContains(request, "symbol=BRK.A");
        }
    }
}