using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Stores;
using Abstraction.MultiTenancy;
using Abstraction.MultiTenancy.Dtos;
using Domain.MultiTenancy;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Infrastructure.MultiTenancy;

/// <summary>
/// Implementation of tenant service
/// </summary>
public class TenantService : ITenantService
{
    /// <summary>
    /// Tenant store
    /// </summary>
    private readonly IMultiTenantStore<AppTenantInfo> _tenantStore;

    /// <summary>
    /// Database settings
    /// </summary>
    private readonly DatabaseSettings _dbSettings;

    /// <summary>
    /// Tenant database context
    /// </summary>
    private readonly TenantDbContext _tenantDbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    public TenantService(
        IMultiTenantStore<AppTenantInfo> tenantStore,
        IOptions<DatabaseSettings> dbSettings,
        TenantDbContext tenantDbContext)
    {
        _tenantStore = tenantStore;
        _dbSettings = dbSettings.Value;
        _tenantDbContext = tenantDbContext;
    }

    /// <summary>
    /// Get all tenants
    /// </summary>
    public async Task<List<TenantDto>> GetAllAsync()
    {
        var tenants = await _tenantStore.GetAllAsync();
        return tenants.Adapt<List<TenantDto>>();
    }

    /// <summary>
    /// Check if a tenant exists with the given ID
    /// </summary>
    public async Task<bool> ExistsWithIdAsync(string id)
    {
        return await _tenantStore.TryGetAsync(id) != null;
    }

    /// <summary>
    /// Check if a tenant already exists with the given ID
    /// </summary>
    public async Task<bool> IsTenantAlreadyExistsAsync(string id, CancellationToken cancellationToken)
    {
        return await _tenantStore.TryGetAsync(id) != null;
    }

    /// <summary>
    /// Check if a tenant exists with the given name
    /// </summary>
    public async Task<bool> ExistsWithNameAsync(string name)
    {
        var tenants = await _tenantStore.GetAllAsync();
        return tenants.Any(t => t.Name == name);
    }

    /// <summary>
    /// Get a tenant by ID
    /// </summary>
    public async Task<TenantDto?> GetByIdAsync(string id)
    {
        var tenant = await _tenantStore.TryGetAsync(id);
        if (tenant == null)
        {
            return null;
        }

        return tenant.Adapt<TenantDto>();
    }

    /// <summary>
    /// Create a new tenant
    /// </summary>
    public async Task<string> CreateAsync(TenantDto request, CancellationToken cancellationToken)
    {
        if (await ExistsWithIdAsync(request.Id))
        {
            throw new Exception($"Tenant {request.Id} already exists.");
        }

        if (await ExistsWithNameAsync(request.Name))
        {
            throw new Exception($"Tenant {request.Name} already exists.");
        }

        var tenant = new AppTenantInfo(
            request.Id,
            request.Name,
            request.ConnectionString,
            request.AdminEmail,
            request.Issuer);

        // Add to in-memory store
        var storeResult = await _tenantStore.TryAddAsync(tenant);

        if (storeResult)
        {
            try
            {
                // Add to database
                _tenantDbContext.TenantInfo.Add(tenant);
                await _tenantDbContext.SaveChangesAsync(cancellationToken);

                return $"Tenant {request.Id} created.";
            }
            catch (Exception ex)
            {
                // If database save fails, remove from in-memory store
                await _tenantStore.TryRemoveAsync(request.Id);
                throw new Exception($"Failed to create tenant in database: {ex.Message}");
            }
        }

        throw new Exception($"Failed to add tenant {request.Id} to store.");
    }

    /// <summary>
    /// Activate a tenant
    /// </summary>
    public async Task<string> ActivateAsync(string id)
    {
        var tenant = await _tenantStore.TryGetAsync(id);
        if (tenant == null)
        {
            return $"Tenant {id} not found.";
        }

        if (tenant.IsActive)
        {
            throw new Exception($"Tenant {id} is already active.");
        }

        tenant.IsActive = true;

        // Update in-memory store
        var storeResult = await _tenantStore.TryUpdateAsync(tenant);

        if (storeResult)
        {
            try
            {
                // Update in database
                var dbTenant = await _tenantDbContext.TenantInfo.FindAsync(id);
                if (dbTenant != null)
                {
                    dbTenant.IsActive = true;
                    _tenantDbContext.TenantInfo.Update(dbTenant);
                    await _tenantDbContext.SaveChangesAsync();
                }

                return $"Tenant {id} activated.";
            }
            catch (Exception ex)
            {
                // If database update fails, revert in-memory store
                tenant.IsActive = false;
                await _tenantStore.TryUpdateAsync(tenant);
                throw new Exception($"Failed to activate tenant in database: {ex.Message}");
            }
        }

        throw new Exception($"Failed to activate tenant {id} in store.");
    }

    /// <summary>
    /// Deactivate a tenant
    /// </summary>
    public async Task<string> DeactivateAsync(string id)
    {
        var tenant = await _tenantStore.TryGetAsync(id);
        if (tenant == null)
        {
            return $"Tenant {id} not found.";
        }

        if (!tenant.IsActive)
        {
            throw new Exception($"Tenant {id} is already deactivated.");
        }

        tenant.IsActive = false;

        // Update in-memory store
        var storeResult = await _tenantStore.TryUpdateAsync(tenant);

        if (storeResult)
        {
            try
            {
                // Update in database
                var dbTenant = await _tenantDbContext.TenantInfo.FindAsync(id);
                if (dbTenant != null)
                {
                    dbTenant.IsActive = false;
                    _tenantDbContext.TenantInfo.Update(dbTenant);
                    await _tenantDbContext.SaveChangesAsync();
                }

                return $"Tenant {id} deactivated.";
            }
            catch (Exception ex)
            {
                // If database update fails, revert in-memory store
                tenant.IsActive = true;
                await _tenantStore.TryUpdateAsync(tenant);
                throw new Exception($"Failed to deactivate tenant in database: {ex.Message}");
            }
        }

        throw new Exception($"Failed to deactivate tenant {id} in store.");
    }

    /// <summary>
    /// Update a tenant
    /// </summary>
    public async Task<string> UpdateAsync(TenantDto request)
    {
        var tenant = await _tenantStore.TryGetAsync(request.Id);
        if (tenant == null)
        {
            return $"Tenant {request.Id} not found.";
        }

        var tenants = await _tenantStore.GetAllAsync();
        if (tenants.Any(t => t.Id != request.Id && t.Name == request.Name))
        {
            throw new Exception($"Tenant {request.Name} already exists.");
        }

        // Save original values in case we need to revert
        var originalName = tenant.Name;
        var originalConnectionString = tenant.ConnectionString;
        var originalAdminEmail = tenant.AdminEmail;
        var originalIsActive = tenant.IsActive;
        var originalValidUpto = tenant.ValidUpto;
        var originalIssuer = tenant.Issuer;

        // Update tenant properties
        tenant.Name = request.Name;
        tenant.ConnectionString = request.ConnectionString ?? tenant.ConnectionString;
        tenant.AdminEmail = request.AdminEmail;
        tenant.IsActive = request.IsActive;
        tenant.ValidUpto = request.ValidUpto;
        tenant.Issuer = request.Issuer;

        // Update in-memory store
        var storeResult = await _tenantStore.TryUpdateAsync(tenant);

        if (storeResult)
        {
            try
            {
                // Update in database
                var dbTenant = await _tenantDbContext.TenantInfo.FindAsync(request.Id);
                if (dbTenant != null)
                {
                    dbTenant.Name = request.Name;
                    dbTenant.ConnectionString = request.ConnectionString ?? dbTenant.ConnectionString;
                    dbTenant.AdminEmail = request.AdminEmail;
                    dbTenant.IsActive = request.IsActive;
                    dbTenant.ValidUpto = request.ValidUpto;
                    dbTenant.Issuer = request.Issuer;

                    _tenantDbContext.TenantInfo.Update(dbTenant);
                    await _tenantDbContext.SaveChangesAsync();
                }

                return $"Tenant {request.Id} updated.";
            }
            catch (Exception ex)
            {
                // If database update fails, revert in-memory store
                tenant.Name = originalName;
                tenant.ConnectionString = originalConnectionString;
                tenant.AdminEmail = originalAdminEmail;
                tenant.IsActive = originalIsActive;
                tenant.ValidUpto = originalValidUpto;
                tenant.Issuer = originalIssuer;

                await _tenantStore.TryUpdateAsync(tenant);
                throw new Exception($"Failed to update tenant in database: {ex.Message}");
            }
        }

        throw new Exception($"Failed to update tenant {request.Id} in store.");
    }

    /// <summary>
    /// Get tenant ID by API key
    /// </summary>
    public Task<string?> GetTenantIdByApiKey(Guid apiKey)
    {
        // TODO: Implement API key-based tenant resolution
        return Task.FromResult<string?>(null);
    }
}
