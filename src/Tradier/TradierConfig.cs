namespace Tradier
{
    /// <summary>
    /// Static configuration for Tradier API clients.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>⚠️ For simple scenarios only.</b> This static configuration is suitable for console applications,
    /// scripts, and single-user desktop applications where only one token is used throughout the app lifetime.
    /// </para>
    /// <para>
    /// <b>Not thread-safe.</b> The static properties can be modified at any time, which may cause
    /// unexpected behavior in multi-threaded applications. Changing the token while requests are in
    /// flight could affect other operations.
    /// </para>
    /// <para>
    /// <b>For ASP.NET Core and multi-tenant applications</b>, use the dependency injection approach instead:
    /// <code>
    /// builder.Services.AddTradier(options =>
    /// {
    ///     options.AccessToken = "your-token";
    ///     options.UseSandbox = true;
    /// });
    /// </code>
    /// </para>
    /// </remarks>
    [Obsolete("For ASP.NET Core applications, use builder.Services.AddTradier() instead. " +
              "TradierConfig is static and not thread-safe. " +
              "See documentation for details.")]
    public static class TradierConfig
    {
        private static bool IsUsingSandboxAndLiveClients = false;

        internal static ITradierClient DefaultClient
        {
            get
            {
                if (IsUsingSandboxAndLiveClients)
                    throw new InvalidOperationException(
                        "You cannot implicitly use both the Sandbox Client and the Live client in the same application. " +
                        "To use both, please explicitly pass in the client you want to use in each service.");
                
                if (defaultClient is null)
                    throw new InvalidOperationException(
                        "The default Tradier Client is not initialized. " +
                        "Please create an instance of the client before using any services, " +
                        "or use builder.Services.AddTradier() for dependency injection.");
                
                return defaultClient;
            }
            set
            {
                if (!IsUsingSandboxAndLiveClients && defaultClient != null && value != null)
                {
                    if (defaultClient.GetType() != value.GetType())
                        IsUsingSandboxAndLiveClients = true;
                }
                defaultClient = value;
            }
        }

        private static ITradierClient? defaultClient;

        /// <summary>
        /// Gets or sets the API access token.
        /// </summary>
        /// <remarks>
        /// <b>Warning:</b> This is static and not thread-safe. For web applications, use AddTradier() instead.
        /// </remarks>
        public static string? AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the OAuth redirect URI. Only needed for OAuth authorization flows.
        /// </summary>
        /// <remarks>
        /// <b>Warning:</b> This is static and not thread-safe. For web applications, use AddTradier() instead.
        /// </remarks>
        public static string? RedirectUri { get; set; }
    }
}
