using Finbuckle.MultiTenant;

namespace Domain.MultiTenancy;

/// <summary>
/// Application tenant information
/// </summary>
public class AppTenantInfo : ITenantInfo
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public AppTenantInfo()
    {
    }

    /// <summary>
    /// Constructor with tenant details
    /// </summary>
    public AppTenantInfo(string id, string name, string? connectionString, string adminEmail, string? issuer = null, DateTime? validUpto = null)
    {
        Id = id;
        Identifier = id;
        Name = name;
        ConnectionString = connectionString ?? string.Empty;
        AdminEmail = adminEmail;
        IsActive = true;
        Issuer = issuer;
        // Add Default 1 Month Validity for all new tenants
        ValidUpto = validUpto ?? DateTime.UtcNow.AddMonths(1);
    }

    /// <summary>
    /// Created by user ID
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Created on date
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Last modified by user ID
    /// </summary>
    public string? LastModifiedBy { get; set; }

    /// <summary>
    /// Last modified on date
    /// </summary>
    public DateTime? LastModifiedOn { get; set; }

    /// <summary>
    /// Tenant ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Tenant identifier (same as ID)
    /// </summary>
    public string? Identifier { get; set; }

    /// <summary>
    /// Tenant name
    /// </summary>
    public string? Name { get; set; }

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
    public DateTime? ValidUpto { get; set; }

    /// <summary>
    /// JWT issuer for the tenant
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// Resolution keys for tenant resolution strategies
    /// </summary>
    public string[]? ResolutionKeys { get; set; }
}
