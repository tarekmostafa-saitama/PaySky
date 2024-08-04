using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PaySky.Application.Common.Services;

namespace PaySky.Infrastructure.Identity.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string Id => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}