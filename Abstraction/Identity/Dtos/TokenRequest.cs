namespace Abstraction.Identity.Dtos;

/// <summary>
/// Request for obtaining a token
/// </summary>
public class TokenRequest
{
    /// <summary>
    /// Username or email
    /// </summary>
    public string UsernameOrEmail { get; set; } = default!;

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = default!;
}
