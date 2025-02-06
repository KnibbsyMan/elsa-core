using Elsa.Abstractions;
using Elsa.Sql.Services;
using JetBrains.Annotations;

namespace Elsa.Sql.Endpoints.Database.Connect;

/// <summary>
/// Returns all the registered SQL clients.
/// </summary>
/// <inheritdoc />
[PublicAPI]
internal class Get(ClientStore clientStore) : ElsaEndpointWithoutRequest<List<string>>
{
    /// <inheritdoc />
    public override void Configure()
    {
        Get("/sql-client/registered");
        AllowAnonymous();
        //ConfigurePermissions("sql:registered-clients");
    }

    /// <inheritdoc />
    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var registeredClients = clientStore.Clients.Select(x => x.Value).Select(x => x.Name).ToList();
        await SendOkAsync(registeredClients, cancellationToken);
    }
}