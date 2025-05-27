using Shared.MultiTenancy;

namespace Infrastructure.OpenApi;

/// <summary>
/// Attribute to add tenant ID header to Swagger documentation
/// </summary>
public class TenantIdHeaderAttribute : SwaggerHeaderAttribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    public TenantIdHeaderAttribute()
        : base(
            MultitenancyConstants.TenantIdName,
            "Input your tenant Id to access this API",
            string.Empty,
            true)
    {
    }
}
