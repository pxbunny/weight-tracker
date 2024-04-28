using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace WeightTracker.CliApp.Authentication;

/// <summary>
/// Represents the authentication service.
/// </summary>
/// <remarks>
/// This class is used to acquire and store the access token.
/// </remarks>
/// <param name="authOptions">The authentication options.</param>
internal sealed class AuthService(IOptions<AuthOptions> authOptions)
{
    private const string EnvVariableName = "AUTH_TOKEN";

    /// <summary>
    /// Acquires the access token asynchronously.
    /// </summary>
    /// <remarks>
    /// This method uses the interactive authentication flow to acquire the access token.
    /// </remarks>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing an asynchronous operation.</returns>
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

        Environment.SetEnvironmentVariable(EnvVariableName, authResult.AccessToken, EnvironmentVariableTarget.User);
    }

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <remarks>
    /// This method retrieves the access token from the environment variable.
    /// It uses the user environment variable target, so the access token is stored between sessions.
    /// </remarks>
    /// <returns>The access token.</returns>
    public string? GetToken()
    {
        return Environment.GetEnvironmentVariable(EnvVariableName, EnvironmentVariableTarget.User);
    }

    /// <summary>
    /// Forgets the access token asynchronously.
    /// </summary>
    /// <remarks>
    /// This method removes the access token from the environment variable.
    /// </remarks>
    /// <returns>The task representing an asynchronous operation.</returns>
    public Task ForgetTokenAsync()
    {
        Environment.SetEnvironmentVariable(EnvVariableName, null, EnvironmentVariableTarget.User);
        return Task.CompletedTask;
    }
}
