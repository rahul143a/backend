namespace Abstraction.Identity.Dtos;

/// <summary>
/// Request for updating a user
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// User ID
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// First name
    /// </summary>
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Last name
    /// </summary>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Profile image URL
    /// </summary>
    public string? ImageUrl { get; set; }
}
