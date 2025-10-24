using System.Security.Claims;

namespace WeightTracker.Api.Services;

internal sealed class CurrentUser(IHttpContextAccessor httpContextAccessor)
{
    public string Id => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
