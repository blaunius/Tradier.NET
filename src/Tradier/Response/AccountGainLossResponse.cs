using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class AccountGainLossResponse : TradierResponse
    {
        [JsonPropertyName("gainloss")]
        public AccountGainLossContainer? Data { get; set; }
        public class AccountGainLossContainer
        {
            [JsonPropertyName("closed_position")]
            public List<ClosedPosition> GainLoss { get; set; } = new();
        }

        internal override void Deserialize()
        {
            try
            {
                Data = System.Text.Json.JsonSerializer.Deserialize<AccountGainLossResponse>(this.RawResponse)?.Data ?? new();
            }
            catch (Exception)
            {
                Data = new();
            }
        }
    }
}
