using Elsa.Abstractions;
using Elsa.Sql.Contracts;
using Elsa.Sql.Helpers;
using JetBrains.Annotations;

namespace Elsa.Sql.Endpoints.Tables.GetSchema;

/// <summary>
/// Returns the table schema from the database.
/// </summary>
[PublicAPI]
internal class GetSchema : ElsaEndpoint<GetTableSchemaRequest, TableSchema>
{
    private readonly ISqlClientFactory _sqlClientFactory;

    /// <inheritdoc />
    public GetSchema(ISqlClientFactory sqlClientFactory)
    {
        _sqlClientFactory = sqlClientFactory;
    }

    /// <inheritdoc />
    public override void Configure()
    {
        Post("/sql-client/table/schema");
        AllowAnonymous();
        //ConfigurePermissions("sql:table-schema");
    }

    /// <inheritdoc />
    public override async Task HandleAsync(GetTableSchemaRequest request, CancellationToken cancellationToken)
    {
        var client = _sqlClientFactory.CreateClient(request.Client, request.ConnectionString);
        var tableNames = await client.GetTableNames(true);
        if (!SqlClientHelpers.IsTableNameValid(tableNames, request.Table))
        {
            await SendNotFoundAsync(cancellationToken);
        }

        var tableSchema = await client.GetTableSchema(request.Table);
        await SendOkAsync(tableSchema, cancellationToken);
    }
}