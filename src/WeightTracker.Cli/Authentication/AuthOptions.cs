namespace WeightTracker.Cli.Authentication;

public sealed class AuthOptions
{
    public const string Position = "Auth";
    
    public string ClientId { get; init; } = null!;
    
    public string TenantId { get; init; } = null!;

    public string RedirectUri { get; init; } = null!;
    
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
