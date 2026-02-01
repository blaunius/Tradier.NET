using System.Text.Json.Serialization;
using Tradier.Model;

namespace Tradier.Response.DataContracts
{
    /// <summary>
    /// Data contract for account balance API responses.
    /// </summary>
    public class BalanceDataContract
    {
        /// <summary>
        /// Gets or sets the balance information from the API response.
        /// </summary>
        [JsonPropertyName("balances")]
        public Balance? Balances { get; set; }
    }
}