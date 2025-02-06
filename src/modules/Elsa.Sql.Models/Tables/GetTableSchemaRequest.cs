namespace Elsa.Sql;

public class GetTableSchemaRequest
{
    public string Client { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
    public string Table { get; set; } = default!;
}