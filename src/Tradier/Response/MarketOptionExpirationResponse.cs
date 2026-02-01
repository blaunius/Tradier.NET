using System.Text.Json;
using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response
{
    public class MarketOptionExpirationResponse : TradierResponse
    {
        [JsonPropertyName("expirations")]
        public ExpirationContainer? Data { get; set; }
        
        public class ExpirationContainer
        {
            /// <summary>
            /// Simple date array (when no options are specified)
            /// </summary>
            [JsonPropertyName("date")]
            public List<string>? Dates { get; set; }
            
            /// <summary>
            /// Full expiration objects (when strikes/contractSize/expirationType are requested)
            /// </summary>
            [JsonPropertyName("expiration")]
            public List<Expiration>? Expirations { get; set; }
        }
        
        internal override void Deserialize()
        {
            this.Data = JsonSerializer.Deserialize<MarketOptionExpirationResponse>(this.RawResponse)?.Data 
                ?? new ExpirationContainer();
        }
    }
}
