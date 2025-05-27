using Finbuckle.MultiTenant;
using Abstraction.Identity;
using Abstraction.Identity.Dtos;
using Domain.Identity;
using Shared.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity;

/// <summary>
/// Implementation of token service
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    /// User manager
    /// </summary>
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// JWT settings
    /// </summary>
    private readonly JwtSettings _jwtSettings;

    /// <summary>
    /// Current tenant
    /// </summary>
    private readonly ITenantInfo _currentTenant;

    /// <summary>
    /// Constructor
    /// </summary>
    public TokenService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        ITenantInfo currentTenant)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _currentTenant = currentTenant;
    }

    /// <summary>
    /// Get a token for the specified request
    /// </summary>
    public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress)
    {
        var user = await _userManager.FindByNameAsync(request.UsernameOrEmail) ??
                   await _userManager.FindByEmailAsync(request.UsernameOrEmail);

        if (user == null)
        {
            throw new Exception($"User with username or email {request.UsernameOrEmail} not found.");
        }

        if (!user.IsActive)
        {
            throw new Exception($"User {request.UsernameOrEmail} is not active.");
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new Exception($"Invalid credentials for user {request.UsernameOrEmail}.");
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    /// <summary>
    /// Refresh a token
    /// </summary>
    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user == null)
        {
            throw new Exception($"User with email {userEmail} not found.");
        }

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new Exception("Invalid or expired refresh token.");
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    /// <summary>
    /// Generate tokens and update user
    /// </summary>
    private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
    {
        var token = await GenerateJwtAsync(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        return new TokenResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresIn = _jwtSettings.ExpirationInMinutes * 60,
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Roles = roles.ToList(),
            TenantId = _currentTenant?.Id ?? string.Empty,
            IsActive = user.IsActive
        };
    }

    /// <summary>
    /// Generate a JWT token
    /// </summary>
    private async Task<string> GenerateJwtAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(MultitenancyConstants.TenantIdKey, _currentTenant?.Id ?? string.Empty)
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    /// <summary>
    /// Generate a refresh token
    /// </summary>
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Get principal from expired token
    /// </summary>
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new Exception("Invalid token.");
        }

        return principal;
    }
}
