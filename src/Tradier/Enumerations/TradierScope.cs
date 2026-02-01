namespace Tradier.Enumerations
{
    public enum TradierAuthScope
    {
        /// <summary>
        /// Read-only access to account data.
        /// </summary>
        Read,
        /// <summary>
        /// Write access to account data (does not include placing or updating trades).
        /// </summary>
        Write,
        /// <summary>
        /// Access to market data (does not include streaming).
        /// </summary>
        Market,
        /// <summary>
        /// Ability to update, cancel, and place trades.
        /// </summary>
        Trade,
        /// <summary>
        /// Create streaming sessions for use with the Streaming API.
        /// </summary>
        Stream
    }
}
