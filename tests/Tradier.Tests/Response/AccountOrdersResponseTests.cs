using System.Net;
using System.Text;
using FluentAssertions;
using Tradier.Response;
using Tradier.Enumerations;

namespace Tradier.Tests.Response;

[TestClass]
public class AccountOrdersResponseTests
{
    [TestMethod]
    public async Task ParseAsync_WithMultipleOrders_ShouldParseCorrectly()
    {
        // Arrange
        var json = @"{
            ""orders"": {
                ""order"": [
                    {
                        ""id"": 123456789,
                        ""type"": ""market"",
                        ""symbol"": ""AAPL"",
                        ""side"": ""buy"",
                        ""quantity"": 100,
                        ""status"": ""filled"",
                        ""duration"": ""day"",
                        ""price"": 150.00,
                        ""create_date"": ""2024-01-01T10:00:00Z"",
                        ""class"": ""equity""
                    },
                    {
                        ""id"": 123456790,
                        ""type"": ""limit"",
                        ""symbol"": ""MSFT"",
                        ""side"": ""sell"",
                        ""quantity"": 50,
                        ""status"": ""open"",
                        ""duration"": ""gtc"",
                        ""price"": 300.00,
                        ""create_date"": ""2024-01-01T11:00:00Z"",
                        ""class"": ""equity""
                    }
                ]
            }
        }";

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new AccountOrdersResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue();
        response.IsSuccessful.Should().BeTrue();
        response.HasOrders.Should().BeTrue();
        response.OrderCount.Should().Be(2);
        
        var orders = response.Orders;
        orders.Should().HaveCount(2);
        
        var firstOrder = orders[0];
        firstOrder.Id.Should().Be(123456789);
        firstOrder.Symbol.Should().Be("AAPL");
        firstOrder.Type.Should().Be(OrderType.Market);
        firstOrder.Side.Should().Be(OrderSide.Buy);
        firstOrder.Quantity.Should().Be(100m);
        firstOrder.Status.Should().Be(OrderStatus.Filled);
        
        var secondOrder = orders[1];
        secondOrder.Id.Should().Be(123456790);
        secondOrder.Symbol.Should().Be("MSFT");
        secondOrder.Type.Should().Be(OrderType.Limit);
        secondOrder.Side.Should().Be(OrderSide.Sell);
    }

    [TestMethod]
    public async Task ParseAsync_WithSingleOrder_ShouldParseCorrectly()
    {
        // Arrange
        var json = @"{
            ""orders"": {
                ""order"": {
                    ""id"": 123456789,
                    ""type"": ""limit"",
                    ""symbol"": ""GOOGL"",
                    ""side"": ""buy"",
                    ""quantity"": 25,
                    ""status"": ""partially_filled"",
                    ""duration"": ""day"",
                    ""price"": 2500.00,
                    ""exec_quantity"": 10,
                    ""avg_fill_price"": 2495.50,
                    ""create_date"": ""2024-01-01T09:30:00Z"",
                    ""class"": ""equity""
                }
            }
        }";

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new AccountOrdersResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue();
        response.HasOrders.Should().BeTrue();
        response.OrderCount.Should().Be(1);
        
        var order = response.Orders.First();
        order.Id.Should().Be(123456789);
        order.Symbol.Should().Be("GOOGL");
        order.Status.Should().Be(OrderStatus.PartiallyFilled);
        order.ExecutedQuantity.Should().Be(10m);
        order.AverageFillPrice.Should().Be(2495.50m);
        order.IsActive.Should().BeTrue(); // Partially filled is still active
        order.FillPercentage.Should().Be(40m); // 10/25 = 40%
    }

    [TestMethod]
    public async Task ParseAsync_WithNullOrders_ShouldCreateEmptyList()
    {
        // Arrange
        var json = @"{""orders"":""null""}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new AccountOrdersResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue();
        response.IsSuccessful.Should().BeTrue();
        response.HasOrders.Should().BeFalse();
        response.OrderCount.Should().Be(0);
        response.Orders.Should().NotBeNull();
        response.Orders.Should().BeEmpty();
    }

    [TestMethod]
    public async Task ParseAsync_WithEmptyOrdersObject_ShouldCreateEmptyList()
    {
        // Arrange
        var json = @"{""orders"":{}}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new AccountOrdersResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue();
        response.HasOrders.Should().BeFalse();
        response.OrderCount.Should().Be(0);
        response.Orders.Should().BeEmpty();
    }

    [TestMethod]
    public async Task ParseAsync_WithErrorResponse_ShouldHandleError()
    {
        // Arrange
        var errorJson = @"{""fault"":{""faultstring"":\""Access denied\""}}";
        var content = new StringContent(errorJson, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = content };

        var response = new AccountOrdersResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeFalse();
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        response.HasOrders.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Access denied");
    }
}