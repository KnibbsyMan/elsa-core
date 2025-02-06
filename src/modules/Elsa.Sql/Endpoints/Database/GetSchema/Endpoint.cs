using Elsa.Abstractions;
using Elsa.Sql.Contracts;
using JetBrains.Annotations;

namespace Elsa.Sql.Endpoints.Database.GetSchema;

/// <summary>
/// Returns the database schema.
/// </summary>
[PublicAPI]
internal class GetSchema : ElsaEndpoint<GetDatabaseSchemaRequest, DatabaseSchema>
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
        Post("/sql-client/database/schema");
        AllowAnonymous();
        //ConfigurePermissions("sql:database-schema");
    }

    /// <inheritdoc />
    public override async Task HandleAsync(GetDatabaseSchemaRequest request, CancellationToken cancellationToken)
    {
        var client = _sqlClientFactory.CreateClient(request.Client, request.ConnectionString);
        var databaseSchema = await client.GetDatabaseSchema(request.IncludeViews);
        await SendOkAsync(databaseSchema, cancellationToken);
    }
}