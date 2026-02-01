using System.Text.Json.Serialization;

namespace Tradier.Response
{
    public class AccountProfileResponse : TradierResponse
    {
        [JsonPropertyName("profile")]
        public Model.Profile? Profile { get; set; }
        internal override void Deserialize()
        {
            this.Profile = System.Text.Json.JsonSerializer.Deserialize<AccountProfileResponse>(this.RawResponse)?.Profile ?? new();
        }
    }
}
