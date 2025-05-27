using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common.Contracts;

/// <summary>
/// Base entity with GUID ID
/// </summary>
public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity() => Id = Guid.NewGuid();
}

/// <summary>
/// Base entity with generic ID type
/// </summary>
/// <typeparam name="TId">Type of the ID</typeparam>
public abstract class BaseEntity<TId> : IEntity<TId>, ISoftDelete
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public virtual TId Id { get; set; } = default!;

    /// <summary>
    /// Domain events
    /// </summary>
    [NotMapped]
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public List<DomainEvent> DomainEvents { get; } = new();

    /// <summary>
    /// Whether the entity is deleted (for soft delete)
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Date and time when the entity was deleted
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// ID of the user who deleted the entity
    /// </summary>
    public Guid? DeletedBy { get; set; }
}
