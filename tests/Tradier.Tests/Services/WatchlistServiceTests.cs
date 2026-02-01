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
    public class WatchlistServiceTests : ServiceTestBase
    {
        public virtual WatchlistService service { get; set; }

        [TestInitialize()]
        public override void SetService()
        {
            service = new WatchlistService(this.Client);
        }

        [TestMethod()]
        public void GetWatchlistsTest()
        {
            var rs = this.service.GetWatchlists().Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void GetWatchlistTest()
        {
            var rs = this.service.GetWatchlist("public-22kngjmmr3").Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void CreateWatchlistTest()
        {
            var rs = this.service.CreateWatchlist("my-wl-1", new List<string> { "AAPL", "GOOGL" }).Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void UpdateWatchlistTest()
        {
            //this endpoint does not work through the api
            var allWatchlists = this.service.GetWatchlists().Result;
            var id = allWatchlists.Data?.Watchlists?.WatchlistsList.LastOrDefault()?.Id;
            var rs = this.service.UpdateWatchlist(id, "TestWL", [ "GOOGL" ]).Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void DeleteWatchlistTest()
        {
            var allWatchlists = this.service.GetWatchlists().Result;
            var id = allWatchlists.Data?.Watchlists?.WatchlistsList.LastOrDefault()?.Id;
            var rs = this.service.DeleteWatchlist(id).Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void AddSymbolsTest()
        {
            var allWatchlists = this.service.GetWatchlists().Result;
            var id = allWatchlists.Data?.Watchlists?.WatchlistsList.LastOrDefault()?.Id;
            var rs = this.service.AddSymbols(id, [ "GNL" ]).Result;
            this.AssertResponse(rs);
        }

        [TestMethod()]
        public void RemoveSymbolTest()
        {
            var allWatchlists = this.service.GetWatchlists().Result;
            var wl = allWatchlists.Data?.Watchlists?.WatchlistsList.LastOrDefault();
            var id = wl?.Id;
            var wlq = this.service.GetWatchlist(id).Result;
            var symbol = wlq.Data?.Watchlist?.Items?.Item.LastOrDefault()?.Symbol; 
            var rs = this.service.RemoveSymbol(id, symbol).Result;
            this.AssertResponse(rs);
        }
    }
}
#nullable restore