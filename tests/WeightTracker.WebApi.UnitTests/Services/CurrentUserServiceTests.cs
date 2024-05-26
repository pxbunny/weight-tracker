// using System.Security.Claims;
// using Microsoft.AspNetCore.Http;
//
// namespace WeightTracker.WebApi.UnitTests.Services;
//
// public sealed class CurrentUserServiceTests
// {
//     [Fact]
//     public void UserId_ReturnsUserId()
//     {
//         // Arrange
//         const string userId = "user-id";
//         var httpContext = new DefaultHttpContext();
//         httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
//         {
//             new Claim(ClaimTypes.NameIdentifier, userId)
//         }));
//
//         var httpContextAccessor = new HttpContextAccessor
//         {
//             HttpContext = httpContext
//         };
//
//         var service = new CurrentUserService(httpContextAccessor);
//
//         // Act
//         var result = service.UserId;
//
//         // Assert
//         Assert.Equal(userId, result);
//     }
//
//     [Fact]
//     public void UserId_ReturnsNullWhenUserNotLoggedIn()
//     {
//         // Arrange
//         var httpContext = new DefaultHttpContext();
//         var httpContextAccessor = new HttpContextAccessor
//         {
//             HttpContext = httpContext
//         };
//
//         var service = new CurrentUserService(httpContextAccessor);
//
//         // Act
//         var result = service.UserId;
//
//         // Assert
//         Assert.Null(result);
//     }
// }
