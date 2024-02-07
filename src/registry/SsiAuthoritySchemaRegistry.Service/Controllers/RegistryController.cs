/********************************************************************************
 * Copyright (c) 2024 Contributors to the Eclipse Foundation
 *
 * See the NOTICE file(s) distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This program and the accompanying materials are made available under the
 * terms of the Apache License, Version 2.0 which is available at
 * https://www.apache.org/licenses/LICENSE-2.0.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 *
 * SPDX-License-Identifier: Apache-2.0
 ********************************************************************************/

using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.DbAccess.Models;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Enums;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.BusinessLogic;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Extensions;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Models;
using System.Diagnostics.CodeAnalysis;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Controllers;

/// <summary>
/// Creates a new instance of <see cref="RegistryController"/>
/// </summary>
public static class RegistryController
{
    [ExcludeFromCodeCoverage]
    public static RouteGroupBuilder MapRegistryApi(this RouteGroupBuilder group)
    {
        var policyHub = group.MapGroup("/registry");

        policyHub.MapGet("credentials", (string? bpnl, CredentialTypeId? credentialTypeId, IRegistryBusinessLogic logic) => logic.GetCredentials(bpnl, credentialTypeId))
            .WithSwaggerDescription("Gets all credentials with optional filter posibilities",
                "Example: GET: api/registry/credentials",
                "OPTIONAL: BPNL to filter the response",
                "OPTIONAL: Type to filter the response")
            .WithDefaultResponses()
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<CredentialData>), Constants.JsonContentType);

        return group;
    }
}
