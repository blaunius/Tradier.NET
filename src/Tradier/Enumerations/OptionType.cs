using System.Text.Json.Serialization;

namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents the type of options contract (Call or Put).
    /// </summary>
    public enum OptionType
    {
        /// <summary>
        /// Option type is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Call option - gives the right to buy the underlying security.
        /// </summary>
        [JsonPropertyName("call")]
        Call,

        /// <summary>
        /// Put option - gives the right to sell the underlying security.
        /// </summary>
        [JsonPropertyName("put")]
        Put
    }
}