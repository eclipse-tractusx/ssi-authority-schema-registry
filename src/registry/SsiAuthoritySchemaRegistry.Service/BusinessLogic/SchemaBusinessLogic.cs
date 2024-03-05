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

using Json.Schema;
using Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Models;
using System.Reflection;
using System.Text.Json;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.BusinessLogic;

public class SchemaBusinessLogic : ISchemaBusinessLogic
{
    public async Task<bool> Validate(CredentialSchemaType schemaType, JsonDocument content, CancellationToken cancellationToken)
    {
        var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (location == null)
        {
            throw new UnexpectedConditionException("Assembly location must be set");
        }

        var path = Path.Combine(location, "Schemas", $"{schemaType.ToString()}.schema.json");
        var schemaJson = await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);

        var schema = JsonSchema.FromText(schemaJson);
        SchemaRegistry.Global.Register(schema);
        var results = schema.Evaluate(content);
        return results.IsValid;
    }
}
