using Domain.Identity;
using Shared.Database;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Identity;

/// <summary>
/// Configuration for ApplicationRole entity
/// </summary>
public class ApplicationRoleConfig : IEntityTypeConfiguration<ApplicationRole>
{
    /// <summary>
    /// Configure the entity
    /// </summary>
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("Roles", SchemaNames.Identity);

        // Configure multi-tenancy
        builder.IsMultiTenant();

        // Configure properties
        builder.Property(r => r.Description)
            .HasMaxLength(256);
    }
}
