using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace WeightTracker.Cli.Authentication;

internal class AuthService(IOptions<AuthOptions> authOptions)
{
    public async Task AcquireTokenAsync(CancellationToken cancellationToken = default)
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
            .ExecuteAsync(cancellationToken);

        Environment.SetEnvironmentVariable("AUTH_TOKEN", authResult.AccessToken, EnvironmentVariableTarget.User);
    }

    public string? GetToken()
    {
        return Environment.GetEnvironmentVariable("AUTH_TOKEN", EnvironmentVariableTarget.User);
    }

    public Task ForgetTokenAsync()
    {
        Environment.SetEnvironmentVariable("AUTH_TOKEN", null, EnvironmentVariableTarget.User);
        return Task.CompletedTask;
    }
}
