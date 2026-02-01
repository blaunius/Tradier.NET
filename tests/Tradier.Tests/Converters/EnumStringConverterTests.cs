using System.Text.Json;
using FluentAssertions;
using Tradier.Converters;
using Tradier.Enumerations;

namespace Tradier.Tests.Converters;

[TestClass]
public class EnumStringConverterTests
{
    private readonly JsonSerializerOptions _options;

    public EnumStringConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new EnumStringConverter<AccountStatus>());
        _options.Converters.Add(new EnumStringConverter<OrderType>());
        _options.Converters.Add(new EnumStringConverter<SecurityType>());
        _options.Converters.Add(new EnumStringConverter<OptionType>());
    }

    [TestMethod]
    public void EnumStringConverter_Serialize_ShouldConvertEnumToString()
    {
        // Arrange
        var testObject = new { Status = AccountStatus.Active, Type = OrderType.Limit };

        // Act
        var json = JsonSerializer.Serialize(testObject, _options);

        // Assert
        json.Should().Contain("\"Status\":\"active\"");
        json.Should().Contain("\"Type\":\"limit\"");
    }

    [TestMethod]
    public void EnumStringConverter_Deserialize_ShouldConvertStringToEnum()
    {
        // Arrange
        var json = @"{
            ""Status"": ""active"",
            ""Type"": ""limit"",
            ""SecurityType"": ""stock"",
            ""OptionType"": ""call""
        }";

        // Act
        var result = JsonSerializer.Deserialize<TestEnumObject>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(AccountStatus.Active);
        result.Type.Should().Be(OrderType.Limit);
        result.SecurityType.Should().Be(SecurityType.Stock);
        result.OptionType.Should().Be(OptionType.Call);
    }

    [TestMethod]
    public void EnumStringConverter_DeserializeCaseInsensitive_ShouldWork()
    {
        // Arrange
        var json = @"{
            ""Status"": ""ACTIVE"",
            ""Type"": ""Limit"",
            ""SecurityType"": ""STOCK""
        }";

        // Act
        var result = JsonSerializer.Deserialize<TestEnumObject>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(AccountStatus.Active);
        result.Type.Should().Be(OrderType.Limit);
        result.SecurityType.Should().Be(SecurityType.Stock);
    }

    [TestMethod]
    public void EnumStringConverter_DeserializeInvalidValue_ShouldReturnDefault()
    {
        // Arrange
        var json = @"{
            ""Status"": ""invalid_status"",
            ""Type"": ""unknown_type""
        }";

        // Act
        var result = JsonSerializer.Deserialize<TestEnumObject>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(AccountStatus.Unknown); // Default enum value
        result.Type.Should().Be(OrderType.Unknown); // Default enum value
    }

    [TestMethod]
    public void EnumStringConverter_SerializeUnknownEnum_ShouldUseEnumName()
    {
        // Arrange
        var testObject = new { Status = AccountStatus.Unknown, Type = OrderType.Unknown };

        // Act
        var json = JsonSerializer.Serialize(testObject, _options);

        // Assert
        json.Should().Contain("\"Status\":\"unknown\"");
        json.Should().Contain("\"Type\":\"unknown\"");
    }

    [TestMethod]
    public void EnumStringConverter_RoundTrip_ShouldPreserveValues()
    {
        // Arrange
        var original = new TestEnumObject
        {
            Status = AccountStatus.Suspended,
            Type = OrderType.StopLimit,
            SecurityType = SecurityType.Option,
            OptionType = OptionType.Put
        };

        // Act
        var json = JsonSerializer.Serialize(original, _options);
        var deserialized = JsonSerializer.Deserialize<TestEnumObject>(json, _options);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Status.Should().Be(original.Status);
        deserialized.Type.Should().Be(original.Type);
        deserialized.SecurityType.Should().Be(original.SecurityType);
        deserialized.OptionType.Should().Be(original.OptionType);
    }

    private class TestEnumObject
    {
        public AccountStatus Status { get; set; }
        public OrderType Type { get; set; }
        public SecurityType SecurityType { get; set; }
        public OptionType OptionType { get; set; }
    }
}