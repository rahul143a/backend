using Abstraction.Identity;
using Abstraction.Identity.Dtos;
using Domain.Identity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

/// <summary>
/// Implementation of role service
/// </summary>
public class RoleService : IRoleService
{
    /// <summary>
    /// Role manager
    /// </summary>
    private readonly RoleManager<ApplicationRole> _roleManager;

    /// <summary>
    /// Constructor
    /// </summary>
    public RoleService(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <summary>
    /// Get a list of all roles
    /// </summary>
    public async Task<List<RoleDto>> GetListAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Adapt<List<RoleDto>>();
    }

    /// <summary>
    /// Get a role by ID
    /// </summary>
    public async Task<RoleDto?> GetByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        return role?.Adapt<RoleDto>();
    }

    /// <summary>
    /// Get a role by name
    /// </summary>
    public async Task<RoleDto?> GetByNameAsync(string name)
    {
        var role = await _roleManager.FindByNameAsync(name);
        return role?.Adapt<RoleDto>();
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    public async Task<string> CreateAsync(CreateRoleRequest request)
    {
        var role = await _roleManager.FindByNameAsync(request.Name);
        if (role != null)
        {
            throw new Exception($"Role {request.Name} already exists.");
        }

        role = new ApplicationRole(request.Name, request.Description)
        {
            CreatedOn = DateTime.UtcNow
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return $"Role {request.Name} created.";
    }

    /// <summary>
    /// Update a role
    /// </summary>
    public async Task<string> UpdateAsync(UpdateRoleRequest request)
    {
        var role = await _roleManager.FindByIdAsync(request.Id);
        if (role == null)
        {
            throw new Exception($"Role with ID {request.Id} not found.");
        }

        role.Name = request.Name;
        role.NormalizedName = request.Name.ToUpperInvariant();
        role.Description = request.Description;
        role.LastModifiedOn = DateTime.UtcNow;

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to update role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return $"Role {request.Name} updated.";
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    public async Task<string> DeleteAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            throw new Exception($"Role with ID {id} not found.");
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            throw new Exception($"Failed to delete role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return $"Role {role.Name} deleted.";
    }
}
