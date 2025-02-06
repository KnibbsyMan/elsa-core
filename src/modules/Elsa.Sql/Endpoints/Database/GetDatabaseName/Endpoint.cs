using Elsa.Abstractions;
using Elsa.Models;
using Elsa.Sql.Contracts;
using JetBrains.Annotations;

namespace Elsa.Sql.Endpoints.Database.GetDatabaseName;

/// <summary>
/// Returns all table names from the database.
/// </summary>
[PublicAPI]
internal class GetDatabaseName : ElsaEndpoint<GetDatabaseNameRequest, string>
{
    private readonly ISqlClientFactory _sqlClientFactory;

    /// <inheritdoc />
    public GetDatabaseName(ISqlClientFactory sqlClientFactory)
    {
        _sqlClientFactory = sqlClientFactory;
    }

    /// <inheritdoc />
    public override void Configure()
    {
        Post("/sql-client/database/name");
        AllowAnonymous();
        //ConfigurePermissions("sql:database-name");
    }

    /// <inheritdoc />
    public override async Task HandleAsync(GetDatabaseNameRequest request, CancellationToken cancellationToken)
    {
        var client = _sqlClientFactory.CreateClient(request.Client, request.ConnectionString);
        var databaseName = client.GetDatabaseName();
        await SendOkAsync(databaseName, cancellationToken);
    }
}