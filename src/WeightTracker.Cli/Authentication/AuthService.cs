using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace WeightTracker.Cli.Authentication;

internal class AuthService(IOptions<AuthOptions> authOptions)
{
    public async Task AcquireTokenAsync()
    {
        var (clientId, tenantId, redirectUri) = authOptions.Value;

        var scopes = new[] { $"api://{clientId}/access_as_user" };

        var options = new PublicClientApplicationOptions
        {
            ClientId = clientId,
            TenantId = tenantId,
            RedirectUri = redirectUri
        };

        var client = PublicClientApplicationBuilder
            .CreateWithApplicationOptions(options)
            .Build();

        var authResult = await client
            .AcquireTokenInteractive(scopes)
            .ExecuteAsync();

#if DEBUG
        Console.WriteLine($"Token: {authResult.AccessToken}");
#endif

        Environment.SetEnvironmentVariable("AUTH_TOKEN", authResult.AccessToken, EnvironmentVariableTarget.User);
    }

    public void ForgetToken()
    {
        Environment.SetEnvironmentVariable("AUTH_TOKEN", null, EnvironmentVariableTarget.User);
    }
}
