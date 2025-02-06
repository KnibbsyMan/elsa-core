using System.Data;

namespace Elsa.Sql.Client;

public interface ISqlClient
{
    /// <summary>
    /// Asyncronously executes a Transact-SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="sqlCommand">The command to execute</param>
    /// <returns>The number of rows affected.</returns>
    Task<int?> ExecuteCommandAsync(string sqlCommand);

    /// <summary>
    /// Asyncronously executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <param name="sqlQuery">The query to execute</param>
    /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty. Returns a maximum of 2033 characters.</returns>
    Task<object?> ExecuteScalarAsync(string sqlQuery);

    /// <summary>
    /// Asyncronously executes the query, and returns a dataset of data returned by the query.
    /// </summary>
    /// <param name="sqlQuery">Query to execute</param>
    /// <returns>DataSet of the quiried data</returns>
    Task<DataSet?> ExecuteQueryAsync(string sqlQuery);

    /// <summary>
    /// True if you can connect with the database
    /// </summary>
    /// <returns></returns>
    Task<bool> CanConnect();

    /// <summary>
    /// Returns the connection string database name
    /// </summary>
    /// <returns></returns>
    string GetDatabaseName();

    /// <summary>
    /// Returns a list of table names from the database.
    /// </summary>
    /// <param name="includeViews">True to include views in the returned tables</param>
    /// <returns></returns>
    Task<List<string>?> GetTableNames(bool includeViews = false);

    /// <summary>
    /// Returns the schema for a database table.
    /// </summary>
    /// <param name="tableName">Table name to get meta data for.</param>
    /// <returns></returns>
    Task<TableSchema> GetTableSchema(string tableName);

    /// <summary>
    /// Returns the schema of a database.
    /// </summary>
    /// <param name="includeViews">True to include views in the returned tables</param>
    /// <returns></returns>
    Task<DatabaseSchema> GetDatabaseSchema(bool includeViews = false);
}