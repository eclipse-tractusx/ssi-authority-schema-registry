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
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Org.Eclipse.TractusX.Portal.Backend.Framework.DBAccess;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Logging;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Seeding.DependencyInjection;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities;
using Serilog;
using System.Reflection;

LoggingExtensions.EnsureInitialized();
Log.Information("Starting process");
try
{
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services
                .AddDbContext<RegistryContext>(o =>
                    o.UseNpgsql(hostContext.Configuration.GetConnectionString("RegistryDb"),
                        x => x.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)
                            .MigrationsHistoryTable("__efmigrations_history_authority_schema_registry", "public"))
                        .ReplaceService<IHistoryRepository, CustomNpgsqlHistoryRepository>())
                .AddDatabaseInitializer<RegistryContext>(hostContext.Configuration.GetSection("Seeding"));
        })
        .AddLogging()
        .Build();

    await host.Services.InitializeDatabasesAsync(); // We don't actually run anything here. The magic happens in InitializeDatabasesAsync
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Log.Fatal(ex, "Unhandled exception");
    throw;
}
finally
{
    Log.Information("Process Shutting down...");
    await Log.CloseAndFlushAsync();
}
