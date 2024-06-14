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
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Models;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Tests.Setup;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Service.Tests.Controllers;

public class SchemaControllerTests(IntegrationTestFactory factory) : IClassFixture<IntegrationTestFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };

    private const string BaseUrl = "/api/schema";
    private readonly HttpClient _client = factory.CreateClient();

    #region Validate

    [Fact]
    public async Task Validate_WithValidBusinessPartner_ReturnsTrue()
    {
        // Arrange
        var json = JsonDocument.Parse("""
                                      {
                                          "@context": [
                                              "https://www.w3.org/2018/credentials/v1",
                                              "https://w3id.org/security/suites/jws-2020/v1",
                                              "https://raw.githubusercontent.com/catenax-ng/product-coreschemas/main/businessPartnerData"
                                          ],
                                          "id": "6b74cd89-6c25-4e3c-9fb7-3ee15c803afc",
                                          "issuer": "test",
                                          "type": [
                                              "VerifiableCredential",
                                              "BpnCredential"
                                          ],
                                          "issuanceDate": "2024-05-29T01:01:01Z",
                                          "expirationDate": "2024-05-29T01:01:01Z",
                                          "credentialSubject": {
                                              "id": "6b74cd89-6c25-4e3c-9fb7-3ee15c803afc",
                                              "holderIdentifier": "BPNL00000001TEST",
                                              "bpn": "BPNL00000003CRHL"
                                          }
                                      }
                                      """);

        // Act
        var data = await _client.PostAsJsonAsync($"{BaseUrl}/validate?schemaType={CredentialSchemaType.BusinessPartnerCredential}", json, JsonOptions);

        // Assert
        var result = await data.Content.ReadFromJsonAsync<bool>();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithInvalidBusinessPartner_ReturnsFalse()
    {
        // Arrange
        var json = JsonDocument.Parse("{}");

        // Act
        var data = await _client.PostAsJsonAsync($"{BaseUrl}/validate?schemaType={CredentialSchemaType.BusinessPartnerCredential}", json, JsonOptions);

        // Assert
        var result = await data.Content.ReadFromJsonAsync<bool>(JsonOptions);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_WithValidDismantler_ReturnsTrue()
    {
        // Arrange
        var schema = JsonDocument.Parse("""
                                        {
                                            "@context": [
                                                "https://www.w3.org/2018/credentials/v1",
                                                "https://w3id.org/security/suites/jws-2020/v1",
                                                "https://raw.githubusercontent.com/catenax-ng/product-coreschemas/main/businessPartnerData"
                                            ],
                                            "id": "6b74cd89-6c25-4e3c-9fb7-3ee15c803afc",
                                            "issuer": "test",
                                            "type": [
                                                "VerifiableCredential",
                                                "MembershipCredential"
                                            ],
                                            "issuanceDate": "2024-05-29T01:01:01Z",
                                            "expirationDate": "2024-12-31T23:59:59Z",
                                            "credentialSubject": {
                                                "id": "6b74cd89-6c25-4e3c-9fb7-3ee15c803afc",
                                                "holderIdentifier": "Test",
                                                "allowedVehicleBrands": [ "BMW", "Mercedes" ]
                                            }
                                        }
                                        """);

        // Act
        var data = await _client.PostAsJsonAsync($"{BaseUrl}/validate?schemaType={CredentialSchemaType.DismantlerCredential}", schema, JsonOptions);

        // Assert
        var result = await data.Content.ReadFromJsonAsync<bool>();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithInvalidDismantler_ReturnsFalse()
    {
        // Arrange
        var json = JsonDocument.Parse("{}");

        // Act
        var data = await _client.PostAsJsonAsync($"{BaseUrl}/validate?schemaType={CredentialSchemaType.DismantlerCredential}", json, JsonOptions);

        // Assert
        var result = await data.Content.ReadFromJsonAsync<bool>();
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_WithValidFramework_ReturnsTrue()
    {
        // Arrange
        var json = JsonDocument.Parse("""
                                      {
                                          "@context": [
                                              "https://www.w3.org/2018/credentials/v1",
                                              "https://w3id.org/security/suites/jws-2020/v1",
                                              "https://raw.githubusercontent.com/catenax-ng/product-coreschemas/main/businessPartnerData"
                                          ],
                                          "id": "6b74cd89-6c25-4e3c-9fb7-3ee15c803afc",
                                          "issuer": "test",
                                          "type": [
                                              "VerifiableCredential",
                                              "MembershipCredential"
                                          ],
                                          "issuanceDate": "2024-05-29T01:01:01Z",
                                          "expirationDate": "2024-12-31T23:59:59Z",
                                          "credentialSubject": {
                                              "id": "6b74cd89-6c25-4e3c-9fb7-3ee15c803afc",
                                              "holderIdentifier": "Test",
                                              "group": "UseCaseFramework",
                                              "useCase": "16c1029e-c4af-47e6-9c33-a144625cc7ac",
                                              "contractTemplate": "https://example.org/temp-1",
                                              "contractVersion": "1.0"
                                          }
                                      }
                                      """);

        // Act
        var data = await _client.PostAsJsonAsync($"{BaseUrl}/validate?schemaType={CredentialSchemaType.FrameworkCredential}", json, JsonOptions);

        // Assert
        var result = await data.Content.ReadFromJsonAsync<bool>();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithInvalidFramework_ReturnsFalse()
    {
        // Arrange
        var json = JsonDocument.Parse("{}");

        // Act
        var data = await _client.PostAsJsonAsync($"{BaseUrl}/validate?schemaType={CredentialSchemaType.FrameworkCredential}", json, JsonOptions);

        // Assert
        var result = await data.Content.ReadFromJsonAsync<bool>();
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_WithValidMembership_ReturnsTrue()
    {
        // Arrange
        var json = JsonDocument.Parse("""
                                      {
                                          "@context": [
                                              "https://www.w3.org/2018/credentials/v1",
                                              "https://w3id.org/security/suites/jws-2020/v1",
                                              "https://raw.githubusercontent.com/catenax-ng/product-coreschemas/main/businessPartnerData"
                                          ],
                                          "id": "6b74cd89-6c25-4e3c-9fb7-3ee15c803afc",
                                          "issuer": "test",
                                          "type": [
                                              "VerifiableCredential",
                                              "MembershipCredential"
                                          ],
                                          "issuanceDate": "2024-05-29T01:01:01Z",
                                          "expirationDate": "2024-12-31T23:59:59Z",
                                          "credentialSubject": {
                                              "id": "6b74cd89-6c25-4e3c-9fb7-3ee15c803afc",
                                              "holderIdentifier": "Test",
                                              "memberOf": "catena-x"
                                          }
                                      }
                                      """);

        // Act
        var data = await _client.PostAsJsonAsync($"{BaseUrl}/validate?schemaType={CredentialSchemaType.MembershipCredential}", json, JsonOptions);

        // Assert
        var result = await data.Content.ReadFromJsonAsync<bool>();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithInvalidMembership_ReturnsFalse()
    {
        // Arrange
        var json = JsonDocument.Parse("{}");

        // Act
        var data = await _client.PostAsJsonAsync($"{BaseUrl}/validate?schemaType={CredentialSchemaType.MembershipCredential}", json, JsonOptions);

        // Assert
        var result = await data.Content.ReadFromJsonAsync<bool>();
        result.Should().BeFalse();
    }

    #endregion
}
