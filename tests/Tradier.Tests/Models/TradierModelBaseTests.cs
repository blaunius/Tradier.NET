using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Tradier.Model;

namespace Tradier.Tests.Models;

[TestClass]
public class TradierModelBaseTests
{
    private class TestModel : TradierModelBase
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(10, ErrorMessage = "Name cannot exceed 10 characters")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100")]
        public int Value { get; set; }
    }

    [TestMethod]
    public void Validate_WithValidModel_ReturnsEmptyResults()
    {
        // Arrange
        var model = new TestModel { Name = "Test", Value = 50 };

        // Act
        var results = model.Validate();

        // Assert
        results.Should().BeEmpty();
        model.IsValid.Should().BeTrue();
        model.ValidationError.Should().BeNull();
    }

    [TestMethod]
    public void Validate_WithInvalidModel_ReturnsValidationErrors()
    {
        // Arrange
        var model = new TestModel { Name = "", Value = 150 };

        // Act
        var results = model.Validate().ToList();

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain(r => r.ErrorMessage == "Name is required");
        results.Should().Contain(r => r.ErrorMessage == "Value must be between 0 and 100");
        
        model.IsValid.Should().BeFalse();
        model.ValidationError.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public void ThrowIfInvalid_WithValidModel_DoesNotThrow()
    {
        // Arrange
        var model = new TestModel { Name = "Test", Value = 50 };

        // Act & Assert
        model.Invoking(m => m.ThrowIfInvalid()).Should().NotThrow();
    }

    [TestMethod]
    public void ThrowIfInvalid_WithInvalidModel_ThrowsValidationException()
    {
        // Arrange
        var model = new TestModel { Name = "ThisNameIsTooLong", Value = 150 };

        // Act & Assert
        model.Invoking(m => m.ThrowIfInvalid())
            .Should().Throw<ValidationException>()
            .WithMessage("Model validation failed:*");
    }
}