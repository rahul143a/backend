using Finbuckle.MultiTenant;

namespace Infrastructure.Database;

/// <summary>
/// Interface for tenant database context
/// </summary>
public interface ITenantDbContext
{
    /// <summary>
    /// Tenant ID
    /// </summary>
    string? TenantId { get; }
}
