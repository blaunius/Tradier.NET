using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tradier.Converters
{
    /// <summary>
    /// JSON converter for Unix timestamp values (milliseconds since epoch) to DateTime objects.
    /// </summary>
    public class UnixTimestampConverter : JsonConverter<DateTime?>
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var timestamp = reader.GetInt64();
                if (timestamp == 0) return null;
                
                // Handle both seconds and milliseconds timestamps
                var dateTime = timestamp > 9999999999 
                    ? UnixEpoch.AddMilliseconds(timestamp) 
                    : UnixEpoch.AddSeconds(timestamp);
                
                return dateTime;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (long.TryParse(stringValue, out var timestamp))
                {
                    if (timestamp == 0) return null;
                    
                    var dateTime = timestamp > 9999999999 
                        ? UnixEpoch.AddMilliseconds(timestamp) 
                        : UnixEpoch.AddSeconds(timestamp);
                    
                    return dateTime;
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                var timestamp = (long)(value.Value.ToUniversalTime() - UnixEpoch).TotalMilliseconds;
                writer.WriteNumberValue(timestamp);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}