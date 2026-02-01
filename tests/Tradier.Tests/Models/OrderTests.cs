using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FluentAssertions;
using Tradier.Enumerations;
using Tradier.Model;

namespace Tradier.Tests.Models;

[TestClass]
public class OrderTests
{
    [TestMethod]
    public void Order_WithValidData_ShouldBeValid()
    {
        // Arrange
        var order = new Order
        {
            Id = 123456789,
            Type = OrderType.Limit,
            Symbol = "AAPL",
            Side = OrderSide.Buy,
            Quantity = 100m,
            Status = OrderStatus.Open,
            Duration = OrderDuration.Day,
            Price = 150.00m,
            ExecutedQuantity = 50m,
            AverageFillPrice = 149.95m,
            RemainingQuantity = 50m,
            CreateDate = DateTime.UtcNow.AddHours(-1),
            TransactionDate = DateTime.UtcNow,
            Class = OrderClass.Equity
        };

        // Act & Assert
        order.IsValid.Should().BeTrue();
        order.IsActive.Should().BeTrue();
        order.IsCompleted.Should().BeFalse();
        order.IsTerminated.Should().BeFalse();
        order.FillPercentage.Should().Be(50m);
        order.IsBuyOrder.Should().BeTrue();
        order.IsSellOrder.Should().BeFalse();
        order.IsLimitOrder.Should().BeTrue();
        order.IsMarketOrder.Should().BeFalse();
        order.ExecutedValue.Should().Be(7497.50m); // 50 * 149.95
    }

    [TestMethod]
    public void Order_WithInvalidId_ShouldBeInvalid()
    {
        // Arrange
        var order = new Order
        {
            Id = 0, // Invalid ID
            Symbol = "AAPL",
            Quantity = 100m,
            CreateDate = DateTime.UtcNow
        };

        // Act
        var validationResults = order.Validate().ToList();

        // Assert
        order.IsValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("positive number"));
    }

    [TestMethod]
    public void Order_WithInvalidSymbol_ShouldBeInvalid()
    {
        // Arrange
        var order = new Order
        {
            Id = 123456,
            Symbol = "invalid symbol!", // Invalid characters
            Quantity = 100m,
            CreateDate = DateTime.UtcNow
        };

        // Act
        var validationResults = order.Validate().ToList();

        // Assert
        order.IsValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("uppercase letters, numbers, dots, and underscores"));
    }

    [TestMethod]
    public void Order_WithInvalidQuantity_ShouldBeInvalid()
    {
        // Arrange
        var order = new Order
        {
            Id = 123456,
            Symbol = "AAPL",
            Quantity = 0m, // Invalid quantity
            CreateDate = DateTime.UtcNow
        };

        // Act
        var validationResults = order.Validate().ToList();

        // Assert
        order.IsValid.Should().BeFalse();
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("greater than 0"));
    }

    [TestMethod]
    public void Order_StatusProperties_ShouldReturnCorrectValues()
    {
        // Arrange
        var openOrder = new Order { Status = OrderStatus.Open };
        var partiallyFilledOrder = new Order { Status = OrderStatus.PartiallyFilled };
        var filledOrder = new Order { Status = OrderStatus.Filled };
        var canceledOrder = new Order { Status = OrderStatus.Canceled };
        var rejectedOrder = new Order { Status = OrderStatus.Rejected };
        var expiredOrder = new Order { Status = OrderStatus.Expired };

        // Act & Assert
        openOrder.IsActive.Should().BeTrue();
        partiallyFilledOrder.IsActive.Should().BeTrue();
        filledOrder.IsActive.Should().BeFalse();

        filledOrder.IsCompleted.Should().BeTrue();
        openOrder.IsCompleted.Should().BeFalse();

        canceledOrder.IsTerminated.Should().BeTrue();
        rejectedOrder.IsTerminated.Should().BeTrue();
        expiredOrder.IsTerminated.Should().BeTrue();
        openOrder.IsTerminated.Should().BeFalse();
    }

    [TestMethod]
    public void Order_SideProperties_ShouldReturnCorrectValues()
    {
        // Arrange
        var buyOrder = new Order { Side = OrderSide.Buy };
        var buyToOpenOrder = new Order { Side = OrderSide.BuyToOpen };
        var buyToCloseOrder = new Order { Side = OrderSide.BuyToClose };
        var sellOrder = new Order { Side = OrderSide.Sell };
        var sellToOpenOrder = new Order { Side = OrderSide.SellToOpen };
        var sellToCloseOrder = new Order { Side = OrderSide.SellToClose };

        // Act & Assert
        buyOrder.IsBuyOrder.Should().BeTrue();
        buyToOpenOrder.IsBuyOrder.Should().BeTrue();
        buyToCloseOrder.IsBuyOrder.Should().BeTrue();
        sellOrder.IsBuyOrder.Should().BeFalse();

        sellOrder.IsSellOrder.Should().BeTrue();
        sellToOpenOrder.IsSellOrder.Should().BeTrue();
        sellToCloseOrder.IsSellOrder.Should().BeTrue();
        buyOrder.IsSellOrder.Should().BeFalse();
    }

    [TestMethod]
    public void Order_TypeProperties_ShouldReturnCorrectValues()
    {
        // Arrange
        var marketOrder = new Order { Type = OrderType.Market };
        var limitOrder = new Order { Type = OrderType.Limit };
        var stopOrder = new Order { Type = OrderType.Stop };
        var stopLimitOrder = new Order { Type = OrderType.StopLimit };

        // Act & Assert
        marketOrder.IsMarketOrder.Should().BeTrue();
        limitOrder.IsMarketOrder.Should().BeFalse();

        limitOrder.IsLimitOrder.Should().BeTrue();
        stopLimitOrder.IsLimitOrder.Should().BeTrue();
        marketOrder.IsLimitOrder.Should().BeFalse();
        stopOrder.IsLimitOrder.Should().BeFalse();
    }

    [TestMethod]
    public void Order_FillPercentageCalculation_ShouldBeCorrect()
    {
        // Arrange
        var order = new Order
        {
            Quantity = 100m,
            ExecutedQuantity = 75m
        };

        var emptyOrder = new Order
        {
            Quantity = 0m,
            ExecutedQuantity = 0m
        };

        // Act & Assert
        order.FillPercentage.Should().Be(75m);
        emptyOrder.FillPercentage.Should().Be(0m);
    }

    [TestMethod]
    public void Order_ExecutedValueCalculation_ShouldBeCorrect()
    {
        // Arrange
        var orderWithFills = new Order
        {
            ExecutedQuantity = 100m,
            AverageFillPrice = 50.25m
        };

        var orderWithoutFills = new Order
        {
            ExecutedQuantity = 0m,
            AverageFillPrice = 50.25m
        };

        var orderWithoutPrice = new Order
        {
            ExecutedQuantity = 100m,
            AverageFillPrice = null
        };

        // Act & Assert
        orderWithFills.ExecutedValue.Should().Be(5025m);
        orderWithoutFills.ExecutedValue.Should().BeNull();
        orderWithoutPrice.ExecutedValue.Should().BeNull();
    }

    [TestMethod]
    public void Order_JsonSerialization_ShouldWorkCorrectly()
    {
        // Arrange
        var order = new Order
        {
            Id = 123456789,
            Type = OrderType.Market,
            Symbol = "AAPL",
            Side = OrderSide.Buy,
            Quantity = 100m,
            Status = OrderStatus.Filled,
            Duration = OrderDuration.Day,
            ExecutedQuantity = 100m,
            AverageFillPrice = 150.50m,
            CreateDate = new DateTime(2024, 1, 1, 10, 0, 0, DateTimeKind.Utc),
            Class = OrderClass.Equity
        };

        // Act
        var json = JsonSerializer.Serialize(order);
        var deserializedOrder = JsonSerializer.Deserialize<Order>(json);

        // Assert
        json.Should().NotBeNullOrEmpty();
        deserializedOrder.Should().NotBeNull();
        deserializedOrder!.Id.Should().Be(123456789);
        deserializedOrder.Type.Should().Be(OrderType.Market);
        deserializedOrder.Symbol.Should().Be("AAPL");
        deserializedOrder.Side.Should().Be(OrderSide.Buy);
        deserializedOrder.Quantity.Should().Be(100m);
        deserializedOrder.Status.Should().Be(OrderStatus.Filled);
        deserializedOrder.Duration.Should().Be(OrderDuration.Day);
        deserializedOrder.Class.Should().Be(OrderClass.Equity);
    }

    [TestMethod]
    public void Order_ThrowIfInvalid_ShouldThrowForInvalidData()
    {
        // Arrange
        var invalidOrder = new Order(); // Missing required fields

        // Act & Assert
        invalidOrder.Invoking(o => o.ThrowIfInvalid())
            .Should().Throw<ValidationException>();
    }
}