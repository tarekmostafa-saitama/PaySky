using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Common.Authentication;

/// <summary>
///     Interface for generating refresh token.
/// </summary>
public interface IRefreshTokenService : ITokenService, IScopedService
{
}