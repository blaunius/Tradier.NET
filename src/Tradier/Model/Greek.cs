using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tradier.Model
{
    public class Greek
    {
        [JsonPropertyName("delta")]
        public decimal? Delta { get; init; }

        [JsonPropertyName("gamma")]
        public decimal? Gamma { get; init; }

        [JsonPropertyName("theta")]
        public decimal? Theta { get; init; }

        [JsonPropertyName("vega")]
        public decimal? Vega { get; init; }

        [JsonPropertyName("rho")]
        public decimal? Rho { get; init; }

        [JsonPropertyName("phi")]
        public decimal? Phi { get; init; }

        [JsonPropertyName("bid_iv")]
        public decimal? BidIv { get; init; }

        [JsonPropertyName("mid_iv")]
        public decimal? MidIv { get; init; }

        [JsonPropertyName("ask_iv")]
        public decimal? AskIv { get; init; }

        [JsonPropertyName("smv_vol")]
        public decimal? SmvVol { get; init; }

        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; init; }
    }
}
