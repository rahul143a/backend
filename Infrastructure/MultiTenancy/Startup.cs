using Dapper;
using Finbuckle.MultiTenant;
using Abstraction.MultiTenancy;
using Domain.MultiTenancy;
using Infrastructure.Database;
using Infrastructure.MultiTenancy.TenantResolutions;
using Shared.MultiTenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.MultiTenancy;

/// <summary>
/// Startup configuration for multi-tenancy
/// </summary>
public static class Startup
{
    /// <summary>
    /// Add multi-tenancy services
    /// </summary>
    public static IServiceCollection AddMultitenancy(this IServiceCollection services, IConfiguration config, string tenantId)
    {
        return services
            .AddDbContext<TenantDbContext>((p, m) =>
            {
                m.UseNpgsql(DatabaseSettings.ConnectionString,
                    npgsql => npgsql.MigrationsAssembly("Inventory.Migrators"));
            })
            .AddMultiTenant<AppTenantInfo>()
                .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
                .WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
                .WithRouteStrategy(MultitenancyConstants.TenantIdName)
                .WithStrategy<CustomTenantResolutionStrategy>(ServiceLifetime.Transient)
                .WithStaticStrategy(string.IsNullOrEmpty(tenantId) ? MultitenancyConstants.Root.Id : tenantId)
            .WithInMemoryStore(inMemoryConfig =>
            {
                var tenants = GetAllTenantsInfo(DatabaseSettings.ConnectionString);
                inMemoryConfig.Tenants = tenants;
            }).Services.AddScoped<ITenantService, TenantService>();
    }

    /// <summary>
    /// Add multi-tenancy services for meta operations
    /// </summary>
    public static IServiceCollection AddMultitenancyForMeta(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddDbContext<TenantDbContext>((p, m) =>
            {
                m.UseNpgsql(DatabaseSettings.ConnectionString,
                    npgsql => npgsql.MigrationsAssembly("Inventory.Migrators"));
            })
            .AddMultiTenant<AppTenantInfo>()
                .WithHeaderStrategy(MultitenancyConstants.TenantIdName)
                .WithQueryStringStrategy(MultitenancyConstants.TenantIdName)
                .WithRouteStrategy(MultitenancyConstants.TenantIdName)
                .WithStaticStrategy(MultitenancyConstants.Root.Id)
            .WithInMemoryStore(inMemoryConfig =>
            {
                var tenants = GetAllTenantsInfo(DatabaseSettings.ConnectionString);
                inMemoryConfig.Tenants = tenants;
            }).Services;
    }

    /// <summary>
    /// Use multi-tenancy middleware
    /// </summary>
    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app) =>
        app.UseMultiTenant();

    /// <summary>
    /// Add query string strategy for multi-tenancy
    /// </summary>
    private static FinbuckleMultiTenantBuilder<AppTenantInfo> WithQueryStringStrategy(this FinbuckleMultiTenantBuilder<AppTenantInfo> builder, string queryStringKey) =>
        builder.WithDelegateStrategy(context =>
        {
            if (context is not HttpContext httpContext)
            {
                return Task.FromResult((string?)null);
            }

            httpContext.Request.Query.TryGetValue(queryStringKey, out StringValues tenantIdParam);

            return Task.FromResult((string?)tenantIdParam.ToString());
        });

    /// <summary>
    /// Get all tenants information
    /// </summary>
    private static List<AppTenantInfo> GetAllTenantsInfo(string connectionString)
    {
        var conn = new NpgsqlConnection(connectionString);
        try
        {
            conn.Open();
            var tenants = conn.Query<AppTenantInfo>("SELECT * FROM \"MultiTenancy\".\"Tenants\"");
            return tenants.GroupBy(t => t.Identifier).Select(g => g.First()).ToList();
        }
        catch (Exception e)
        {
            // If there's an error, return an empty list
            return new();
        }
        finally
        {
            conn.Close();
        }
    }
}
