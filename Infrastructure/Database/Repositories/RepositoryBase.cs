using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Abstraction.Database.Repositories;
using Domain.Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories;

/// <summary>
/// Base repository implementation
/// </summary>
public class RepositoryBase<T> : IRepository<T>
    where T : class, IAggregateRoot
{
    /// <summary>
    /// Database context
    /// </summary>
    protected readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    public RepositoryBase(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Add an entity
    /// </summary>
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// Add multiple entities
    /// </summary>
    public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entities;
    }

    /// <summary>
    /// Update an entity
    /// </summary>
    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Update multiple entities
    /// </summary>
    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Delete an entity
    /// </summary>
    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Delete multiple entities
    /// </summary>
    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Delete entities by specification
    /// </summary>
    public virtual async Task DeleteRangeAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var entities = await ListAsync(specification, cancellationToken);
        await DeleteRangeAsync(entities, cancellationToken);
    }

    /// <summary>
    /// Get an entity by ID
    /// </summary>
    public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    /// <summary>
    /// Get an entity by specification
    /// </summary>
    public virtual async Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get an entity by specification with projection
    /// </summary>
    public virtual async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get the first entity by specification
    /// </summary>
    public virtual async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get the first entity by specification with projection
    /// </summary>
    public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get a single entity by specification
    /// </summary>
    public virtual async Task<T?> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get a single entity by specification with projection
    /// </summary>
    public virtual async Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get all entities
    /// </summary>
    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Get entities by specification
    /// </summary>
    public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Get entities by specification with projection
    /// </summary>
    public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Count entities by specification
    /// </summary>
    public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).CountAsync(cancellationToken);
    }

    /// <summary>
    /// Count all entities
    /// </summary>
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().CountAsync(cancellationToken);
    }

    /// <summary>
    /// Check if any entity exists by specification
    /// </summary>
    public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Check if any entity exists
    /// </summary>
    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Get entities as async enumerable by specification
    /// </summary>
    public virtual IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
    {
        return ApplySpecification(specification).AsAsyncEnumerable();
    }

    /// <summary>
    /// Save changes
    /// </summary>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Apply specification to query
    /// </summary>
    protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        var evaluator = new SpecificationEvaluator();
        return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
    }

    /// <summary>
    /// Apply specification to query with projection
    /// </summary>
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        var evaluator = new SpecificationEvaluator();
        return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
    }
}

/// <summary>
/// Read-only repository implementation
/// </summary>
public class ReadRepositoryBase<T> : IReadRepository<T>
    where T : class, IAggregateRoot
{
    /// <summary>
    /// Database context
    /// </summary>
    protected readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    public ReadRepositoryBase(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Get an entity by ID
    /// </summary>
    public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    /// <summary>
    /// Get an entity by specification
    /// </summary>
    public virtual async Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get an entity by specification with projection
    /// </summary>
    public virtual async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get the first entity by specification
    /// </summary>
    public virtual async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get the first entity by specification with projection
    /// </summary>
    public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get a single entity by specification
    /// </summary>
    public virtual async Task<T?> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get a single entity by specification with projection
    /// </summary>
    public virtual async Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Get all entities
    /// </summary>
    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Get entities by specification
    /// </summary>
    public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Get entities by specification with projection
    /// </summary>
    public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Count entities by specification
    /// </summary>
    public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).CountAsync(cancellationToken);
    }

    /// <summary>
    /// Count all entities
    /// </summary>
    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().CountAsync(cancellationToken);
    }

    /// <summary>
    /// Check if any entity exists by specification
    /// </summary>
    public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Check if any entity exists
    /// </summary>
    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AnyAsync(cancellationToken);
    }

    /// <summary>
    /// Get entities as async enumerable by specification
    /// </summary>
    public virtual IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
    {
        return ApplySpecification(specification).AsAsyncEnumerable();
    }

    /// <summary>
    /// Apply specification to query
    /// </summary>
    protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        var evaluator = new SpecificationEvaluator();
        return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
    }

    /// <summary>
    /// Apply specification to query with projection
    /// </summary>
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        var evaluator = new SpecificationEvaluator();
        return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
    }
}

/// <summary>
/// Repository with events implementation
/// </summary>
public class RepositoryWithEventsBase<T> : RepositoryBase<T>, IRepositoryWithEvents<T>
    where T : class, IAggregateRoot
{
    /// <summary>
    /// Constructor
    /// </summary>
    public RepositoryWithEventsBase(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    /// <summary>
    /// Find entities by predicate
    /// </summary>
    public Task<IEnumerable<T>> FindByAsync(Func<T, bool> predicate)
    {
        return Task.FromResult(_dbContext.Set<T>().Where(predicate));
    }

    /// <summary>
    /// Soft delete an entity
    /// </summary>
    public async Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is ISoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsDeleted = true;
            softDeleteEntity.DeletedOn = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            await DeleteAsync(entity, cancellationToken);
        }
    }

    /// <summary>
    /// Soft delete multiple entities
    /// </summary>
    public async Task SoftDeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                softDeleteEntity.DeletedOn = DateTime.UtcNow;
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}