using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tradier.Model
{
    /// <summary>
    /// Base class for all Tradier API model objects, providing common functionality and validation.
    /// </summary>
    public abstract class TradierModelBase
    {
        /// <summary>
        /// Validates the model and returns any validation errors.
        /// </summary>
        /// <returns>Collection of validation results, empty if valid.</returns>
        public virtual IEnumerable<ValidationResult> Validate()
        {
            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, context, results, validateAllProperties: true);
            return results;
        }

        /// <summary>
        /// Gets a value indicating whether this model instance is valid.
        /// </summary>
        [JsonIgnore]
        public bool IsValid => !Validate().Any();

        /// <summary>
        /// Gets the first validation error message, if any.
        /// </summary>
        [JsonIgnore]
        public string? ValidationError => Validate().FirstOrDefault()?.ErrorMessage;

        /// <summary>
        /// Throws a ValidationException if the model is invalid.
        /// </summary>
        /// <exception cref="ValidationException">Thrown when the model fails validation.</exception>
        public virtual void ThrowIfInvalid()
        {
            var validationResults = Validate().ToList();
            if (validationResults.Count > 0)
            {
                var errorMessage = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ValidationException($"Model validation failed: {errorMessage}");
            }
        }
    }
}