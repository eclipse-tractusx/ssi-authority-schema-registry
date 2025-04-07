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

using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using FluentAssertions;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.DbAccess.Repositories;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.DbAccess.Tests.Setup;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Enums;
using Xunit;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.DbAccess.Tests;

[Collection("TestDbFixture")]
public class CredentialRepositoryTests
{
    private const string ValidDid = "did:web:example.org:api:administration:staticdata:did:BPNL00000003CRHL";
    private readonly TestDbFixture _dbTestDbFixture;

    public CredentialRepositoryTests(TestDbFixture testDbFixture)
    {
        var fixture = new Fixture().Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));

        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _dbTestDbFixture = testDbFixture;
    }

    #region GetCredentials

    [Fact]
    public async Task GetCredentials_WithValidData_ReturnsExpected()
    {
        // Arrange
        var sut = await CreateSut();

        // Act
        var result = await sut.GetCredentials(null, null).ToListAsync();

        // Assert
        result.Should().HaveCount(4).And.Satisfy(
            x => x.CredentialName == "BusinessPartnerNumber" && x.Credential == "BusinessPartnerCredential",
            x => x.CredentialName == "Membership" && x.Credential == "MembershipCredential",
            x => x.CredentialName == "CompanyRole" && x.Credential == "DismantlerCredential",
            x => x.CredentialName == "Framework" && x.Credential == "DataExchangeGovernanceCredential");
    }

    [Fact]
    public async Task GetCredentials_WithBpnFilter_ReturnsExpected()
    {
        // Arrange
        var sut = await CreateSut();

        // Act
        var result = await sut.GetCredentials(ValidDid, null).ToListAsync();

        // Assert
        result.Should().ContainSingle().And.Satisfy(
            x => x.CredentialName == "CompanyRole" && x.Credential == "DismantlerCredential");
    }

    [Fact]
    public async Task GetCredentials_WithTypeFilter_ReturnsExpected()
    {
        // Arrange
        var sut = await CreateSut();

        // Act
        var result = await sut.GetCredentials(null, CredentialTypeId.Framework).ToListAsync();

        // Assert
        result.Should().ContainSingle().And.Satisfy(
            x => x.CredentialName == "Framework" && x.Credential == "DataExchangeGovernanceCredential");
    }

    #endregion

    #region Setup

    private async Task<CredentialRepository> CreateSut()
    {
        var context = await _dbTestDbFixture.GetDbContext();
        return new CredentialRepository(context);
    }

    #endregion
}
