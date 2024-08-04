using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaySky.Domain.Entities;
using PaySky.Domain.ValueObjects;
using ApplicationId = PaySky.Domain.ValueObjects.ApplicationId;

namespace PaySky.Infrastructure.Persistence.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Domain.Entities.Application>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Application> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).HasConversion(
            applicationId => applicationId.Value,
            dbId => ApplicationId.Of(dbId));

        builder.Property(o => o.VacancyId).HasConversion(
            vacancyId => vacancyId.Value,
            dbId => VacancyId.Of(dbId));

        builder.HasOne(x => x.Vacancy)
            .WithMany(x => x.Applications)
            .OnDelete(DeleteBehavior.Cascade);

    }
}