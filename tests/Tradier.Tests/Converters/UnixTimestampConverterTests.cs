using System.Text.Json;
using FluentAssertions;
using Tradier.Converters;

namespace Tradier.Tests.Converters;

[TestClass]
public class UnixTimestampConverterTests
{
    private readonly JsonSerializerOptions _options;

    public UnixTimestampConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new UnixTimestampConverter());
    }

    [TestMethod]
    public void UnixTimestampConverter_SerializeDateTime_ShouldConvertToUnixTimestamp()
    {
        // Arrange
        var dateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var expectedTimestamp = ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();
        var testObject = new { Timestamp = (DateTime?)dateTime };

        // Act
        var json = JsonSerializer.Serialize(testObject, _options);

        // Assert
        json.Should().Contain($"\"Timestamp\":{expectedTimestamp}");
    }

    [TestMethod]
    public void UnixTimestampConverter_SerializeNull_ShouldWriteNull()
    {
        // Arrange
        var testObject = new { Timestamp = (DateTime?)null };

        // Act
        var json = JsonSerializer.Serialize(testObject, _options);

        // Assert
        json.Should().Contain("\"Timestamp\":null");
    }

    [TestMethod]
    public void UnixTimestampConverter_DeserializeMillisecondsTimestamp_ShouldConvertToDateTime()
    {
        // Arrange
        var expectedDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var timestamp = ((DateTimeOffset)expectedDateTime).ToUnixTimeMilliseconds();
        var json = $@"{{ ""Timestamp"": {timestamp} }}";

        // Act
        var result = JsonSerializer.Deserialize<TestTimestampObject>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Timestamp.Should().BeCloseTo(expectedDateTime, TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    public void UnixTimestampConverter_DeserializeSecondsTimestamp_ShouldConvertToDateTime()
    {
        // Arrange
        var expectedDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var timestamp = ((DateTimeOffset)expectedDateTime).ToUnixTimeSeconds();
        var json = $@"{{ ""Timestamp"": {timestamp} }}";

        // Act
        var result = JsonSerializer.Deserialize<TestTimestampObject>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Timestamp.Should().BeCloseTo(expectedDateTime, TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    public void UnixTimestampConverter_DeserializeStringTimestamp_ShouldConvertToDateTime()
    {
        // Arrange
        var expectedDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var timestamp = ((DateTimeOffset)expectedDateTime).ToUnixTimeMilliseconds();
        var json = $@"{{ ""Timestamp"": ""{timestamp}"" }}";

        // Act
        var result = JsonSerializer.Deserialize<TestTimestampObject>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Timestamp.Should().BeCloseTo(expectedDateTime, TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    public void UnixTimestampConverter_DeserializeZeroTimestamp_ShouldReturnNull()
    {
        // Arrange
        var json = @"{ ""Timestamp"": 0 }";

        // Act
        var result = JsonSerializer.Deserialize<TestTimestampObject>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Timestamp.Should().BeNull();
    }

    [TestMethod]
    public void UnixTimestampConverter_DeserializeNull_ShouldReturnNull()
    {
        // Arrange
        var json = @"{ ""Timestamp"": null }";

        // Act
        var result = JsonSerializer.Deserialize<TestTimestampObject>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Timestamp.Should().BeNull();
    }

    [TestMethod]
    public void UnixTimestampConverter_DeserializeInvalidString_ShouldReturnNull()
    {
        // Arrange
        var json = @"{ ""Timestamp"": ""invalid"" }";

        // Act
        var result = JsonSerializer.Deserialize<TestTimestampObject>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Timestamp.Should().BeNull();
    }

    [TestMethod]
    public void UnixTimestampConverter_RoundTrip_ShouldPreserveDateTime()
    {
        // Arrange
        var originalDateTime = new DateTime(2024, 6, 15, 14, 30, 45, DateTimeKind.Utc);
        var original = new TestTimestampObject { Timestamp = originalDateTime };

        // Act
        var json = JsonSerializer.Serialize(original, _options);
        var deserialized = JsonSerializer.Deserialize<TestTimestampObject>(json, _options);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Timestamp.Should().BeCloseTo(originalDateTime, TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    public void UnixTimestampConverter_HandlesDifferentTimestampFormats()
    {
        // Test various timestamp formats that might come from the Tradier API
        var testCases = new[]
        {
            new { Json = @"{ ""Timestamp"": 1704110400000 }", Expected = (DateTime?)new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc) },
            new { Json = @"{ ""Timestamp"": 1704110400 }", Expected = (DateTime?)new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc) },
            new { Json = @"{ ""Timestamp"": ""1704110400000"" }", Expected = (DateTime?)new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc) },
            new { Json = @"{ ""Timestamp"": ""0"" }", Expected = (DateTime?)null },
            new { Json = @"{ ""Timestamp"": 0 }", Expected = (DateTime?)null }
        };

        foreach (var testCase in testCases)
        {
            // Act
            var result = JsonSerializer.Deserialize<TestTimestampObject>(testCase.Json, _options);

            // Assert
            result.Should().NotBeNull();
            if (testCase.Expected.HasValue)
            {
                result!.Timestamp.Should().BeCloseTo(testCase.Expected.Value, TimeSpan.FromSeconds(1));
            }
            else
            {
                result!.Timestamp.Should().BeNull();
            }
        }
    }

    private class TestTimestampObject
    {
        public DateTime? Timestamp { get; set; }
    }
}