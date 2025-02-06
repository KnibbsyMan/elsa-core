using System.Text.RegularExpressions;

namespace Elsa.Sql.Helpers;

internal static class SqlClientHelpers
{
    /// <summary>
    /// Validate table name to ensure its safe
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public static bool IsTableNameValid(List<string> validTableNames, string tableName)
    {
        if (validTableNames.Exists(t => t.Equals(tableName, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return Regex.IsMatch(tableName, @"^[A-Za-z0-9_]+$");
    }
}