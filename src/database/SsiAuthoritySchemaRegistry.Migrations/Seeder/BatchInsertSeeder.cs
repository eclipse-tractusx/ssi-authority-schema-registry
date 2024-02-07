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

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Seeding;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Entities;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Migrations.Seeder;

/// <summary>
/// Seeder to seed the all configured entities
/// </summary>
public class BatchInsertSeeder(RegistryContext context, ILogger<BatchInsertSeeder> logger, IOptions<SeederSettings> options)
    : ICustomSeeder
{
    private readonly SeederSettings _settings = options.Value;

    /// <inheritdoc />
    public int Order => 1;

    /// <inheritdoc />
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (!_settings.DataPaths.Any())
        {
            logger.LogInformation("There a no data paths configured, therefore the {SeederName} will be skipped", nameof(BatchInsertSeeder));
            return;
        }

        await SeedTable<Authority>("authorities", x => x.Bpn, cancellationToken).ConfigureAwait(false);
        await SeedTable<Credential>("credentials", x => x.Id, cancellationToken).ConfigureAwait(false);
        await SeedTable<CredentialAuthority>("credential_authorities", x => new { x.CredentialId, x.Bpn }, cancellationToken).ConfigureAwait(false);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task SeedTable<T>(string fileName, Func<T, object> keySelector, CancellationToken cancellationToken) where T : class
    {
        logger.LogDebug("Start seeding {Filename}", fileName);
        var additionalEnvironments = _settings.TestDataEnvironments ?? Enumerable.Empty<string>();
        var data = await SeederHelper.GetSeedData<T>(logger, fileName, _settings.DataPaths, cancellationToken, additionalEnvironments.ToArray()).ConfigureAwait(false);
        logger.LogDebug("Found {ElementCount} data", data.Count);
        if (data.Any())
        {
            var typeName = typeof(T).Name;
            logger.LogDebug("Started to Seed {TableName}", typeName);
            data = data.GroupJoin(context.Set<T>(), keySelector, keySelector, (d, dbEntry) => new { d, dbEntry })
                .SelectMany(t => t.dbEntry.DefaultIfEmpty(), (t, x) => new { t, x })
                .Where(t => t.x == null)
                .Select(t => t.t.d).ToList();
            logger.LogDebug("Seeding {DataCount} {TableName}", data.Count, typeName);
            await context.Set<T>().AddRangeAsync(data, cancellationToken).ConfigureAwait(false);
            logger.LogDebug("Seeded {TableName}", typeName);
        }
    }
}
