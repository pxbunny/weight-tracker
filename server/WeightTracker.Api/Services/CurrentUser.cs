using System.Security.Claims;

namespace WeightTracker.Api.Services;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor)
{
    public string? Id => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
