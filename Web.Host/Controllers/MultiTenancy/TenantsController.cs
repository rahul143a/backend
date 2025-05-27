using Abstraction.MultiTenancy;
using Abstraction.MultiTenancy.Dtos;
using Shared.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Host.Controllers.MultiTenancy;

/// <summary>
/// Controller for tenant operations
/// </summary>
public class TenantsController : BaseApiController
{
    /// <summary>
    /// Tenant service
    /// </summary>
    private readonly ITenantService _tenantService;

    /// <summary>
    /// Constructor
    /// </summary>
    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    /// <summary>
    /// Get all tenants
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<Result<List<TenantDto>>>> GetAllAsync()
    {
        try
        {
            var tenants = await _tenantService.GetAllAsync();
            return Ok(Result<List<TenantDto>>.Success(tenants));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<List<TenantDto>>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Get a tenant by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Result<TenantDto>>> GetByIdAsync(string id)
    {
        try
        {
            var tenant = await _tenantService.GetByIdAsync(id);
            if (tenant == null)
            {
                return NotFound(Result<TenantDto>.Failure($"Tenant with ID {id} not found"));
            }
            return Ok(Result<TenantDto>.Success(tenant));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<TenantDto>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Create a new tenant
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Result<string>>> CreateAsync(TenantDto request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _tenantService.CreateAsync(request, cancellationToken);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Update a tenant
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<Result<string>>> UpdateAsync(TenantDto request)
    {
        try
        {
            var result = await _tenantService.UpdateAsync(request);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Activate a tenant
    /// </summary>
    [HttpPost("{id}/activate")]
    public async Task<ActionResult<Result<string>>> ActivateAsync(string id)
    {
        try
        {
            var result = await _tenantService.ActivateAsync(id);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Deactivate a tenant
    /// </summary>
    [HttpPost("{id}/deactivate")]
    public async Task<ActionResult<Result<string>>> DeactivateAsync(string id)
    {
        try
        {
            var result = await _tenantService.DeactivateAsync(id);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }
}
