namespace Abstraction.Identity.Dtos;

/// <summary>
/// Request for resetting password
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// Confirm password
    /// </summary>
    public string ConfirmPassword { get; set; } = default!;

    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } = default!;
}
