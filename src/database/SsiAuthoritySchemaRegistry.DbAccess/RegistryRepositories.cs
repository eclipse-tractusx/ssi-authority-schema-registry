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

using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.DbAccess.Repositories;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities;
using System.Collections.Immutable;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.DbAccess;

public class RegistryRepositories : IRegistryRepositories
{
    private readonly RegistryContext _dbContext;

    private static readonly IReadOnlyDictionary<Type, Func<RegistryContext, Object>> Types = new Dictionary<Type, Func<RegistryContext, Object>> {
        { typeof(ICredentialRepository), context => new CredentialRepository(context) }
    }.ToImmutableDictionary();

    public RegistryRepositories(RegistryContext dbContext)
    {
        _dbContext = dbContext;
    }

    public RepositoryType GetInstance<RepositoryType>()
    {
        object? repository = default;

        if (Types.TryGetValue(typeof(RepositoryType), out var createFunc))
        {
            repository = createFunc(_dbContext);
        }

        return (RepositoryType)(repository ?? throw new ArgumentException($"unexpected type {typeof(RepositoryType).Name}", nameof(RepositoryType)));
    }
}
