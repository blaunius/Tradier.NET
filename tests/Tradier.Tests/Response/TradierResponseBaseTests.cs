using System.Net;
using System.Text;
using FluentAssertions;
using Tradier.Response;
using Tradier.Response.DataContracts;

namespace Tradier.Tests.Response;

[TestClass]
public class TradierResponseBaseTests
{
    private class TestDataContract
    {
        public string? TestProperty { get; set; }
        public int NumberProperty { get; set; }
    }

    private class TestResponse : TradierResponseBase<TestDataContract>
    {
        // Test implementation
    }

    [TestMethod]
    public async Task ParseAsync_WithSuccessfulResponse_ShouldParseCorrectly()
    {
        // Arrange
        var json = "{\"test_property\":\"test value\",\"number_property\":42}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new TestResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue();
        response.IsSuccessful.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data!.TestProperty.Should().Be("test value");
        response.Data.NumberProperty.Should().Be(42);
        response.HasData.Should().BeTrue();
        response.ErrorMessage.Should().BeNull();
    }

    [TestMethod]
    public async Task ParseAsync_WithErrorResponse_ShouldHandleError()
    {
        // Arrange
        var errorJson = "{\"fault\":{\"faultstring\":\"Invalid request\"}}";
        var content = new StringContent(errorJson, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = content };

        var response = new TestResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeFalse();
        response.IsSuccessful.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.HasData.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Invalid request");
    }

    [TestMethod]
    public async Task ParseAsync_WithInvalidJson_ShouldHandleJsonError()
    {
        // Arrange
        var invalidJson = "{invalid json content";
        var content = new StringContent(invalidJson, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new TestResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeFalse();
        response.IsSuccessful.Should().BeTrue(); // HTTP was successful
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeNull();
        response.ErrorMessage.Should().Contain("JSON deserialization failed");
    }

    [TestMethod]
    public async Task ParseAsync_WithEmptyResponse_ShouldHandleEmpty()
    {
        // Arrange
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new TestResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue(); // Empty response is acceptable
        response.IsSuccessful.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod]
    public async Task ParseAsync_WithNullResponse_ShouldReturnFalse()
    {
        // Arrange
        var response = new TestResponse();

        // Act
        var result = await response.ParseAsync(null!);

        // Assert
        result.Should().BeFalse();
        response.ErrorMessage.Should().Be("HTTP response is null");
    }

    [TestMethod]
    public async Task ParseAsync_WithHeaders_ShouldExtractHeaders()
    {
        // Arrange
        var json = "{\"TestProperty\":\"test\"}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        
        httpResponse.Headers.Add("X-RateLimit-Available", "100");
        httpResponse.Headers.Add("X-Custom-Header", "custom-value");

        var response = new TestResponse();

        // Act
        var result = await response.ParseAsync(httpResponse);

        // Assert
        result.Should().BeTrue();
        response.Headers.Should().ContainKey("RateLimit-Available");
        response.Headers["RateLimit-Available"].Should().Be("100");
        response.Headers.Should().ContainKey("X-Custom-Header");
        response.Headers["X-Custom-Header"].Should().Be("custom-value");
    }

    [TestMethod]
    public async Task ParseAsync_WithCancellation_ShouldHandleCancellation()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel(); // Cancel immediately

        var json = "{\"TestProperty\":\"test\"}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

        var response = new TestResponse();

        // Act
        var result = await response.ParseAsync(httpResponse, null, cts.Token);

        // Assert
        result.Should().BeFalse();
        response.ErrorMessage.Should().Contain("cancelled");
    }

    [TestMethod]
    public void ValidateData_WithValidData_ShouldReturnTrue()
    {
        // Arrange
        var response = new TestResponse();
        // Manually set data for validation test
        typeof(TradierResponseBase<TestDataContract>)
            .GetProperty("Data")!
            .SetValue(response, new TestDataContract { TestProperty = "valid", NumberProperty = 10 });

        // Act
        var isValid = response.ValidateData();

        // Assert
        isValid.Should().BeTrue();
        response.ValidationErrors.Should().BeNull();
    }

    [TestMethod]
    public void GetValidatedData_WithValidData_ShouldReturnData()
    {
        // Arrange
        var testData = new TestDataContract { TestProperty = "valid", NumberProperty = 10 };
        var response = new TestResponse();
        
        // Manually set data
        typeof(TradierResponseBase<TestDataContract>)
            .GetProperty("Data")!
            .SetValue(response, testData);

        // Act
        var validatedData = response.GetValidatedData();

        // Assert
        validatedData.Should().Be(testData);
        validatedData.TestProperty.Should().Be("valid");
        validatedData.NumberProperty.Should().Be(10);
    }

    [TestMethod]
    public void GetValidatedData_WithNoData_ShouldThrowException()
    {
        // Arrange
        var response = new TestResponse();

        // Act & Assert
        response.Invoking(r => r.GetValidatedData())
            .Should().Throw<InvalidOperationException>()
            .WithMessage("No data available in response*");
    }
}