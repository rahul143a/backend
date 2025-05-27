using System.Security.Claims;

namespace Abstraction.Common;

/// <summary>
/// Interface for current user service
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Get the current user ID
    /// </summary>
    string? GetUserId();

    /// <summary>
    /// Get the current user email
    /// </summary>
    string? GetUserEmail();

    /// <summary>
    /// Get the current tenant ID
    /// </summary>
    string? GetTenant();

    /// <summary>
    /// Get the current user name
    /// </summary>
    string? GetName();

    /// <summary>
    /// Check if the current user is in the specified role
    /// </summary>
    bool IsInRole(string role);
}

/// <summary>
/// Interface for initializing the current user
/// </summary>
public interface ICurrentUserInitializer
{
    /// <summary>
    /// Set the current user
    /// </summary>
    void SetCurrentUser(ClaimsPrincipal principal);

    /// <summary>
    /// Set the current tenant
    /// </summary>
    void SetCurrentTenant(string tenantId);
}
