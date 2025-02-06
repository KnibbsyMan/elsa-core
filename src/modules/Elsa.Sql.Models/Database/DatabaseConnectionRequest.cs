namespace Elsa.Sql;

public class DatabaseConnectionRequest
{
    public string Client { get; set; } = default!;
    public string ConnectionString { get; set; } = default!;
}