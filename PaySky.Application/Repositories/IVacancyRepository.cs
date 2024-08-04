using PaySky.Domain.Entities;
using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Repositories;

public interface IVacancyRepository : IRepository<Vacancy>, IScopedService
{
}