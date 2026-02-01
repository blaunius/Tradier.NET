using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FluentAssertions;
using Tradier.Enumerations;
using Tradier.Model;

namespace Tradier.Tests.Models;

[TestClass]
public class AccountTests
{
    [TestMethod]
    public void Account_WithValidData_ShouldBeValid()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "ABC12345",
            Classification = AccountClassification.Individual,
            DateCreated = DateTime.UtcNow,
            IsDayTrader = false,
            OptionLevel = 2,
            Status = AccountStatus.Active,
            Type = AccountType.Margin,
            LastUpdateDate = DateTime.UtcNow
        };

        // Act & Assert
        account.IsValid.Should().BeTrue();
        account.IsActive.Should().BeTrue();
        account.SupportsOptions.Should().BeTrue();
        account.SupportsMargin.Should().BeTrue();
    }

    [TestMethod]
    public void Account_WithInvalidAccountNumber_ShouldBeInvalid()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "123", // Too short
            DateCreated = DateTime.UtcNow
        };

        // Act
        var validationResults = account.Validate().ToList();

        // Assert
        account.IsValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("8-12 alphanumeric characters"));
    }

    [TestMethod]
    public void Account_WithInvalidOptionLevel_ShouldBeInvalid()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "ABC12345",
            DateCreated = DateTime.UtcNow,
            OptionLevel = 10 // Invalid range
        };

        // Act
        var validationResults = account.Validate().ToList();

        // Assert
        account.IsValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("between 0 and 5"));
    }

    [TestMethod]
    public void Account_Properties_ShouldReturnCorrectValues()
    {
        // Arrange & Act
        var activeAccount = new Account { Status = AccountStatus.Active };
        var inactiveAccount = new Account { Status = AccountStatus.Closed };
        var optionsAccount = new Account { OptionLevel = 3 };
        var noOptionsAccount = new Account { OptionLevel = 0 };
        var marginAccount = new Account { Type = AccountType.Margin };
        var cashAccount = new Account { Type = AccountType.Cash };

        // Assert
        activeAccount.IsActive.Should().BeTrue();
        inactiveAccount.IsActive.Should().BeFalse();
        optionsAccount.SupportsOptions.Should().BeTrue();
        noOptionsAccount.SupportsOptions.Should().BeFalse();
        marginAccount.SupportsMargin.Should().BeTrue();
        cashAccount.SupportsMargin.Should().BeFalse();
    }

    [TestMethod]
    public void Account_JsonSerialization_ShouldWorkCorrectly()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "TEST12345",
            Classification = AccountClassification.Individual,
            DateCreated = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            IsDayTrader = true,
            OptionLevel = 4,
            Status = AccountStatus.Active,
            Type = AccountType.Margin,
            LastUpdateDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        var expectedJson = @"{
            ""account_number"": ""TEST12345"",
            ""classification"": ""individual"",
            ""date_created"": ""2023-01-01T00:00:00Z"",
            ""day_trader"": true,
            ""option_level"": 4,
            ""status"": ""active"",
            ""type"": ""margin"",
            ""last_update_date"": ""2024-01-01T00:00:00Z""
        }";

        // Act
        var json = JsonSerializer.Serialize(account);
        var deserializedAccount = JsonSerializer.Deserialize<Account>(expectedJson);

        // Assert
        json.Should().NotBeNullOrEmpty();
        deserializedAccount.Should().NotBeNull();
        deserializedAccount!.AccountNumber.Should().Be("TEST12345");
        deserializedAccount.Classification.Should().Be(AccountClassification.Individual);
        deserializedAccount.IsDayTrader.Should().BeTrue();
        deserializedAccount.OptionLevel.Should().Be(4);
        deserializedAccount.Status.Should().Be(AccountStatus.Active);
        deserializedAccount.Type.Should().Be(AccountType.Margin);
    }

    [TestMethod]
    public void Account_ThrowIfInvalid_ShouldThrowForInvalidData()
    {
        // Arrange
        var invalidAccount = new Account(); // Missing required fields

        // Act & Assert
        invalidAccount.Invoking(a => a.ThrowIfInvalid())
            .Should().Throw<ValidationException>();
    }
}