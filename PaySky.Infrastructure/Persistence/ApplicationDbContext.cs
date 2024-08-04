using Microsoft.EntityFrameworkCore;
using PaySky.Application.Common.Services;
using PaySky.Domain.Entities;
using PaySky.Infrastructure.Persistence.Context;

namespace PaySky.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ISerializerService serializerService,
        ICurrentUserService currentUserService)
    : BaseDbContext(options, serializerService, currentUserService)
{
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<ArchivedVacancy>  ArchivedVacancies { get; set; }
    public DbSet<Domain.Entities.Application> Applications { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}