using Domain.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

/// <summary>
/// Application role entity
/// </summary>
public class ApplicationRole : IdentityRole, IAggregateRoot
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public ApplicationRole()
    {
    }

    /// <summary>
    /// Constructor with role name and description
    /// </summary>
    public ApplicationRole(string name, string? description = null)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }

    /// <summary>
    /// Role description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// When the role was last modified
    /// </summary>
    public DateTime? LastModifiedOn { get; set; }

    /// <summary>
    /// When the role was created
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// ID of the user who created this role
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// ID of the user who last modified this role
    /// </summary>
    public Guid LastModifiedBy { get; set; }

    /// <summary>
    /// Whether the role is deleted (soft delete)
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// When the role was deleted
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// ID of the user who deleted this role
    /// </summary>
    public Guid? DeletedBy { get; set; }
}
