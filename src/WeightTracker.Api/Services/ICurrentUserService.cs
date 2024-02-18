using System.Security.Claims;

namespace WeightTracker.Api.Services;

internal interface ICurrentUserService
{
    public string? UserId { get; }
}

internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
