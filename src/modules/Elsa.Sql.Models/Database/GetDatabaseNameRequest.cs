namespace Elsa.Sql;

public class GetDatabaseNameRequest
{
    public string Client { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
    public bool IncludeViews { get; set; } = default!;
}