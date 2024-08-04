using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Common.Authentication;

/// <summary>
///     Interface for generating access token.
/// </summary>
public interface IAccessTokenService : ITokenService, IScopedService
{
}