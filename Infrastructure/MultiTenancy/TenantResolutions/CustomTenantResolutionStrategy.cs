using Finbuckle.MultiTenant;
using Abstraction.MultiTenancy;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.MultiTenancy.TenantResolutions;

/// <summary>
/// Custom tenant resolution strategy for API key-based resolution
/// </summary>
public class CustomTenantResolutionStrategy : IMultiTenantStrategy
{
    private readonly ITenantService _tenantService;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomTenantResolutionStrategy(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    /// <summary>
    /// Get tenant identifier from context
    /// </summary>
    public async Task<string?> GetIdentifierAsync(object context)
    {
        HttpContext? httpContext = context as HttpContext;
        if (httpContext == null || !httpContext.Request.Headers.TryGetValue("Api-Key", out var apiKeyHeader))
        {
            return null;
        }

        string? tenantId;
        var apiKey = apiKeyHeader.FirstOrDefault();
        if (string.IsNullOrEmpty(apiKey))
        {
            return null;
        }

        if (Guid.TryParse(apiKey, out Guid accountId))
        {
            tenantId = await _tenantService.GetTenantIdByApiKey(accountId);
        }
        else
        {
            try
            {
                var bytes = Convert.FromBase64String(apiKey);
                var idString = System.Text.Encoding.UTF8.GetString(bytes);
                accountId = Guid.Parse(idString);
                tenantId = await _tenantService.GetTenantIdByApiKey(accountId);
            }
            catch
            {
                return null;
            }
        }

        return tenantId;
    }
}
