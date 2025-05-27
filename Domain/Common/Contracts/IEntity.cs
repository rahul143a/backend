namespace Domain.Common.Contracts;

/// <summary>
/// Interface for entities with domain events
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Domain events
    /// </summary>
    List<DomainEvent> DomainEvents { get; }
}

/// <summary>
/// Interface for entities with a specific ID type
/// </summary>
/// <typeparam name="TId">Type of the ID</typeparam>
public interface IEntity<TId> : IEntity
{
    /// <summary>
    /// Entity ID
    /// </summary>
    TId Id { get; set; }
}
