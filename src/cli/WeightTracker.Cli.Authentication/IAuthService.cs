namespace WeightTracker.Cli.Authentication;

/// <summary>
/// Represents the authentication service.
/// </summary>
/// <remarks>
/// This class is used to acquire and store the access token.
/// </remarks>
public interface IAuthService
{
    /// <summary>
    /// Acquires the access token asynchronously.
    /// </summary>
    /// <param name="persistAccessToken">A value indicating whether to persist the access token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing an asynchronous operation. The task result contains the access token.</returns>
    Task<string> AcquireTokenAsync(bool persistAccessToken = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <returns>The access token.</returns>
    string? GetToken();

    /// <summary>
    /// Forgets the access token asynchronously.
    /// </summary>
    void ForgetToken();
}
