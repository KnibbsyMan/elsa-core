namespace Elsa.Sql;

public class ColumnSchema
{
    public bool? IsKey { get; set; }
    public bool? AllowDbNull { get; set; }
    public string? Name { get; set; }
    public string? SqlType { get; set; }
    public string? DotNetType { get; set; }
}

//public bool? IsUnique { get; set; }
//public bool? IsAliased { get; set; }
//public bool? IsExpression { get; set; }
//public bool? IsIdentity { get; set; }
//public bool? IsAutoIncrement { get; set; }