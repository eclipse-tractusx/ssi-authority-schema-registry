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

using Org.Eclipse.TractusX.Portal.Backend.Framework.Web;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.DbAccess.DependencyInjection;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Controllers;
using System.Text.Json.Serialization;

const string Version = "v1";

WebApplicationBuildRunner
    .BuildAndRunWebApplication<Program>(args, "registry", Version, ".Registry",
        builder =>
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRegistryRepositories(builder.Configuration);
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        },
        (app, _) =>
        {
            app.MapGroup("/api")
                .WithOpenApi()
                .MapRegistryApi()
                .MapSchemaApi();
        });
