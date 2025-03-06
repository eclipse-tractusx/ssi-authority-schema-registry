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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.Eclipse.TractusX.Portal.Backend.Framework.Seeding;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Entities;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Migrations.Seeder;

/// <summary>
/// Seeder to seed the all configured entities
/// </summary>
public class BatchDeleteSeeder(RegistryContext context, ILogger<BatchDeleteSeeder> logger, IOptions<SeederSettings> options)
    : ICustomSeeder
{
    private readonly SeederSettings _settings = options.Value;

    /// <inheritdoc />
    public int Order => 2;

    /// <inheritdoc />
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (!_settings.DataPaths.Any())
        {
            logger.LogInformation("There a no data paths configured, therefore the {SeederName} will be skipped", nameof(BatchInsertSeeder));
            return;
        }

        await SeedTable<CredentialAuthority>("credential_authorities", x => new { x.CredentialId, x.Did }, cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);
        await SeedTable<Credential>("credentials", x => x.Id, cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);
        await SeedTable<Authority>("authorities", x => x.Did, cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(ConfigureAwaitOptions.None);
    }

    private async Task SeedTable<T>(string fileName, Func<T, object> keySelector, CancellationToken cancellationToken)
        where T : class
    {
        logger.LogDebug("Start seeding {Filename}", fileName);
        var additionalEnvironments = _settings.TestDataEnvironments ?? Enumerable.Empty<string>();
        var data = await SeederHelper
            .GetSeedData<T>(logger, fileName, _settings.DataPaths, cancellationToken, additionalEnvironments.ToArray())
            .ConfigureAwait(false);
        logger.LogDebug("Found {ElementCount} data", data.Count);

        // Identify entities in the database that are not present in the JSON data
        var existingEntities = await context.Set<T>().ToListAsync(cancellationToken).ConfigureAwait(false);
        var entitiesToRemove = existingEntities
            .Where(dbEntity => data.All(jsonEntity => !keySelector(dbEntity).Equals(keySelector(jsonEntity)))).ToList();
        context.Set<T>().RemoveRange(entitiesToRemove);
    }
}
