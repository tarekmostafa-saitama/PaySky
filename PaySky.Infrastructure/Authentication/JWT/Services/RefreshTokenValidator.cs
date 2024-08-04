using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PaySky.Application.Common.Authentication;
using PaySky.Infrastructure.Authentication.JWT.Models;

namespace PaySky.Infrastructure.Authentication.JWT.Services;

internal sealed class RefreshTokenValidator(JwtSettings jwtSettings) : IRefreshTokenValidator
{
    public bool Validate(string refreshToken)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.RefreshTokenSecret)),
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ClockSkew = TimeSpan.Zero
        };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        try
        {
            jwtSecurityTokenHandler.ValidateToken(refreshToken, validationParameters, out var _);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}