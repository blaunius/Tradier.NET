using System.Text.Json.Serialization;

namespace Tradier.Model
{
    public class WatchList
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("public_id")]
        public string? PublicId { get; set; }

        [JsonPropertyName("items")]
        public WatchListItemContainer? Items { get; set; }
    }

    public class WatchListItemContainer
    {
        [JsonPropertyName("item")]
        public List<WatchListItem> Item { get; set; } = new();
    }

    public class WatchListItem
    {
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}
