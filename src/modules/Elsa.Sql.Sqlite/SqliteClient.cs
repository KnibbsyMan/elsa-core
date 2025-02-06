using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using Elsa.Sql.Client;
using Microsoft.Data.Sqlite;

namespace Elsa.Sql.Sqlite;

public class SqliteClient : BaseSqlClient, ISqlClient
{
    private string _connectionString;

    /// <summary>
    /// Sqlite client implementation.
    /// </summary>
    /// <param name="connectionString"></param>
    public SqliteClient(string? connectionString) => _connectionString = connectionString;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<int?> ExecuteCommandAsync(string sqlCommand)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = new SqliteCommand(sqlCommand, connection);

        var result = await command.ExecuteNonQueryAsync();
        return result;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<object?> ExecuteScalarAsync(string sqlQuery)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = new SqliteCommand(sqlQuery, connection);

        var result = await command.ExecuteScalarAsync();
        return result;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<DataSet?> ExecuteQueryAsync(string sqlQuery)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var command = new SqliteCommand(sqlQuery, connection);

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
        if (builder.TryGetValue("Data Source", out var dataSource))
        {
            return Path.GetFileName(dataSource.ToString());
        }
        return "Error - No database name found.";
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<List<string>?> GetTableNames(bool includeViews = false)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var whereClause = includeViews ? "(type = 'table' OR type = 'view')" : "type = 'table'";
        var query = $@"
            SELECT name
            FROM sqlite_master
            WHERE {whereClause}
            ORDER BY name;";
        var command = new SqliteCommand(query, connection);

        using var reader = await command.ExecuteReaderAsync();
        return await Task.FromResult(ReadAsListOfTableNames(reader));
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<TableSchema> GetTableSchema(string tableName)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var query = $@"
            SELECT *
            FROM {tableName}
            LIMIT 0;";
        var command = new SqliteCommand(query, connection);

        using var reader = await command.ExecuteReaderAsync();
        var tableSchema = ReadAsTableSchema(reader, tableName);
        return await Task.FromResult(tableSchema);
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