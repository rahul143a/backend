using Abstraction.Identity;
using Abstraction.Identity.Dtos;
using Application.Identity.Queries.GetUsersWithPagination;
using Shared.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.OpenApi;

namespace Web.Host.Controllers.Identity;

/// <summary>
/// Controller for user operations
/// </summary>
[Authorize]
public class UsersController : BaseApiController
{
    /// <summary>
    /// Identity service
    /// </summary>
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Mediator
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    public UsersController(IIdentityService identityService, IMediator mediator)
    {
        _identityService = identityService;
        _mediator = mediator;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    [TenantIdHeader]
    public async Task<ActionResult<Result<List<UserDetailsDto>>>> GetAllAsync()
    {
        try
        {
            var users = await _identityService.GetUsersAsync();
            return Ok(Result<List<UserDetailsDto>>.Success(users));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<List<UserDetailsDto>>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Get paginated users
    /// </summary>
    [HttpGet("paginated")]
    [TenantIdHeader]
    public async Task<ActionResult<PagedResponse<UserDetailsDto, object>>> GetPaginatedAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchString = null,
        [FromQuery] bool includeInactive = false)
    {
        try
        {
            var query = new GetUsersWithPaginationQuery
            {
                PageNumber = pageNumber < 1 ? 1 : pageNumber,
                PageSize = pageSize < 1 ? 10 : pageSize,
                SearchString = searchString,
                IncludeInactive = includeInactive
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(PagedResponse<UserDetailsDto, object>.Failure($"Error retrieving paginated users: {ex.Message}"));
        }
    }

    /// <summary>
    /// Get a user by ID
    /// </summary>
    [HttpGet("{id}")]
    [TenantIdHeader]
    public async Task<ActionResult<Result<UserDetailsDto>>> GetByIdAsync(string id)
    {
        try
        {
            var user = await _identityService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound(Result<UserDetailsDto>.Failure($"User with ID {id} not found."));
            }

            return Ok(Result<UserDetailsDto>.Success(user));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<UserDetailsDto>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [TenantIdHeader]
    public async Task<ActionResult<Result<string>>> RegisterAsync(RegisterUserRequest request)
    {
        try
        {
            var origin = Request.Headers["origin"].ToString();
            var result = await _identityService.RegisterUserAsync(request, origin);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Update a user
    /// </summary>
    [HttpPut("{id}")]
    [TenantIdHeader]
    public async Task<ActionResult<Result<string>>> UpdateAsync(UpdateUserRequest request, string id)
    {
        try
        {
            var result = await _identityService.UpdateUserAsync(request, id);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Change password
    /// </summary>
    [HttpPut("{id}/change-password")]
    [TenantIdHeader]
    public async Task<ActionResult<Result<string>>> ChangePasswordAsync(ChangePasswordRequest request, string id)
    {
        try
        {
            var result = await _identityService.ChangePasswordAsync(request, id);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Forgot password
    /// </summary>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [TenantIdHeader]
    public async Task<ActionResult<Result<string>>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        try
        {
            var origin = Request.Headers["origin"].ToString();
            var result = await _identityService.ForgotPasswordAsync(request, origin);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Reset password
    /// </summary>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    [TenantIdHeader]
    public async Task<ActionResult<Result<string>>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            var result = await _identityService.ResetPasswordAsync(request);
            return Ok(Result<string>.Success(result));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message));
        }
    }
}
