using PaySky.Domain.Entities;
using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Repositories;

public interface IApplicationRepository : IRepository<Domain.Entities.Application>, IScopedService
{
    
}