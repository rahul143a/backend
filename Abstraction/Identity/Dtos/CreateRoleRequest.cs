namespace Abstraction.Identity.Dtos;

/// <summary>
/// Request for creating a new role
/// </summary>
public class CreateRoleRequest
{
    /// <summary>
    /// Role name
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Role description
    /// </summary>
    public string? Description { get; set; }
}
