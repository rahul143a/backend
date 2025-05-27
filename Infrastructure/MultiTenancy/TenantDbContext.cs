using Finbuckle.MultiTenant.Stores;
using Abstraction.Common;
using Domain.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MultiTenancy;

/// <summary>
/// Database context for tenant information
/// </summary>
public class TenantDbContext : EFCoreStoreDbContext<AppTenantInfo>
{
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Constructor
    /// </summary>
    public TenantDbContext(DbContextOptions<TenantDbContext> options, ICurrentUser currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    /// <summary>
    /// Tenant information
    /// </summary>
    public new DbSet<AppTenantInfo> TenantInfo => Set<AppTenantInfo>();

    /// <summary>
    /// Configure the model
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppTenantInfo>().ToTable("Tenants", "MultiTenancy");
    }

    /// <summary>
    /// Configure the context
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Suppress the PendingModelChangesWarning
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    /// <summary>
    /// Save changes
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AppTenantInfo>().Where(e => e.State == EntityState.Added))
        {
            entry.Entity.CreatedBy = _currentUser.GetUserId();
            entry.Entity.CreatedOn = DateTime.UtcNow;
        }

        foreach (var entry in ChangeTracker.Entries<AppTenantInfo>().Where(e => e.State == EntityState.Modified))
        {
            entry.Entity.LastModifiedBy = _currentUser.GetUserId();
            entry.Entity.LastModifiedOn = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
