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

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class _010rc1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ssi-authority-schema-registry");

            migrationBuilder.CreateTable(
                name: "authorities",
                schema: "ssi-authority-schema-registry",
                columns: table => new
                {
                    bpn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authorities", x => x.bpn);
                });

            migrationBuilder.CreateTable(
                name: "credential_types",
                schema: "ssi-authority-schema-registry",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_credential_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "credentials",
                schema: "ssi-authority-schema-registry",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_credentials", x => x.id);
                    table.ForeignKey(
                        name: "fk_credentials_credential_types_type_id",
                        column: x => x.type_id,
                        principalSchema: "ssi-authority-schema-registry",
                        principalTable: "credential_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credential_authorities",
                schema: "ssi-authority-schema-registry",
                columns: table => new
                {
                    credential_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bpn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_credential_authorities", x => new { x.credential_id, x.bpn });
                    table.ForeignKey(
                        name: "fk_credential_authorities_authorities_authority_temp_id",
                        column: x => x.bpn,
                        principalSchema: "ssi-authority-schema-registry",
                        principalTable: "authorities",
                        principalColumn: "bpn");
                    table.ForeignKey(
                        name: "fk_credential_authorities_credentials_credential_id",
                        column: x => x.credential_id,
                        principalSchema: "ssi-authority-schema-registry",
                        principalTable: "credentials",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                schema: "ssi-authority-schema-registry",
                table: "credential_types",
                columns: new[] { "id", "label" },
                values: new object[,]
                {
                    { 1, "BusinessPartnerNumber" },
                    { 2, "Membership" },
                    { 3, "Framework" },
                    { 4, "CompanyRole" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_credential_authorities_bpn",
                schema: "ssi-authority-schema-registry",
                table: "credential_authorities",
                column: "bpn");

            migrationBuilder.CreateIndex(
                name: "ix_credentials_type_id",
                schema: "ssi-authority-schema-registry",
                table: "credentials",
                column: "type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "credential_authorities",
                schema: "ssi-authority-schema-registry");

            migrationBuilder.DropTable(
                name: "authorities",
                schema: "ssi-authority-schema-registry");

            migrationBuilder.DropTable(
                name: "credentials",
                schema: "ssi-authority-schema-registry");

            migrationBuilder.DropTable(
                name: "credential_types",
                schema: "ssi-authority-schema-registry");
        }
    }
}
