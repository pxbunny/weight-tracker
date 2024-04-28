using System.Security.Claims;

namespace WeightTracker.Api.Services;

/// <summary>
/// Represents the current user service.
/// </summary>
internal interface ICurrentUserService
{
    /// <summary>
    /// Gets the ID of the currently logged-in user.
    /// </summary>
    /// <value>The user ID.</value>
    public string? UserId { get; }
}

/// <inheritdoc />
internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    /// <inheritdoc />
    /// <remarks>
    /// It uses the <see cref="HttpContext"/> to get the user ID from the <see cref="ClaimsPrincipal"/>.
    /// </remarks>
    public string? UserId => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
