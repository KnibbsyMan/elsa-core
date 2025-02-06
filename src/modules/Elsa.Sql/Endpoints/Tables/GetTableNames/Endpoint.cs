using Elsa.Abstractions;
using Elsa.Models;
using Elsa.Sql.Contracts;
using JetBrains.Annotations;

namespace Elsa.Sql.Endpoints.Tables.GetTableNames;

/// <summary>
/// Returns all table names from the database.
/// </summary>
[PublicAPI]
internal class GetTableNames : ElsaEndpoint<GetTableNamesRequest, ListResponse<string>>
{
    private readonly ISqlClientFactory _sqlClientFactory;

    /// <inheritdoc />
    public GetTableNames(ISqlClientFactory sqlClientFactory)
    {
        _sqlClientFactory = sqlClientFactory;
    }

    /// <inheritdoc />
    public override void Configure()
    {
        Post("/sql-client/table/names");
        AllowAnonymous();
        //ConfigurePermissions("sql:table-names");
    }

    /// <inheritdoc />
    public override async Task HandleAsync(GetTableNamesRequest request, CancellationToken cancellationToken)
    {
        var client = _sqlClientFactory.CreateClient(request.Client, request.ConnectionString);
        var tableNames = await client.GetTableNames(request.IncludeViews);
        await SendOkAsync(new ListResponse<string>(tableNames), cancellationToken);
    }
}