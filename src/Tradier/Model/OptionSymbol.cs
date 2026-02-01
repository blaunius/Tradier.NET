using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tradier.Model
{
    public class OptionSymbol
    {
        [JsonPropertyName("rootSymbol")]
        public string? RootSymbol { get; set; }
        [JsonPropertyName("options")]
        public string[]? Options { get; set; }
    }
}
