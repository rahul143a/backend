using Domain.MultiTenancy;
using Shared.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration.MultiTenancy;

/// <summary>
/// Configuration for AppTenantInfo entity
/// </summary>
public class AppTenantInfoConfig : IEntityTypeConfiguration<AppTenantInfo>
{
    /// <summary>
    /// Configure the entity
    /// </summary>
    public void Configure(EntityTypeBuilder<AppTenantInfo> builder)
    {
        builder.ToTable("Tenants", SchemaNames.MultiTenancy);
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .HasMaxLength(64)
            .IsRequired();
            
        builder.Property(t => t.Identifier)
            .HasMaxLength(64)
            .IsRequired();
            
        builder.Property(t => t.Name)
            .HasMaxLength(128)
            .IsRequired();
            
        builder.Property(t => t.ConnectionString)
            .HasMaxLength(512);
            
        builder.Property(t => t.AdminEmail)
            .HasMaxLength(256)
            .IsRequired();
            
        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(t => t.ValidUpto)
            .IsRequired(false);
    }
}
