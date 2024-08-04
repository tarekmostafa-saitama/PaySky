using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaySky.Domain.Entities;
using PaySky.Domain.ValueObjects;

namespace PaySky.Infrastructure.Persistence.Configurations;

public class ArchivedVacanciesConfiguration : IEntityTypeConfiguration<ArchivedVacancy>
{
    public void Configure(EntityTypeBuilder<ArchivedVacancy> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).HasConversion(
            vacancyId => vacancyId.Value,
            dbId => VacancyId.Of(dbId));


    }
}