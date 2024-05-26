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
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task representing an asynchronous operation.</returns>
    Task AcquireTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <returns>The access token.</returns>
    string? GetToken();

    /// <summary>
    /// Forgets the access token asynchronously.
    /// </summary>
    /// <returns>The task representing an asynchronous operation.</returns>
    Task ForgetTokenAsync();
}
