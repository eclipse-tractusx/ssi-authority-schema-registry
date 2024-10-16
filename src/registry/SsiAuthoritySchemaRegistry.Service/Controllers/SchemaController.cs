using Microsoft.AspNetCore.Mvc;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.BusinessLogic;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Extensions;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Models;
using System.Text.Json;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Controllers;

public static class SchemaController
{
    public static RouteGroupBuilder MapSchemaApi(this RouteGroupBuilder group)
    {
        var schema = group.MapGroup("/schema");

        schema.MapPost("validate", ([FromBody] JsonDocument content, [FromServices] ISchemaBusinessLogic logic, [FromQuery] CredentialSchemaType? schemaType = null) => logic.Validate(schemaType, content))
            .WithSwaggerDescription("Gets all credentials with optional filter possibilities",
                "Example: POST: api/schema/validate",
                "Optional: The type of the schema that should be validated",
                "The schema as json")
            .WithDefaultResponses()
            .Produces(StatusCodes.Status200OK, typeof(bool), Constants.JsonContentType);

        return group;
    }
}
