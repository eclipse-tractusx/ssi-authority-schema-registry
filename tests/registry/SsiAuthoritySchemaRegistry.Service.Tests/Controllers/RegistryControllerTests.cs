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

using FluentAssertions;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.DbAccess.Models;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Enums;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Tests.Setup;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Tests.Controllers;

public class RegistryControllerTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private const string ValidDid = "did:web:portal-backend.int.catena-x.net:api:administration:staticdata:did:BPNL00000003CRHL";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };

    private const string BaseUrl = "/api/registry";
    private readonly HttpClient _client = factory.CreateClient();

    #region GetCredentials

    [Fact]
    public async Task GetCredentials_WithoutFilters_ReturnsExpected()
    {
        // Act
        var data = await _client.GetFromJsonAsync<IEnumerable<CredentialData>>($"{BaseUrl}/credentials", JsonOptions);

        // Assert
        data.Should().NotBeNull().And.HaveCount(4).And.Satisfy(
            x => x.CredentialName == "BusinessPartnerNumber" && x.Credential == "BusinessPartnerCredential" && x.Authorities.Count() == 1,
            x => x.CredentialName == "Membership" && x.Credential == "MembershipCredential" && x.Authorities.Count() == 1,
            x => x.CredentialName == "CompanyRole" && x.Credential == "DismantlerCredential" && x.Authorities.Count() == 2,
            x => x.CredentialName == "Framework" && x.Credential == "DataExchangeGovernanceCredential" && x.Authorities.Count() == 1
        );
    }

    [Fact]
    public async Task GetCredentials_WithBpnFilters_ReturnsExpected()
    {
        // Act
        var data = await _client.GetFromJsonAsync<IEnumerable<CredentialData>>($"{BaseUrl}/credentials?did={ValidDid}", JsonOptions);

        // Assert
        data.Should().ContainSingle().And.Satisfy(
            x => x.CredentialName == "CompanyRole" && x.Credential == "DismantlerCredential");
    }

    [Fact]
    public async Task GetCredentials_WithCredentialTypeFilters_ReturnsExpected()
    {
        // Act
        var data = await _client.GetFromJsonAsync<IEnumerable<CredentialData>>($"{BaseUrl}/credentials?credentialTypeId={CredentialTypeId.Framework}", JsonOptions);

        // Assert
        data.Should().HaveCount(1).And.Satisfy(
            x => x.CredentialName == "Framework" && x.Credential == "DataExchangeGovernanceCredential");
    }

    #endregion
}
