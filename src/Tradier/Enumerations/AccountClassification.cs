namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents the classification of a trading account.
    /// </summary>
    public enum AccountClassification
    {
        /// <summary>
        /// Classification is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Individual account.
        /// </summary>
        Individual,

        /// <summary>
        /// Joint account with multiple owners.
        /// </summary>
        Joint,

        /// <summary>
        /// Corporate account.
        /// </summary>
        Corporate,

        /// <summary>
        /// Partnership account.
        /// </summary>
        Partnership,

        /// <summary>
        /// Trust account.
        /// </summary>
        Trust,

        /// <summary>
        /// Estate account.
        /// </summary>
        Estate
    }
}