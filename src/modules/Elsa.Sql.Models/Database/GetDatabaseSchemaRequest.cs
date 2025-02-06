namespace Elsa.Sql;

public class GetDatabaseSchemaRequest
{
    public string Client { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
    public bool IncludeViews { get; set; } = default!;
}