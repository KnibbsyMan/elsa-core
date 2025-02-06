using Npgsql;
using Elsa.Sql.Client;
using System.Data;
using System.Collections.Concurrent;
using System.Data.Common;

namespace Elsa.Sql.PostgreSql;

public class PostgreSqlClient : BaseSqlClient, ISqlClient
{
    private string? _connectionString;

    /// <summary>
    /// PostgreSQL client implimentation.
    /// </summary>
    /// <param name="connectionString"></param>
    public PostgreSqlClient(string? connectionString) => _connectionString = connectionString;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<int?> ExecuteCommandAsync(string sqlCommand)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var command = new NpgsqlCommand(sqlCommand, connection);

        var result = await command.ExecuteNonQueryAsync();
        return result;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<object?> ExecuteScalarAsync(string sqlQuery)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var command = new NpgsqlCommand(sqlQuery, connection);

        var result = await command.ExecuteScalarAsync();
        return result;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<DataSet?> ExecuteQueryAsync(string sqlQuery)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var command = new NpgsqlCommand(sqlQuery, connection);

        using var reader = await command.ExecuteReaderAsync();
        return await Task.FromResult(ReadAsDataSet(reader));
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<bool> CanConnect()
    {
        try
        {
            await ExecuteCommandAsync("SELECT 1");
            return await Task.FromResult(true);
        }
        catch (Exception)
        {
            return await Task.FromResult(false);
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public string GetDatabaseName()
    {
        var builder = new DbConnectionStringBuilder { ConnectionString = _connectionString };
        if (builder.TryGetValue("Database", out var dbName))
        {
            return dbName.ToString();
        }
        return "Error - No database name found.";
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<List<string>?> GetTableNames(bool includeViews = false)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var whereClause = includeViews ? "TABLE_TYPE IN ('BASE TABLE', 'VIEW')" : "TABLE_TYPE = 'BASE TABLE'";
        var query = $@"
            SELECT TABLE_NAME, TABLE_SCHEMA
            FROM INFORMATION_SCHEMA.TABLES
            WHERE {whereClause}
            ORDER BY TABLE_SCHEMA, TABLE_NAME;";
        var command = new NpgsqlCommand(query, connection);

        using var reader = await command.ExecuteReaderAsync();
        return await Task.FromResult(ReadAsListOfTableNames(reader));
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<TableSchema> GetTableSchema(string tableName)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = $@"
            SELECT TOP 0 *
            FROM {tableName};";
        var command = new NpgsqlCommand(query, connection);

        using var reader = await command.ExecuteReaderAsync();
        return await Task.FromResult(ReadAsTableSchema(reader, tableName));
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<DatabaseSchema> GetDatabaseSchema(bool includeViews = false)
    {
        var results = new ConcurrentBag<TableSchema>();
        var tables = await GetTableNames(includeViews);
        await Parallel.ForEachAsync(tables, async (table, cancellationToken) =>
        {
            results.Add(await GetTableSchema(table));
        });
        var databaseSchema = ReadAsDatabaseSchema(results.ToList(), GetDatabaseName());
        return await Task.FromResult(databaseSchema);
    }
}