namespace Abstraction.Identity.Dtos;

/// <summary>
/// Response containing token information
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// JWT token
    /// </summary>
    public string Token { get; set; } = default!;

    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } = default!;

    /// <summary>
    /// Token expiration time in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = default!;

    /// <summary>
    /// User name
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// User roles
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Tenant ID
    /// </summary>
    public string TenantId { get; set; } = default!;

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; }
}
