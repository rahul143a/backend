using Abstraction.Identity.Dtos;
using Shared.Common;

namespace Abstraction.Identity;

/// <summary>
/// Interface for role service
/// </summary>
public interface IRoleService : ITransientService
{
    /// <summary>
    /// Get a list of all roles
    /// </summary>
    Task<List<RoleDto>> GetListAsync();

    /// <summary>
    /// Get a role by ID
    /// </summary>
    Task<RoleDto?> GetByIdAsync(string id);

    /// <summary>
    /// Get a role by name
    /// </summary>
    Task<RoleDto?> GetByNameAsync(string name);

    /// <summary>
    /// Create a new role
    /// </summary>
    Task<string> CreateAsync(CreateRoleRequest request);

    /// <summary>
    /// Update a role
    /// </summary>
    Task<string> UpdateAsync(UpdateRoleRequest request);

    /// <summary>
    /// Delete a role
    /// </summary>
    Task<string> DeleteAsync(string id);
}
