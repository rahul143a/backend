using Dapper;
using Abstraction.Database.Repositories;
using Domain.MultiTenancy;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace Infrastructure.Database.Repositories;

/// <summary>
/// Implementation of Dapper repository
/// </summary>
public class DapperRepository : IDapperRepository
{
    /// <summary>
    /// Database connection string
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Constructor
    /// </summary>
    public DapperRepository(IOptions<DatabaseSettings> options)
    {
        _connectionString = DatabaseSettings.ConnectionString;
    }

    /// <summary>
    /// Create a new database connection
    /// </summary>
    private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    /// <summary>
    /// Execute a query and return the first result
    /// </summary>
    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class
    {
        using IDbConnection connection = CreateConnection();
        connection.Open();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
    }

    /// <summary>
    /// Execute a query and return all results
    /// </summary>
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class
    {
        using IDbConnection connection = CreateConnection();
        connection.Open();
        return await connection.QueryAsync<T>(sql, param, transaction);
    }

    /// <summary>
    /// Execute a command and return the number of affected rows
    /// </summary>
    public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    {
        using IDbConnection connection = CreateConnection();
        connection.Open();
        return await connection.ExecuteAsync(sql, param, transaction);
    }
}
