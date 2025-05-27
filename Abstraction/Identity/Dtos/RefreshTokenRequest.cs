namespace Abstraction.Identity.Dtos;

/// <summary>
/// Request for refreshing a token
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// JWT token
    /// </summary>
    public string Token { get; set; } = default!;

    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}
