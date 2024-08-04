using PaySky.Application.Repositories;
using PaySky.Domain.Entities;
using PaySky.Infrastructure.Persistence;

namespace PaySky.Infrastructure.Repositories;

public class ArchivedVacancyRepository(ApplicationDbContext context) : EfRepository<ArchivedVacancy>(context), IArchivedVacancyRepository

{

}
