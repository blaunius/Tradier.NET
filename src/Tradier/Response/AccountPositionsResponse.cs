using System.Text.Json.Serialization;
namespace Tradier.Response
{
    public class AccountPositionsResponse : TradierResponse
    {
        [JsonPropertyName("positions")]
        private AccountPositionsContainer? Data { get; set; }
        public class AccountPositionsContainer
        {
            [JsonPropertyName("position")]
            public List<Model.Position> Positions { get; set; } = new();
        }
        internal override void Deserialize()
        {
            this.Data = System.Text.Json.JsonSerializer.Deserialize<AccountPositionsResponse>(this.RawResponse)?.Data ?? new();            
        }
    }
}
