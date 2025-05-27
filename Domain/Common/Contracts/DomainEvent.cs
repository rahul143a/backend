namespace Domain.Common.Contracts;

/// <summary>
/// Base class for domain events
/// </summary>
public abstract class DomainEvent
{
    /// <summary>
    /// Event ID
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Event timestamp
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
