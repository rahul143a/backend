using Ardalis.Specification;
using Domain.Common.Contracts;

namespace Abstraction.Database.Repositories;

/// <summary>
/// Base repository interface
/// </summary>
public interface IRepositoryBase<T>
    where T : class, IAggregateRoot
{
    /// <summary>
    /// Add an entity
    /// </summary>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add multiple entities
    /// </summary>
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an entity
    /// </summary>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update multiple entities
    /// </summary>
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete an entity
    /// </summary>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete multiple entities
    /// </summary>
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete entities by specification
    /// </summary>
    Task DeleteRangeAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Save changes
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Base read repository interface
/// </summary>
public interface IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
    /// <summary>
    /// Get an entity by ID
    /// </summary>
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get an entity by specification
    /// </summary>
    Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get an entity by specification with projection
    /// </summary>
    Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the first entity by specification
    /// </summary>
    Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the first entity by specification with projection
    /// </summary>
    Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single entity by specification
    /// </summary>
    Task<T?> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single entity by specification with projection
    /// </summary>
    Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all entities
    /// </summary>
    Task<List<T>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get entities by specification
    /// </summary>
    Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get entities by specification with projection
    /// </summary>
    Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Count entities by specification
    /// </summary>
    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Count all entities
    /// </summary>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if any entity exists by specification
    /// </summary>
    Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if any entity exists
    /// </summary>
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get entities as async enumerable by specification
    /// </summary>
    IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification);
}

/// <summary>
/// The regular read/write repository for an aggregate root.
/// </summary>
public interface IRepository<T> : IRepositoryBase<T>, IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
}

/// <summary>
/// The read-only repository for an aggregate root.
/// </summary>
public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
}

/// <summary>
/// A special (read/write) repository for an aggregate root,
/// that also adds EntityCreated, EntityUpdated or EntityDeleted
/// events to the DomainEvents of the entities before adding,
/// updating or deleting them.
/// </summary>
public interface IRepositoryWithEvents<T> : IRepository<T>
    where T : class, IAggregateRoot
{
    /// <summary>
    /// Find entities by predicate
    /// </summary>
    Task<IEnumerable<T>> FindByAsync(Func<T, bool> predicate);

    /// <summary>
    /// Soft delete an entity
    /// </summary>
    Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft delete multiple entities
    /// </summary>
    Task SoftDeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
