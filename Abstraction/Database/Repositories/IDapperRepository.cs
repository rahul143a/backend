using Shared.Common;

namespace Abstraction.Database.Repositories;

/// <summary>
/// Interface for Dapper repository
/// </summary>
public interface IDapperRepository : ITransientRepository
{
    /// <summary>
    /// Execute a query and return the first result
    /// </summary>
    Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, System.Data.IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Execute a query and return all results
    /// </summary>
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, System.Data.IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    /// Execute a command and return the number of affected rows
    /// </summary>
    Task<int> ExecuteAsync(string sql, object? param = null, System.Data.IDbTransaction? transaction = null, CancellationToken cancellationToken = default);
}
