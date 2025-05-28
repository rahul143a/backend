using Abstraction.Identity;
using Abstraction.Identity.Dtos;
using Shared.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.OpenApi;

namespace Web.Host.Controllers.Identity;

/// <summary>
/// Controller for role operations
/// </summary>
[Authorize]
public class RolesController : BaseApiController
{
    /// <summary>
    /// Role service
    /// </summary>
    private readonly IRoleService _roleService;

    /// <summary>
    /// Constructor
    /// </summary>
    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet]
    [TenantIdHeader]
    public async Task<ActionResult<Result<List<RoleDto>>>> GetAllAsync()
    {
        try
        {
            var roles = await _roleService.GetListAsync();
            return Ok(Result<List<RoleDto>>.Success(roles));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<List<RoleDto>>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Get a role by ID
    /// </summary>
    [HttpGet("{id}")]
    [TenantIdHeader]
    public async Task<ActionResult<Result<RoleDto>>> GetByIdAsync(string id)
    {
        try
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
            {
                return NotFound(Result<RoleDto>.Failure($"Role with ID {id} not found."));
            }

            return Ok(Result<RoleDto>.Success(role));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<RoleDto>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    [HttpPost]
    [TenantIdHeader]
    public async Task<ActionResult<Result<string>>> CreateAsync(CreateRoleRequest request)
    {
        try
        {
            var result = await _roleService.CreateAsync(request);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Update a role
    /// </summary>
    [HttpPut]
    [TenantIdHeader]
    public async Task<ActionResult<Result<string>>> UpdateAsync(UpdateRoleRequest request)
    {
        try
        {
            var result = await _roleService.UpdateAsync(request);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    [HttpDelete("{id}")]
    [TenantIdHeader]
    public async Task<ActionResult<Result<string>>> DeleteAsync(string id)
    {
        try
        {
            var result = await _roleService.DeleteAsync(id);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }
}
