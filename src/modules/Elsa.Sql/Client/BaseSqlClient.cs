using System.Data;

namespace Elsa.Sql.Client;

public abstract class BaseSqlClient
{
    /// <summary>
    /// Returns <see cref="IDataReader"/> data as a <see cref="DataSet"/>.
    /// </summary>
    /// <param name="reader">Reader to return data from.</param>
    /// <returns><see cref="DataSet"/> of data.</returns>
    protected static DataSet ReadAsDataSet(IDataReader reader, string datasetName = "dataset")
    {
        var dataSet  = new DataSet(datasetName);
        dataSet.Tables.Add(ReadAsDataTable(reader)); 
        return dataSet;
    }

    /// <summary>
    /// Returns <see cref="IDataReader"/> data as a <see cref="DataTable"/>.
    /// </summary>
    /// <param name="reader">Reader to return data from.</param>
    /// <param name="tableName">Table name the data is from.</param>
    /// <returns><see cref="DataTable"/> of data.</returns>
    protected static DataTable ReadAsDataTable(IDataReader reader, string tableName = "table")
    {
        var data = new DataTable(tableName);
        var schemaTable = reader.GetSchemaTable();

        foreach (DataRow row in schemaTable.Rows)
        {
            var colName = row.Field<string>("ColumnName");
            var type = row.Field<Type>("DataType");
            data.Columns.Add(colName, type);
        }

        while (reader.Read())
        {
            var newRow = data.Rows.Add();
            foreach (DataColumn col in data.Columns)
            {
                newRow[col.ColumnName] = reader[col.ColumnName];
            }
        }
        return data;
    }

    /// <summary>
    /// Returns a list of table names from a dataset
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected static List<string> ReadAsListOfTableNames(IDataReader reader)
    {
        var results = new List<string>();
        while (reader.Read())
        {
            var tableName = reader.GetString(0);
            if (reader.FieldCount == 1)
            {
                results.Add($"{tableName}");
                continue;
            }

            var schemaName = reader.IsDBNull(1) ? null : reader.GetString(1);
            if (!string.IsNullOrEmpty(schemaName))
            {
                results.Add($"{schemaName}.{tableName}");
            }
            else
            {
                results.Add($"{tableName}");
            }
        }
        results.Order();
        return results;
    }

    /// <summary>
    /// Returns a table schema
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    protected static TableSchema ReadAsTableSchema(IDataReader reader, string tableName)
    {
        var results = new TableSchema(tableName);
        foreach (DataRow row in reader.GetSchemaTable().Rows)
        {
            results.Columns.Add(new ColumnSchema
            {
                IsKey = row.Field<bool>("IsKey"),
                AllowDbNull = row.Field<bool>("AllowDbNull"),
                Name = row.Field<string>("ColumnName"),
                SqlType = row.Field<string?>("DataTypeName"),
                DotNetType = row.Field<Type?>("DataType")?.FullName
            });
        }
        results.Count = results.Columns.Count();
        results.Columns.OrderBy(x => x.Name);
        return results;
    }

    /// <summary>
    /// Returns a database schema
    /// </summary>
    /// <param name="tables"></param>
    /// <param name="databaseName"></param>
    /// <returns></returns>
    protected static DatabaseSchema ReadAsDatabaseSchema(IList<TableSchema> tables, string databaseName)
    {
        var results = new DatabaseSchema(databaseName);
        foreach (TableSchema table in tables)
        {
            results.Tables.Add(table);
        }
        results.Count = results.Tables.Count();
        return results;
    }
}