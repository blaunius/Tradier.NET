using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class Profile
    {
        [JsonPropertyName("account")]
        public Account? Account { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
