namespace Abstraction.Identity.Dtos;

/// <summary>
/// Data transfer object for user roles
/// </summary>
public class UserRolesDto
{
    /// <summary>
    /// User ID
    /// </summary>
    public string UserId { get; set; } = default!;

    /// <summary>
    /// Roles
    /// </summary>
    public List<string> Roles { get; set; } = new();
}
