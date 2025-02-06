namespace Elsa.Sql;

public class DatabaseSchema
{
    public string Name { get; set; }
    public List<TableSchema> Tables { get; set; } = new()!;
    public int Count { get; set; }

    public DatabaseSchema() { }
    public DatabaseSchema(string databaseName)
    {
        Name = databaseName;
    }
}