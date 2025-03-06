/********************************************************************************
 * Copyright (c) 2025 Cofinity-X GmbH
 * Copyright (c) 2025 Contributors to the Eclipse Foundation
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

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RenameBpnToDid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_credential_authorities_authorities_bpn",
                schema: "ssi-authority-schema-registry",
                table: "credential_authorities");

            migrationBuilder.RenameColumn(
                name: "bpn",
                schema: "ssi-authority-schema-registry",
                table: "credential_authorities",
                newName: "did");

            migrationBuilder.RenameIndex(
                name: "ix_credential_authorities_bpn",
                schema: "ssi-authority-schema-registry",
                table: "credential_authorities",
                newName: "ix_credential_authorities_did");

            migrationBuilder.RenameColumn(
                name: "bpn",
                schema: "ssi-authority-schema-registry",
                table: "authorities",
                newName: "did");

            migrationBuilder.AddForeignKey(
                name: "fk_credential_authorities_authorities_did",
                schema: "ssi-authority-schema-registry",
                table: "credential_authorities",
                column: "did",
                principalSchema: "ssi-authority-schema-registry",
                principalTable: "authorities",
                principalColumn: "did");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_credential_authorities_authorities_did",
                schema: "ssi-authority-schema-registry",
                table: "credential_authorities");

            migrationBuilder.RenameColumn(
                name: "did",
                schema: "ssi-authority-schema-registry",
                table: "credential_authorities",
                newName: "bpn");

            migrationBuilder.RenameIndex(
                name: "ix_credential_authorities_did",
                schema: "ssi-authority-schema-registry",
                table: "credential_authorities",
                newName: "ix_credential_authorities_bpn");

            migrationBuilder.RenameColumn(
                name: "did",
                schema: "ssi-authority-schema-registry",
                table: "authorities",
                newName: "bpn");

            migrationBuilder.AddForeignKey(
                name: "fk_credential_authorities_authorities_bpn",
                schema: "ssi-authority-schema-registry",
                table: "credential_authorities",
                column: "bpn",
                principalSchema: "ssi-authority-schema-registry",
                principalTable: "authorities",
                principalColumn: "bpn");
        }
    }
}
