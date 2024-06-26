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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.DbAccess.DependencyInjection;

public static class RegistryRepositoriesServiceExtensions
{
    private static readonly string[] Tags = ["registrydb"];

    public static IServiceCollection AddRegistryRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<RegistryContext>(o =>
                o.UseNpgsql(configuration.GetConnectionString("RegistryDb")))
            .AddScoped<IRegistryRepositories, RegistryRepositories>()
            .AddHealthChecks()
            .AddDbContextCheck<RegistryContext>("RegistryContext", tags: Tags);
        return services;
    }
}
