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

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Logging;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Seeding;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Migrations.Seeder;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.BusinessLogic;
using System.Text.Json.Serialization;
using Testcontainers.PostgreSql;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Tests.Setup;

public class IntegrationTestFactory : WebApplicationFactory<RegistryBusinessLogic>, IAsyncLifetime
{
    private static readonly string[] TestDataEnvironments = ["test"];
    private static readonly string[] DataPaths = ["Seeder/Data", "Seeder/Data/overwrite"];

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithDatabase("test_db")
        .WithImage("postgres")
        .WithCleanUp(true)
        .WithName(Guid.NewGuid().ToString())
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("SKIP_CONFIGURATION_VALIDATION", "true");
        builder.ConfigureTestServices(services =>
        {
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RegistryContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<RegistryContext>(options =>
            {
                options.UseNpgsql(_container.GetConnectionString(),
                        x => x.MigrationsAssembly(typeof(BatchInsertSeeder).Assembly.GetName().Name)
                            .MigrationsHistoryTable("__efmigrations_history_registry"));
            });
        });
    }

    /// <inheritdoc />
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.AddLogging();
        var host = base.CreateHost(builder);

        var optionsBuilder = new DbContextOptionsBuilder<RegistryContext>();

        optionsBuilder.UseNpgsql(
            _container.GetConnectionString(),
            x => x.MigrationsAssembly(typeof(BatchInsertSeeder).Assembly.GetName().Name)
                .MigrationsHistoryTable("__efmigrations_history_registry", "public")
        );
        var context = new RegistryContext(optionsBuilder.Options);
        context.Database.Migrate();

        var seederOptions = Options.Create(new SeederSettings
        {
            TestDataEnvironments = TestDataEnvironments,
            DataPaths = DataPaths
        });
        var insertSeeder = new BatchInsertSeeder(context,
            LoggerFactory.Create(c => c.AddConsole()).CreateLogger<BatchInsertSeeder>(),
            seederOptions);
        insertSeeder.ExecuteAsync(CancellationToken.None).GetAwaiter().GetResult();
        return host;
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public new async Task DisposeAsync() => await _container.DisposeAsync();
}
