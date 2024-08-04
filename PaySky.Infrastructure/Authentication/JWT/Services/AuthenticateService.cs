using Microsoft.AspNetCore.Identity;
using PaySky.Application.Common.Authentication;
using PaySky.Application.Requests.Authentication.Models;
using PaySky.Domain.Entities;
using PaySky.Infrastructure.Identity;
using PaySky.Infrastructure.Persistence;

namespace PaySky.Infrastructure.Authentication.JWT.Services;

internal sealed class AuthenticateService(IAccessTokenService accessTokenService,
        IRefreshTokenService refreshTokenService,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    : IAuthenticateService
{
    public async Task<AuthenticateResponse> Authenticate(string userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);

        var refreshToken = await refreshTokenService.Generate(user.Id);
        await context.RefreshTokens.AddAsync(new RefreshToken { Token = refreshToken, UserId = user.Id },
            cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return new AuthenticateResponse
        {
            AccessToken = await accessTokenService.Generate(user.Id),
            RefreshToken = refreshToken
        };
    }
}