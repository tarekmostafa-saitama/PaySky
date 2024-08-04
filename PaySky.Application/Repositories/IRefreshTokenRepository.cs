using PaySky.Domain.Entities;
using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>, IScopedService
{
}