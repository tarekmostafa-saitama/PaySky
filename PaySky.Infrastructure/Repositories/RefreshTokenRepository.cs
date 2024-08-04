using PaySky.Application.Repositories;
using PaySky.Domain.Entities;
using PaySky.Infrastructure.Persistence;

namespace PaySky.Infrastructure.Repositories;

public class RefreshTokenRepository(ApplicationDbContext dbContext) : EfRepository<RefreshToken>(dbContext),
    IRefreshTokenRepository;