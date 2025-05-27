using Domain.Identity;
using Shared.Database;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.Identity;

/// <summary>
/// Configuration for ApplicationUser entity
/// </summary>
public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    /// <summary>
    /// Configure the entity
    /// </summary>
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users", SchemaNames.Identity);

        // Configure multi-tenancy
        builder.IsMultiTenant();

        // Configure properties
        builder.Property(u => u.FirstName)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(128);

        builder.Property(u => u.ImageUrl)
            .HasMaxLength(2048);

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(256);

        builder.Property(u => u.ObjectId)
            .HasMaxLength(256);
    }
}
