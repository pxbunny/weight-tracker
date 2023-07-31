using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using WeightTracker.Cli.Configuration;

namespace WeightTracker.Cli.Services;

public class AuthService : IAuthService
{
    private readonly AuthOptions _authOptions;

    public AuthService(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions.Value;
    }
    
    public async Task AcquireTokenAsync()
    {
        var (tenantId, clientId, redirectUrl) = _authOptions;
        var scopes = new[] { "User.Read" };

        var client = PublicClientApplicationBuilder.Create(clientId)
         .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
         .WithRedirectUri(redirectUrl)
         .Build();

        var authResult = await client.AcquireTokenInteractive(scopes).ExecuteAsync();
        // TODO: persist token
    }
}
