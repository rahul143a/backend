namespace Domain.Common.Contracts;

/// <summary>
/// Base entity with auditing properties
/// </summary>
public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// ID of the user who created the entity
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Date and time when the entity was last modified
    /// </summary>
    public DateTime? LastModifiedOn { get; set; }

    /// <summary>
    /// ID of the user who last modified the entity
    /// </summary>
    public Guid? LastModifiedBy { get; set; }
}

/// <summary>
/// Base entity with auditing properties and generic ID type
/// </summary>
/// <typeparam name="TId">Type of the ID</typeparam>
public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity
{
    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// ID of the user who created the entity
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Date and time when the entity was last modified
    /// </summary>
    public DateTime? LastModifiedOn { get; set; }

    /// <summary>
    /// ID of the user who last modified the entity
    /// </summary>
    public Guid? LastModifiedBy { get; set; }
}
