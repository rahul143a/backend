using Abstraction.Common;
using Shared.MultiTenancy;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Common;

/// <summary>
/// Implementation of current user service
/// </summary>
public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    /// <summary>
    /// User ID
    /// </summary>
    private string? _userId;

    /// <summary>
    /// User name
    /// </summary>
    private string? _name;

    /// <summary>
    /// Tenant ID
    /// </summary>
    private string? _tenantId;

    /// <summary>
    /// HTTP context accessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Constructor
    /// </summary>
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Get the current user ID
    /// </summary>
    public string? GetUserId()
    {
        if (_userId != null)
        {
            return _userId;
        }

        _userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return _userId;
    }

    /// <summary>
    /// Get the current user email
    /// </summary>
    public string? GetUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
    }

    /// <summary>
    /// Get the current tenant ID
    /// </summary>
    public string? GetTenant()
    {
        if (_tenantId != null)
        {
            return _tenantId;
        }

        _tenantId = _httpContextAccessor.HttpContext?.User.FindFirstValue(MultitenancyConstants.TenantIdKey);
        return _tenantId;
    }

    /// <summary>
    /// Get the current user name
    /// </summary>
    public string? GetName()
    {
        if (_name != null)
        {
            return _name;
        }

        _name = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        return _name;
    }

    /// <summary>
    /// Check if the current user is in the specified role
    /// </summary>
    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
    }

    /// <summary>
    /// Set the current user
    /// </summary>
    public void SetCurrentUser(ClaimsPrincipal principal)
    {
        _userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        _name = principal.FindFirstValue(ClaimTypes.Name);
        _tenantId = principal.FindFirstValue(MultitenancyConstants.TenantIdKey);
    }

    /// <summary>
    /// Set the current tenant
    /// </summary>
    public void SetCurrentTenant(string tenantId)
    {
        _tenantId = tenantId;
    }
}
