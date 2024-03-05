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

using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Enums;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Entities;

public class Credential(Guid id, CredentialTypeId typeId, string name)
{
    public Guid Id { get; set; } = id;
    public CredentialTypeId TypeId { get; set; } = typeId;
    public string Name { get; set; } = name;

    public ICollection<CredentialAuthority> Authorities { get; private set; } = new HashSet<CredentialAuthority>();
    public virtual CredentialType? Type { get; private set; }
}
