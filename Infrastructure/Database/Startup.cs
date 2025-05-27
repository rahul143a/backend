using Finbuckle.MultiTenant;
using Abstraction.Database.Repositories;
using Domain.MultiTenancy;
using Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.Database;

/// <summary>
/// Startup configuration for persistence
/// </summary>
public static class Startup
{
    /// <summary>
    /// Add persistence services
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var tenantInfoAccessor = serviceProvider.GetRequiredService<IMultiTenantContextAccessor<AppTenantInfo>>();
                var connection = tenantInfoAccessor.MultiTenantContext?.TenantInfo?.ConnectionString;

                var connectionString = string.IsNullOrEmpty(connection) ? DatabaseSettings.ConnectionString : connection;

                // Apply EnableDynamicJson() properly
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString)
                    .EnableDynamicJson();
                var dataSource = dataSourceBuilder.Build();

                // Use the dataSource properly
                options.UseNpgsql(dataSource, dbOptions =>
                {
                    dbOptions.MigrationsAssembly("Inventory.Migrators");
                });
            }, ServiceLifetime.Scoped)
            .AddScoped<IDatabaseInitializer, DatabaseInitializer>()
            .AddRepositories();
    }

    /// <summary>
    /// Add repositories
    /// </summary>
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Add Repositories
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepositoryBase<>));
        services.AddScoped(typeof(IRepositoryWithEvents<>), typeof(RepositoryWithEventsBase<>));
        services.AddTransient<IDapperRepository, DapperRepository>();

        return services;
    }

    /// <summary>
    /// Use database
    /// </summary>
    public static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string? connectionString)
    {
        switch (dbProvider.ToLowerInvariant())
        {
            case "postgresql":
                return builder.UseNpgsql(connectionString, e =>
                     e.MigrationsAssembly("Inventory.Migrators"));

            default:
                throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
        }
    }
}
