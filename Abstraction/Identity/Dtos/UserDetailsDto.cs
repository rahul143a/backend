namespace Abstraction.Identity.Dtos;

/// <summary>
/// Data transfer object for user details
/// </summary>
public class UserDetailsDto
{
    /// <summary>
    /// User ID
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// User name
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// First name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Whether the email is confirmed
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Whether the phone number is confirmed
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// Profile image URL
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// User roles
    /// </summary>
    public List<string> Roles { get; set; } = new();
}
