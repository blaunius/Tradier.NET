using Microsoft.VisualBasic;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tradier.Model
{
    public class Expiration
    {
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        [JsonPropertyName("contract_size")]
        public int? ContractSize { get; set; }

        [JsonPropertyName("expiration_type")]
        public string? ExpirationType { get; set; }

        [JsonPropertyName("strikes")]
        public Response.MarketOptionStikesResponse.StrikeData? Strikes { get; set; }
    }
}
