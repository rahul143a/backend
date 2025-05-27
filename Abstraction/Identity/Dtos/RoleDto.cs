namespace Abstraction.Identity.Dtos;

/// <summary>
/// Data transfer object for role information
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Role ID
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Role name
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Role description
    /// </summary>
    public string? Description { get; set; }
}
