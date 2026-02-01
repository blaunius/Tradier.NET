namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents the possible statuses of a trading account.
    /// </summary>
    public enum AccountStatus
    {
        /// <summary>
        /// Account status is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Account is active and operational.
        /// </summary>
        Active,

        /// <summary>
        /// Account is closed and no longer operational.
        /// </summary>
        Closed,

        /// <summary>
        /// Account is suspended and temporarily inactive.
        /// </summary>
        Suspended,

        /// <summary>
        /// Account is pending activation or verification.
        /// </summary>
        Pending
    }
}