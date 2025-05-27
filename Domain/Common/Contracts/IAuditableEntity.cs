namespace Domain.Common.Contracts;

/// <summary>
/// Interface for entities with auditing properties
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    DateTime CreatedOn { get; set; }

    /// <summary>
    /// ID of the user who created the entity
    /// </summary>
    Guid CreatedBy { get; set; }

    /// <summary>
    /// Date and time when the entity was last modified
    /// </summary>
    DateTime? LastModifiedOn { get; set; }

    /// <summary>
    /// ID of the user who last modified the entity
    /// </summary>
    Guid? LastModifiedBy { get; set; }
}
