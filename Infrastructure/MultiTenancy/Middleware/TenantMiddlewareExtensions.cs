using Microsoft.AspNetCore.Builder;

namespace Infrastructure.MultiTenancy.Middleware;

/// <summary>
/// Extensions for tenant middleware
/// </summary>
public static class TenantMiddlewareExtensions
{
    /// <summary>
    /// Use tenant middleware
    /// </summary>
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}
