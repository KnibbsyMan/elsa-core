using Elsa.Abstractions;
using Elsa.Sql.Contracts;
using JetBrains.Annotations;

namespace Elsa.Sql.Endpoints.Database.Connect;

/// <summary>
/// Checks if a connection can be made with the database.
/// </summary>
[PublicAPI]
internal class Connect : ElsaEndpoint<DatabaseConnectionRequest, bool>
{
    private readonly ISqlClientFactory _sqlClientFactory;

    /// <inheritdoc />
    public Connect(ISqlClientFactory sqlClientFactory)
    {
        _sqlClientFactory = sqlClientFactory;
    }

    /// <inheritdoc />
    public override void Configure()
    {
        Post("/sql-client/database/connect");
        AllowAnonymous();
        //ConfigurePermissions("sql:table");
    }

    /// <inheritdoc />
    public override async Task HandleAsync(DatabaseConnectionRequest request, CancellationToken cancellationToken)
    {
        var client = _sqlClientFactory.CreateClient(request.Client, request.ConnectionString);
        var canConnect = await client.CanConnect();
        await SendOkAsync(canConnect, cancellationToken);
    }
}