namespace Domain.Common.Contracts;

/// <summary>
/// Interface for entities that support soft delete
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Whether the entity is deleted
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Date and time when the entity was deleted
    /// </summary>
    DateTime? DeletedOn { get; set; }

    /// <summary>
    /// ID of the user who deleted the entity
    /// </summary>
    Guid? DeletedBy { get; set; }
}
