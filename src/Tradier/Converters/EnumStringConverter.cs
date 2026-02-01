using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace Tradier.Converters
{
    /// <summary>
    /// JSON converter for enum types that handles string-to-enum conversion with case-insensitive matching.
    /// </summary>
    /// <typeparam name="T">The enum type to convert.</typeparam>
    public class EnumStringConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        private static readonly Dictionary<string, T> _stringToEnum;
        private static readonly Dictionary<T, string> _enumToString;

        static EnumStringConverter()
        {
            _stringToEnum = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            _enumToString = new Dictionary<T, string>();

            foreach (var enumValue in Enum.GetValues<T>())
            {
                var enumName = enumValue.ToString();
                var fieldInfo = typeof(T).GetField(enumName);
                
                // Check for JsonPropertyNameAttribute
                var jsonPropertyName = fieldInfo?.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name;
                var key = jsonPropertyName ?? enumName.ToLowerInvariant();
                
                _stringToEnum[key] = enumValue;
                _enumToString[enumValue] = jsonPropertyName ?? enumName.ToLowerInvariant();
            }
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (!string.IsNullOrEmpty(stringValue) && _stringToEnum.TryGetValue(stringValue, out var enumValue))
                {
                    return enumValue;
                }
            }
            
            // Return default value (typically Unknown) if conversion fails
            return default(T);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (_enumToString.TryGetValue(value, out var stringValue))
            {
                writer.WriteStringValue(stringValue);
            }
            else
            {
                writer.WriteStringValue(value.ToString().ToLowerInvariant());
            }
        }
    }
}