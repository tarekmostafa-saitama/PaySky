using PaySky.Application.Repositories;
using PaySky.Domain.Entities;
using PaySky.Infrastructure.Persistence;

namespace PaySky.Infrastructure.Repositories;

public class ApplicationRepository(ApplicationDbContext context) : EfRepository<Domain.Entities.Application>(context),
    IApplicationRepository
{

}
