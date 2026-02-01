using System.Net;
using System.Text;
using FluentAssertions;
using Tradier.Response;
using Tradier.Model;

namespace Tradier.Tests.Response;

[TestClass]
public class AccountBalancesResponseTests
{
    [TestMethod]
    public async Task ParseAsync_WithValidBalanceResponse_ShouldParseCorrectly()
    {
        // Arrange
        var json = @"{
            ""balances"": {
                ""account_number"": ""TEST12345"",
                ""account_type"": ""margin"",
                ""total_equity"": 50000.75,
                ""total_cash"": 25000.50,
                ""long_market_value"": 30000.25,
                ""pending_orders_count"": 2
            }
        }";

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new AccountBalancesResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue();
        response.IsSuccessful.Should().BeTrue();
        response.HasBalance.Should().BeTrue();
        response.Balance.Should().NotBeNull();
        response.Balance!.AccountNumber.Should().Be("TEST12345");
        response.Balance.TotalEquity.Should().Be(50000.75m);
        response.Balance.TotalCash.Should().Be(25000.50m);
        response.Balance.LongMarketValue.Should().Be(30000.25m);
        response.Balance.PendingOrdersCount.Should().Be(2);
    }

    [TestMethod]
    public async Task ParseAsync_WithNullBalanceResponse_ShouldHandleGracefully()
    {
        // Arrange - JSON null for balances indicates no balance data
        var json = @"{""balances"":null}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new AccountBalancesResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert - when API returns null balances, HasBalance should be false
        result.Should().BeTrue();
        response.IsSuccessful.Should().BeTrue();
        response.HasBalance.Should().BeFalse(); // null balances means no balance data
        response.Balance.Should().BeNull();
    }

    [TestMethod]
    public async Task ParseAsync_WithMissingBalances_ShouldHandleGracefully()
    {
        // Arrange
        var json = @"{}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new AccountBalancesResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue();
        response.IsSuccessful.Should().BeTrue();
        response.HasBalance.Should().BeFalse();
        response.Balance.Should().BeNull();
    }

    [TestMethod]
    public async Task ParseAsync_WithErrorResponse_ShouldHandleError()
    {
        // Arrange
        var errorJson = @"{""fault"":{""faultstring"":""Invalid account""}}";
        var content = new StringContent(errorJson, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = content };

        var response = new AccountBalancesResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeFalse();
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.HasBalance.Should().BeFalse();
        response.Balance.Should().BeNull();
        response.ErrorMessage.Should().Contain("Invalid account");
    }

    [TestMethod]
    public async Task ParseAsync_WithCompleteBalanceData_ShouldCalculateProperties()
    {
        // Arrange
        var json = @"{
            ""balances"": {
                ""account_number"": ""TEST12345"",
                ""account_type"": ""margin"",
                ""total_equity"": 30000.00,
                ""open_pl"": 1500.50,
                ""close_pl"": -500.25,
                ""total_cash"": 10000.00,
                ""uncleared_funds"": 1000.00,
                ""pending_cash"": 500.00
            }
        }";

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new AccountBalancesResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue();
        response.Balance.Should().NotBeNull();
        response.Balance!.TotalProfitLoss.Should().Be(1000.25m); // 1500.50 - 500.25
        response.Balance.AvailableCash.Should().Be(8500.00m); // 10000 - 1000 - 500
        response.Balance.MeetsPatternDayTradingRequirement.Should().BeTrue(); // >= 25000
    }
}