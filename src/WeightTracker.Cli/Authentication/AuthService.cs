using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace WeightTracker.Cli.Authentication;

internal class AuthService(IOptions<AuthOptions> authOptions)
{
    public async Task AcquireTokenAsync()
    {
        var (clientId, tenantId, redirectUrl) = authOptions.Value;
        var scopes = new[] { "User.Read" };

        var client = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
            .WithRedirectUri(redirectUrl)
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
