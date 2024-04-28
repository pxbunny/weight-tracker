namespace WeightTracker.CliApp.Authentication;

/// <summary>
/// Represents the authentication options.
/// </summary>
internal sealed class AuthOptions
{
    /// <summary>
    /// The position of the authentication options in the configuration file.
    /// </summary>
    public const string Position = "Auth";

    /// <summary>
    /// Gets or initializes the client ID.
    /// </summary>
    /// <value>The client ID.</value>
    public string ClientId { get; init; } = null!;

    /// <summary>
    /// Gets or initializes the tenant ID.
    /// </summary>
    /// <value>The tenant ID.</value>
    public string TenantId { get; init; } = null!;

    /// <summary>
    /// Gets or initializes the redirect URI.
    /// </summary>
    /// <value>The redirect URI.</value>
    public string RedirectUri { get; init; } = null!;

    /// <summary>
    /// Deconstructs the authentication options.
    /// </summary>
    /// <param name="clientId">The client ID.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="redirectUrl">The redirect URI.</param>
    /// <example>
    /// <code>
    /// var (clientId, tenantId, redirectUrl) = authOptions;
    /// </code>
    /// </example>
    public void Deconstruct(
        out string clientId,
        out string tenantId,
        out string redirectUrl)
    {
        clientId = ClientId;
        tenantId = TenantId;
        redirectUrl = RedirectUri;
    }
}
