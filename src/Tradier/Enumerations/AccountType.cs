namespace Tradier.Enumerations
{
    /// <summary>
    /// Represents the different types of trading accounts.
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// Account type is unknown or not specified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Cash account - transactions settle immediately.
        /// </summary>
        Cash,

        /// <summary>
        /// Margin account - allows borrowing against securities.
        /// </summary>
        Margin,

        /// <summary>
        /// Individual Retirement Account.
        /// </summary>
        IRA,

        /// <summary>
        /// Roth Individual Retirement Account.
        /// </summary>
        RothIRA,

        /// <summary>
        /// Traditional Individual Retirement Account.
        /// </summary>
        TraditionalIRA
    }
}