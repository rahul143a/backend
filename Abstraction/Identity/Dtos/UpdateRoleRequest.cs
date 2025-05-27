namespace Abstraction.Identity.Dtos;

/// <summary>
/// Request for updating a role
/// </summary>
public class UpdateRoleRequest
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
