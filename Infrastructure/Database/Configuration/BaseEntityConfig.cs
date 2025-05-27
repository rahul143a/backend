using Domain.Common.Contracts;
using Infrastructure.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

/// <summary>
/// Base configuration for entities
/// </summary>
public abstract class BaseEntityConfig<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Configure the entity
    /// </summary>
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Configure base entity properties
        if (typeof(IEntity<Guid>).IsAssignableFrom(typeof(TEntity)))
        {
            builder.HasKey("Id");
        }
    }
}
