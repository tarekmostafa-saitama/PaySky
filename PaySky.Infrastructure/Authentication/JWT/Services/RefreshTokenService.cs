using PaySky.Application.Common.Authentication;
using PaySky.Infrastructure.Authentication.JWT.Models;

namespace PaySky.Infrastructure.Authentication.JWT.Services;

public class RefreshTokenService(ITokenGenerator tokenGenerator, JwtSettings jwtSettings) : IRefreshTokenService
{
    public async Task<string> Generate(string userId)
    {
        return tokenGenerator.Generate(new GenerateTokenRequest(
            jwtSettings.RefreshTokenSecret,
            jwtSettings.Issuer, jwtSettings.Audience,
            jwtSettings.RefreshTokenExpirationMinutes)).Token;
    }
}