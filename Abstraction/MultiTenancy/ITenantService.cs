using Abstraction.MultiTenancy.Dtos;
using Shared.Common;

namespace Abstraction.MultiTenancy;

/// <summary>
/// Interface for tenant service
/// </summary>
public interface ITenantService : ITransientService
{
    /// <summary>
    /// Get all tenants
    /// </summary>
    Task<List<TenantDto>> GetAllAsync();

    /// <summary>
    /// Check if a tenant exists with the given ID
    /// </summary>
    Task<bool> ExistsWithIdAsync(string id);

    /// <summary>
    /// Check if a tenant already exists with the given ID
    /// </summary>
    Task<bool> IsTenantAlreadyExistsAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Check if a tenant exists with the given name
    /// </summary>
    Task<bool> ExistsWithNameAsync(string name);

    /// <summary>
    /// Get a tenant by ID
    /// </summary>
    Task<TenantDto?> GetByIdAsync(string id);

    /// <summary>
    /// Create a new tenant
    /// </summary>
    Task<string> CreateAsync(TenantDto request, CancellationToken cancellationToken);

    /// <summary>
    /// Activate a tenant
    /// </summary>
    Task<string> ActivateAsync(string id);

    /// <summary>
    /// Deactivate a tenant
    /// </summary>
    Task<string> DeactivateAsync(string id);

    /// <summary>
    /// Update a tenant
    /// </summary>
    Task<string> UpdateAsync(TenantDto request);

    /// <summary>
    /// Get tenant ID by API key
    /// </summary>
    Task<string?> GetTenantIdByApiKey(Guid apiKey);
}
