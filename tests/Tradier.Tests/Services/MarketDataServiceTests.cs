using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tradier.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tradier.Tests;
#nullable disable
namespace Tradier.Tests.Services
{
    [TestClass()]
    public class MarketDataServiceTests : ServiceTestBase
    {
        public virtual MarketDataService service { get; set; }
        [TestInitialize()]
        public override void SetService()
        {
            this.service = new MarketDataService(base.Client);
        }

        [TestMethod()]
        public void GetQuotesTest()
        {
            var rs = this.service.GetQuotes(true, "AAPL", "GOOG", "MSFT").Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void GetOptionChainsTest()
        {
            var rs = this.service.GetOptionChains("AAPL", new DateTime(2025, 8, 29), true).Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void GetOptionStrikesTest()
        {
            var rs = this.service.GetOptionStrikes("AAPL", new DateTime(2025, 8, 29), true).Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void GetOptionExpirationsTest()
        {
            var rs = this.service.GetOptionExpirations("AAPL", new() { IncludeAllRoots = true, ShowContractSize = true, ShowExpirationType = true, ShowStrikes = true }).Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void LookupOptionSymbolsTest()
        {
            var rs = this.service.LookupOptionSymbols("AAPL").Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void GetHistoricalQuotesTest()
        {
            var rs = this.service.GetHistoricalQuotes("AAPL", new Tradier.Request.MarketHistoricalQuotesRequest()
            {
                Start = DateTime.Now.AddDays(-60),
                End = DateTime.Now.AddDays(60),
                Interval = Enumerations.IntervalType.Monthly,
                SessionFilter = Enumerations.SessionFilter.Open
            }).Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void GetTimeAndSalesTest()
        {
            var rs = this.service.GetTimeAndSales("AAPL", new Tradier.Request.MarketTimeAndSalesRequest()
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(60),
                Interval = Enumerations.IntervalScope.OneMinute,
                Session = Enumerations.SessionFilter.All
            }).Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void GetETBSecuritiesTest()
        {
            var rs = this.service.GetETBSecurities().Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void GetClockTest()
        {
            var rs = this.service.GetClock().Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void GetCalendarTest()
        {
            var rs = this.service.GetCalendar().Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void SearchCompaniesTest()
        {
            var rs = this.service.SearchCompanies("AAPL").Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void LookupSymbolTest()
        {
            var rs = this.service.LookupSymbol("AAPL").Result;
            this.AssertResponse(rs);
        }
    }
}
#nullable enable