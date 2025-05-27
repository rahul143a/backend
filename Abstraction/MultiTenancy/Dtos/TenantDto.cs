namespace Abstraction.MultiTenancy.Dtos;

/// <summary>
/// Data transfer object for tenant information
/// </summary>
public class TenantDto
{
    /// <summary>
    /// Tenant ID
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Tenant name
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Tenant-specific connection string (if any)
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Admin email for the tenant
    /// </summary>
    public string AdminEmail { get; set; } = default!;

    /// <summary>
    /// Whether the tenant is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Tenant validity date
    /// </summary>
    public DateTime ValidUpto { get; set; }

    /// <summary>
    /// JWT issuer for the tenant
    /// </summary>
    public string? Issuer { get; set; }
}
