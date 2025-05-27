using Finbuckle.MultiTenant;
using Domain.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database;

/// <summary>
/// Database initializer interface
/// </summary>
public interface IDatabaseInitializer
{
    /// <summary>
    /// Initialize the database
    /// </summary>
    Task InitializeDatabasesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Initialize the application database for a tenant
    /// </summary>
    Task InitializeApplicationDbForTenantAsync(AppTenantInfo tenant, CancellationToken cancellationToken = default);
}

/// <summary>
/// Database initializer implementation
/// </summary>
public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Initialize the database
    /// </summary>
    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a new scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();

            // Get the tenant DB context
            var tenantDbContext = scope.ServiceProvider.GetRequiredService<MultiTenancy.TenantDbContext>();

            // Check for pending migrations
            var pendingMigrations = await tenantDbContext.Database.GetPendingMigrationsAsync(cancellationToken);
            if (pendingMigrations.Any())
            {
                _logger.LogInformation("Applying tenant database migrations...");
                await tenantDbContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Tenant database migrated successfully.");
            }
            else
            {
                _logger.LogInformation("No pending tenant database migrations.");
            }

            // Get all the tenants
            var tenants = await tenantDbContext.TenantInfo.ToListAsync(cancellationToken);

            // Migrate the application database for each tenant
            foreach (var tenant in tenants)
            {
                await InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the databases.");
            throw;
        }
    }

    /// <summary>
    /// Initialize the application database for a tenant
    /// </summary>
    public async Task InitializeApplicationDbForTenantAsync(AppTenantInfo tenant, CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a new scope
            using var scope = _serviceProvider.CreateScope();

            // Get the multi-tenant context accessor
            var tenantContextAccessor = scope.ServiceProvider.GetRequiredService<IMultiTenantContextAccessor<AppTenantInfo>>();

            // Create a tenant context for the specified tenant
            var tenantContext = new MultiTenantContext<AppTenantInfo>
            {
                TenantInfo = tenant
            };

            // Set the tenant context
            tenantContextAccessor.MultiTenantContext = tenantContext;

            // Get the application DB context
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check for pending migrations
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
            if (pendingMigrations.Any())
            {
                _logger.LogInformation("Applying application database migrations for tenant {TenantId}...", tenant.Id);
                await dbContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Application database for tenant {TenantId} migrated successfully.", tenant.Id);
            }
            else
            {
                _logger.LogInformation("No pending application database migrations for tenant {TenantId}.", tenant.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the application database for tenant {TenantId}.", tenant.Id);
            throw;
        }
    }
}
