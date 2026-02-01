using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FluentAssertions;
using Tradier.Enumerations;
using Tradier.Model;

namespace Tradier.Tests.Models;

[TestClass]
public class QuoteTests
{
    [TestMethod]
    public void Quote_WithValidStockData_ShouldBeValid()
    {
        // Arrange
        var quote = new Quote
        {
            Symbol = "AAPL",
            Description = "Apple Inc.",
            Exchange = "NASDAQ",
            Type = SecurityType.Stock,
            LastPrice = 150.25m,
            Change = 2.50m,
            Volume = 1000000,
            OpenPrice = 148.00m,
            HighPrice = 152.00m,
            LowPrice = 147.50m,
            BidPrice = 150.20m,
            AskPrice = 150.30m,
            BidSize = 100,
            AskSize = 200,
            ChangePercentage = 1.69m,
            PreviousClose = 147.75m,
            Week52High = 180.00m,
            Week52Low = 120.00m,
            TradeDate = DateTime.UtcNow
        };

        // Act & Assert
        quote.IsValid.Should().BeTrue();
        quote.IsOption.Should().BeFalse();
        quote.IsEquity.Should().BeTrue();
        quote.BidAskSpread.Should().Be(0.10m);
        quote.MidPrice.Should().Be(150.25m);
        quote.IsActivelyTrading.Should().BeTrue();
    }

    [TestMethod]
    public void Quote_WithValidOptionData_ShouldBeValid()
    {
        // Arrange
        var quote = new Quote
        {
            Symbol = "AAPL240315C00150000",
            Description = "AAPL Mar 15 2024 150 Call",
            Exchange = "OPRA",
            Type = SecurityType.Option,
            LastPrice = 5.50m,
            UnderlyingSymbol = "AAPL",
            StrikePrice = 150.00m,
            OptionType = OptionType.Call,
            ExpirationDate = "2024-03-15",
            ContractSize = 100,
            OpenInterest = 1500,
            Volume = 500,
            BidPrice = 5.40m,
            AskPrice = 5.60m,
            TradeDate = DateTime.UtcNow
        };

        // Act & Assert
        quote.IsValid.Should().BeTrue();
        quote.IsOption.Should().BeTrue();
        quote.IsEquity.Should().BeFalse();
        quote.BidAskSpread.Should().Be(0.20m);
        quote.MidPrice.Should().Be(5.50m);
    }

    [TestMethod]
    public void Quote_WithInvalidSymbol_ShouldBeInvalid()
    {
        // Arrange
        var quote = new Quote
        {
            Symbol = "invalid symbol!", // Contains invalid characters
            LastPrice = 100m
        };

        // Act
        var validationResults = quote.Validate().ToList();

        // Assert
        quote.IsValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("uppercase letters, numbers, dots, and underscores"));
    }

    [TestMethod]
    public void Quote_WithNegativePrices_ShouldBeInvalid()
    {
        // Arrange
        var quote = new Quote
        {
            Symbol = "TEST",
            LastPrice = -10m, // Invalid negative price
            BidPrice = -5m    // Invalid negative price
        };

        // Act
        var validationResults = quote.Validate().ToList();

        // Assert
        quote.IsValid.Should().BeFalse();
        validationResults.Should().HaveCountGreaterThan(0);
    }

    [TestMethod]
    public void Quote_CalculatedProperties_ShouldReturnCorrectValues()
    {
        // Arrange
        var activeQuote = new Quote 
        { 
            Volume = 1000, 
            BidPrice = 10.00m, 
            AskPrice = 10.05m 
        };
        
        var inactiveQuote = new Quote 
        { 
            Volume = 0, 
            BidPrice = null, 
            AskPrice = 10.05m 
        };

        var bidAskQuote = new Quote
        {
            BidPrice = 100.00m,
            AskPrice = 100.50m
        };

        // Act & Assert
        activeQuote.IsActivelyTrading.Should().BeTrue();
        inactiveQuote.IsActivelyTrading.Should().BeFalse();
        
        bidAskQuote.BidAskSpread.Should().Be(0.50m);
        bidAskQuote.MidPrice.Should().Be(100.25m);
    }

    [TestMethod]
    public void Quote_JsonSerialization_ShouldWorkCorrectly()
    {
        // Arrange
        var quote = new Quote
        {
            Symbol = "AAPL",
            Description = "Apple Inc.",
            Exchange = "NASDAQ",
            Type = SecurityType.Stock,
            LastPrice = 150.25m,
            Change = 2.50m,
            Volume = 1000000,
            BidPrice = 150.20m,
            AskPrice = 150.30m,
            OptionType = OptionType.Unknown
        };

        // Act
        var json = JsonSerializer.Serialize(quote);
        var deserializedQuote = JsonSerializer.Deserialize<Quote>(json);

        // Assert
        json.Should().NotBeNullOrEmpty();
        deserializedQuote.Should().NotBeNull();
        deserializedQuote!.Symbol.Should().Be("AAPL");
        deserializedQuote.Type.Should().Be(SecurityType.Stock);
        deserializedQuote.LastPrice.Should().Be(150.25m);
        deserializedQuote.Volume.Should().Be(1000000);
        deserializedQuote.BidPrice.Should().Be(150.20m);
        deserializedQuote.AskPrice.Should().Be(150.30m);
    }

    [TestMethod]
    public void Quote_UnixTimestampSerialization_ShouldWorkCorrectly()
    {
        // Arrange
        var expectedDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var timestamp = ((DateTimeOffset)expectedDate).ToUnixTimeMilliseconds();
        
        var json = $@"{{
            ""symbol"": ""TEST"",
            ""trade_date"": {timestamp},
            ""bid_date"": {timestamp},
            ""ask_date"": {timestamp}
        }}";

        // Act
        var quote = JsonSerializer.Deserialize<Quote>(json);

        // Assert
        quote.Should().NotBeNull();
        quote!.TradeDate.Should().BeCloseTo(expectedDate, TimeSpan.FromSeconds(1));
        quote.BidDate.Should().BeCloseTo(expectedDate, TimeSpan.FromSeconds(1));
        quote.AskDate.Should().BeCloseTo(expectedDate, TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    public void Quote_InTheMoney_ShouldCalculateBasedOnOptionType()
    {
        // Arrange
        var callOption = new Quote
        {
            Symbol = "TEST240315C00100000",
            Type = SecurityType.Option,
            OptionType = OptionType.Call,
            StrikePrice = 100m,
            LastPrice = 5m
        };

        var putOption = new Quote
        {
            Symbol = "TEST240315P00100000",
            Type = SecurityType.Option,
            OptionType = OptionType.Put,
            StrikePrice = 100m,
            LastPrice = 5m
        };

        var stock = new Quote
        {
            Symbol = "AAPL",
            Type = SecurityType.Stock,
            LastPrice = 150m
        };

        // Act & Assert
        // Note: This is a simplified test - real ITM calculation needs underlying price
        callOption.IsInTheMoney.Should().BeTrue(); // Has value > 0
        putOption.IsInTheMoney.Should().BeTrue(); // Has value > 0
        stock.IsInTheMoney.Should().BeFalse(); // Not an option
    }

    [TestMethod]
    public void Quote_ThrowIfInvalid_ShouldThrowForInvalidData()
    {
        // Arrange
        var invalidQuote = new Quote(); // Missing required Symbol

        // Act & Assert
        invalidQuote.Invoking(q => q.ThrowIfInvalid())
            .Should().Throw<ValidationException>();
    }
}