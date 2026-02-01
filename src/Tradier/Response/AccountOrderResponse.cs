using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tradier.Model;

namespace Tradier.Response
{
    public class AccountOrderResponse : TradierResponse
    {
        [JsonPropertyName("order")]
        public Order? Order { get; set; }
        internal override void Deserialize()
        {
            Order = System.Text.Json.JsonSerializer.Deserialize<AccountOrderResponse>(this.RawResponse)?.Order ?? new();
        }
    }
}
