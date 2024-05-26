using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WeightTracker.Domain.Common.Interfaces;

namespace WeightTracker.Api.Infrastructure.Http;

/// <inheritdoc />
internal sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : IUser
{
    /// <inheritdoc />
    /// <remarks>
    /// It uses the <see cref="HttpContext"/> to get the user ID from the <see cref="ClaimsPrincipal"/>.
    /// </remarks>
    public string? Id => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
