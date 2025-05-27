namespace Abstraction.Identity.Dtos;

/// <summary>
/// Request for forgot password
/// </summary>
public class ForgotPasswordRequest
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = default!;
}
