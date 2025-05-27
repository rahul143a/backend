using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Domain.Common.Contracts;
using Domain.Identity;
using Shared.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Database.Extensions;

/// <summary>
/// Extensions for model builder
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Apply identity configuration
    /// </summary>
    public static void ApplyIdentityConfiguration(this ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().ToTable("Users", SchemaNames.Identity);
        builder.Entity<ApplicationRole>().ToTable("Roles", SchemaNames.Identity);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().ToTable("UserRoles", SchemaNames.Identity);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>().ToTable("UserClaims", SchemaNames.Identity);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>().ToTable("UserLogins", SchemaNames.Identity);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>().ToTable("RoleClaims", SchemaNames.Identity);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>().ToTable("UserTokens", SchemaNames.Identity);
    }

    /// <summary>
    /// Apply global query filter for soft delete entities
    /// </summary>
    public static void ApplySoftDeleteGlobalFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    /// <summary>
    /// Append a global query filter to entities that implement a specific interface
    /// </summary>
    public static ModelBuilder AppendGlobalQueryFilter<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> filter)
    {
        // Get a list of entities without a base type that implement the interface TInterface
        var entities = modelBuilder.Model.GetEntityTypes()
            .Where(e => e.BaseType is null && e.ClrType.GetInterface(typeof(TInterface).Name) is not null)
            .Select(e => e.ClrType);

        foreach (var entity in entities)
        {
            var parameterType = Expression.Parameter(modelBuilder.Entity(entity).Metadata.ClrType);
            var filterBody = ReplacingExpressionVisitor.Replace(filter.Parameters.Single(), parameterType, filter.Body);

            // Get the existing query filter
            if (modelBuilder.Entity(entity).Metadata.GetQueryFilter() is { } existingFilter)
            {
                var existingFilterBody = ReplacingExpressionVisitor.Replace(existingFilter.Parameters.Single(), parameterType, existingFilter.Body);

                // Combine the existing query filter with the new query filter
                filterBody = Expression.AndAlso(existingFilterBody, filterBody);
            }

            // Apply the new query filter
            modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(filterBody, parameterType));
        }

        return modelBuilder;
    }

    /// <summary>
    /// Apply global filters
    /// </summary>
    public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
    {
        var entities = modelBuilder.Model
            .GetEntityTypes()
            .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
            .Select(e => e.ClrType);

        foreach (var entity in entities)
        {
            var newParam = Expression.Parameter(entity);
            var newBody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
            modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newBody, newParam));
        }
    }

    /// <summary>
    /// Apply tenant configuration
    /// </summary>
    public static void ApplyTenantConfiguration(this ModelBuilder modelBuilder)
    {
        // Configure tenant entities
        modelBuilder.Entity<Domain.MultiTenancy.AppTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}
