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
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.ErrorHandling;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Models;
using System.Reflection;
using System.Text.Json;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.BusinessLogic;

public class SchemaBusinessLogic : ISchemaBusinessLogic
{
    public async Task<bool> Validate(CredentialSchemaType? schemaType, JsonDocument content)
    {
        var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (location == null)
        {
            throw UnexpectedConditionException.Create(ErrorTypes.ASSEMBLY_LOCATION_NOT_SET);
        }

        if (schemaType is not null)
        {
            return await ValidateSchema(schemaType.Value, content, location).ConfigureAwait(ConfigureAwaitOptions.None);
        }

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = 2
        };

        var typeResult = false;
        await Parallel.ForEachAsync(Enum.GetValues<CredentialSchemaType>(), options, async (type, _) =>
            {
                if (await ValidateSchema(type, content, location).ConfigureAwait(ConfigureAwaitOptions.None))
                {
                    typeResult = true;
                }
            })
        .ConfigureAwait(ConfigureAwaitOptions.None);
        return typeResult;
    }

    private static async Task<bool> ValidateSchema(CredentialSchemaType schemaType, JsonDocument content, string location)
    {
        var path = Path.Combine(location, "Schemas", $"{schemaType}.schema.json");
        var schema = await JsonSchema.FromStream(File.OpenRead(path)).ConfigureAwait(false);
        SchemaRegistry.Global.Register(schema);
        var results = schema.Evaluate(content);
        return results.IsValid;
    }
}
