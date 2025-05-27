using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Abstraction.Common;
using Domain.Common.Contracts;
using Domain.Identity;
using Domain.MultiTenancy;
using Infrastructure.Database.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext : MultiTenantIdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>, ITenantDbContext
{
    /// <summary>
    /// Current user service
    /// </summary>
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Tenant ID
    /// </summary>
    public string? TenantId => TenantInfo?.Id;

    /// <summary>
    /// Constructor
    /// </summary>
    public ApplicationDbContext(ITenantInfo tenantInfo, DbContextOptions options, ICurrentUser currentUser)
        : base(tenantInfo, options)
    {
        _currentUser = currentUser;
    }

    /// <summary>
    /// Tenants DbSet
    /// </summary>
    public DbSet<AppTenantInfo> Tenants => Set<AppTenantInfo>();

    /// <summary>
    /// Configure the model
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply global filters for soft delete
        modelBuilder.ApplyGlobalFilters<ISoftDelete>(s => s.IsDeleted == false);

        base.OnModelCreating(modelBuilder);

        // Apply entity configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        // Apply identity configuration
        modelBuilder.ApplyIdentityConfiguration();
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
        var userId = _currentUser.GetUserId();
        var auditEntries = HandleAuditingBeforeSaveChanges(userId);
        int result = await base.SaveChangesAsync(cancellationToken);
        await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);
        return result;
    }

    /// <summary>
    /// Handle auditing before save changes
    /// </summary>
    private List<AuditTrail> HandleAuditingBeforeSaveChanges(string? userId)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDeleteEntity)
                    {
                        softDeleteEntity.DeletedBy = string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
                        softDeleteEntity.DeletedOn = DateTime.UtcNow;
                        softDeleteEntity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }

        ChangeTracker.DetectChanges();

        var trailEntries = new List<AuditTrail>();
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
            .ToList())
        {
            var trailEntry = new AuditTrail(entry, _currentUser)
            {
                TableName = entry.Entity.GetType().Name,
                UserId = userId
            };
            trailEntries.Add(trailEntry);
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    trailEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    trailEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        trailEntry.TrailType = TrailType.Create;
                        trailEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        trailEntry.TrailType = TrailType.Delete;
                        trailEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null && property.CurrentValue != null)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Delete;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Update;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }

        foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
        {
            // TODO: Save audit trails to database or other storage
        }

        return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
    }

    /// <summary>
    /// Handle auditing after save changes
    /// </summary>
    private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, CancellationToken cancellationToken = default)
    {
        if (trailEntries == null || trailEntries.Count == 0)
            return Task.CompletedTask;

        foreach (var entry in trailEntries)
        {
            foreach (var prop in entry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }
        }

        // TODO: Save audit trails to database or other storage
        return Task.CompletedTask;
    }
}
