namespace Elsa.Sql;

public class TableSchema
{
    public string Name { get; set; }
    public List<ColumnSchema> Columns { get; set; } = new();
    public int Count { get; set; }

    public TableSchema() { }
    public TableSchema(string tableName)
    {
        Name = tableName;
    }
}