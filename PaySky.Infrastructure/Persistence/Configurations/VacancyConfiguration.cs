using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaySky.Domain.Entities;
using PaySky.Domain.ValueObjects;

namespace PaySky.Infrastructure.Persistence.Configurations;

public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).HasConversion(
            vacancyId => vacancyId.Value,
            dbId => VacancyId.Of(dbId));


    }
}