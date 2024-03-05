using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.BusinessLogic;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Extensions;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Controllers;

public static class SchemaController
{
    [ExcludeFromCodeCoverage]
    public static RouteGroupBuilder MapSchemaApi(this RouteGroupBuilder group)
    {
        var policyHub = group.MapGroup("/schema");

        policyHub.MapPost("validate", (CredentialSchemaType schemaType, JsonDocument content, CancellationToken cancellationToken, ISchemaBusinessLogic logic) => logic.Validate(schemaType, content, cancellationToken))
            .WithSwaggerDescription("Gets all credentials with optional filter posibilities",
                "Example: POST: api/schema/validate",
                "The type of the schema that should be validated",
                "The schema as json")
            .WithDefaultResponses()
            .Produces(StatusCodes.Status200OK, typeof(bool), Constants.JsonContentType);

        return group;
    }
}
