using Abstraction.Identity;
using Abstraction.Identity.Dtos;
using Shared.Common.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.OpenApi;

namespace Web.Host.Controllers.Identity;

/// <summary>
/// Controller for token operations
/// </summary>
public class TokensController : BaseApiController
{
    /// <summary>
    /// Token service
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Constructor
    /// </summary>
    public TokensController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// Get a token
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [TenantIdHeader]
    public async Task<ActionResult<Result<TokenResponse>>> GetTokenAsync(TokenRequest request)
    {
        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var token = await _tokenService.GetTokenAsync(request, ipAddress);
            return Ok(Result<TokenResponse>.Success(token));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<TokenResponse>.Failure(ex.Message));
        }
    }

    /// <summary>
    /// Refresh a token
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [TenantIdHeader]
    public async Task<ActionResult<Result<TokenResponse>>> RefreshAsync(RefreshTokenRequest request)
    {
        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var token = await _tokenService.RefreshTokenAsync(request, ipAddress);
            return Ok(Result<TokenResponse>.Success(token));
        }
        catch (Exception ex)
        {
            return BadRequest(Result<TokenResponse>.Failure(ex.Message));
        }
    }
}
