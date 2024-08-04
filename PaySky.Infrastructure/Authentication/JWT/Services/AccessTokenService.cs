using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PaySky.Application.Common.Authentication;
using PaySky.Domain.Entities;
using PaySky.Infrastructure.Authentication.JWT.Models;
using PaySky.Infrastructure.Identity;

namespace PaySky.Infrastructure.Authentication.JWT.Services;

internal sealed class AccessTokenService(ITokenGenerator tokenGenerator, JwtSettings jwtSettings,
        UserManager<ApplicationUser> userManager)
    : IAccessTokenService
{
    public async Task<string> Generate(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        var roles = await userManager.GetRolesAsync(user);

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return tokenGenerator.Generate(new GenerateTokenRequest(jwtSettings.AccessTokenSecret, jwtSettings.Issuer,
            jwtSettings.Audience,
            jwtSettings.AccessTokenExpirationMinutes, claims)).Token;
    }
}