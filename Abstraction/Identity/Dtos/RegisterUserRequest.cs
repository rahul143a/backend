namespace Abstraction.Identity.Dtos;

/// <summary>
/// Request for registering a new user
/// </summary>
public class RegisterUserRequest
{
    /// <summary>
    /// First name
    /// </summary>
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Last name
    /// </summary>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// User name
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// Confirm password
    /// </summary>
    public string ConfirmPassword { get; set; } = default!;

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }
}
