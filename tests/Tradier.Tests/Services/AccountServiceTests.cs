using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tradier.Services;
using Tradier.Tests;
#nullable disable
namespace Tradier.Tests.Services
{
    [TestClass()]
    public class AccountServiceTests : ServiceTestBase
    {
        public AccountService service { get; set; }

        [TestInitialize()]
        public override void SetService()
        {
            service = new AccountService(Client);
        }

        [TestMethod()]
        public async Task AccountService()
        {
            var myProfile = await service.GetUserProfile();
            AssertResponse(myProfile);
            var id = myProfile.Profile.Id;

            var myBalance = await service.GetBalances(id);
            //AssertResponse(myBalance);

            var myPositions = await service.GetPositions(id);
            AssertResponse(myPositions);

            var history = await service.GetHistory(id);
            var historyWithQuery = await service.GetHistory(id, new Tradier.Request.AccountHistoryRequest()
            {
                ActivityType = Enumerations.ActivityType.Trade,
                End = DateTime.Now,
                ExactMatch = true,
                Limit = "10",
                Start = DateTime.Now.AddDays(-30)
            });
            AssertResponse(history);
            AssertResponse(historyWithQuery);

            var accountGainLoss = await service.GetGainLoss(id);
            var accountGainLossWithQuery = await service.GetGainLoss(id, new Tradier.Request.GainLossOptions()
            {
                End = DateTime.Now,
                Start = DateTime.Now.AddDays(-30),
                Limit = "10",
                Page = "1",
                Sort = Enumerations.SortDirection.Asc,
                SortBy = Enumerations.SortResult.OpenDate,
                Symbol = "AAPL"
            });
            AssertResponse(accountGainLoss);
            AssertResponse(accountGainLossWithQuery);

            var orders = await service.GetOrders(id);
            //var order = await service.GetOrder(id, "1");
            AssertResponse(orders);
            //AssertResponse(order);

        }
    }
}
#nullable restore