using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FluentAssertions;
using Tradier.Enumerations;
using Tradier.Model;

namespace Tradier.Tests.Models;

[TestClass]
public class BalanceTests
{
    [TestMethod]
    public void Balance_WithValidData_ShouldBeValid()
    {
        // Arrange
        var balance = new Balance
        {
            AccountNumber = "TEST12345",
            AccountType = AccountType.Margin,
            TotalEquity = 100000.50m,
            TotalCash = 50000.25m,
            LongMarketValue = 75000.00m,
            OptionShortValue = 5000.00m,
            OptionLongValue = 10000.00m,
            StockLongValue = 65000.00m,
            OpenProfitLoss = 2500.75m,
            ClosedProfitLoss = -1000.50m,
            UnclearedFunds = 1000.00m,
            PendingCash = 500.00m,
            Equity = 100000.50m,
            CurrentRequirement = 15000.00m,
            OptionRequirement = 5000.00m,
            PendingOrdersCount = 3
        };

        // Act & Assert
        balance.IsValid.Should().BeTrue();
        balance.TotalProfitLoss.Should().Be(1500.25m);
        balance.AvailableCash.Should().Be(48500.25m); // 50000.25 - 1000 - 500
        balance.MeetsPatternDayTradingRequirement.Should().BeTrue(); // >= 25000
    }

    [TestMethod]
    public void Balance_WithNegativeValues_ShouldBeInvalid()
    {
        // Arrange
        var balance = new Balance
        {
            AccountNumber = "TEST12345",
            TotalEquity = -1000m, // Invalid negative value
            OptionShortValue = -500m, // Invalid negative value
            PendingOrdersCount = -1 // Invalid negative count
        };

        // Act
        var validationResults = balance.Validate().ToList();

        // Assert
        balance.IsValid.Should().BeFalse();
        validationResults.Should().HaveCountGreaterThan(0);
    }

    [TestMethod]
    public void Balance_WithInvalidAccountNumber_ShouldBeInvalid()
    {
        // Arrange
        var balance = new Balance
        {
            AccountNumber = "123", // Too short
            TotalEquity = 50000m
        };

        // Act
        var validationResults = balance.Validate().ToList();

        // Assert
        balance.IsValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("8-12 alphanumeric characters"));
    }

    [TestMethod]
    public void Balance_PatternDayTradingRequirement_ShouldCalculateCorrectly()
    {
        // Arrange
        var richAccount = new Balance { TotalEquity = 30000m };
        var poorAccount = new Balance { TotalEquity = 20000m };

        // Act & Assert
        richAccount.MeetsPatternDayTradingRequirement.Should().BeTrue();
        poorAccount.MeetsPatternDayTradingRequirement.Should().BeFalse();
    }

    [TestMethod]
    public void Balance_CalculatedProperties_ShouldReturnCorrectValues()
    {
        // Arrange
        var balance = new Balance
        {
            OpenProfitLoss = 1500.50m,
            ClosedProfitLoss = -500.25m,
            TotalCash = 10000m,
            UnclearedFunds = 1000m,
            PendingCash = 500m
        };

        // Act & Assert
        balance.TotalProfitLoss.Should().Be(1000.25m);
        balance.AvailableCash.Should().Be(8500m);
    }

    [TestMethod]
    public void Balance_JsonSerialization_ShouldWorkCorrectly()
    {
        // Arrange
        var balance = new Balance
        {
            AccountNumber = "TEST12345",
            AccountType = AccountType.Cash,
            TotalEquity = 50000.75m,
            TotalCash = 25000.50m,
            PendingOrdersCount = 2
        };

        var expectedJson = @"{
            ""account_number"": ""TEST12345"",
            ""account_type"": ""cash"",
            ""total_equity"": 50000.75,
            ""total_cash"": 25000.50,
            ""pending_orders_count"": 2,
            ""option_short_value"": 0,
            ""close_pl"": 0,
            ""current_requirement"": 0,
            ""equity"": 0,
            ""long_market_value"": 0,
            ""market_value"": 0,
            ""open_pl"": 0,
            ""option_long_value"": 0,
            ""option_requirement"": 0,
            ""short_market_value"": 0,
            ""stock_long_value"": 0,
            ""uncleared_funds"": 0,
            ""pending_cash"": 0
        }";

        // Act
        var json = JsonSerializer.Serialize(balance);
        var deserializedBalance = JsonSerializer.Deserialize<Balance>(expectedJson);

        // Assert
        json.Should().NotBeNullOrEmpty();
        deserializedBalance.Should().NotBeNull();
        deserializedBalance!.AccountNumber.Should().Be("TEST12345");
        deserializedBalance.AccountType.Should().Be(AccountType.Cash);
        deserializedBalance.TotalEquity.Should().Be(50000.75m);
        deserializedBalance.TotalCash.Should().Be(25000.50m);
        deserializedBalance.PendingOrdersCount.Should().Be(2);
    }

    [TestMethod]
    public void Balance_ThrowIfInvalid_ShouldThrowForInvalidData()
    {
        // Arrange
        var invalidBalance = new Balance(); // Missing required AccountNumber

        // Act & Assert
        invalidBalance.Invoking(b => b.ThrowIfInvalid())
            .Should().Throw<ValidationException>();
    }
}