using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaySky.Application.Common.Services;
using PaySky.Domain.Common;
using PaySky.Domain.Entities;
using PaySky.Infrastructure.Identity;
using PaySky.Shared.Contracts;

namespace PaySky.Infrastructure.Persistence.Context;

public abstract class BaseDbContext(DbContextOptions<ApplicationDbContext> options,
        ISerializerService serializerService,
        ICurrentUserService currentUserService)
    : IdentityDbContext<ApplicationUser>(options)
{
    private readonly ISerializerService _serializerService = serializerService;

    public DbSet<RefreshToken> RefreshTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // QueryFilters need to be applied before base.OnModelCreating
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        HandleAuditingBeforeSaveChanges(currentUserService.Id);

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }


    private void HandleAuditingBeforeSaveChanges(string userId)
    {
        foreach (var entry in ChangeTracker.Entries<IEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModified = DateTimeOffset.UtcNow;
                    entry.Entity.LastModifiedBy = userId;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = userId;
                        softDelete.DeletedOn = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                    }

                    break;
            }
    }
}