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
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Entities;
using Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities.Enums;

namespace Org.Eclipse.TractusX.SsiAuthoritySchemaRegistry.Entities;

public class RegistryContext : DbContext
{
    public RegistryContext()
    {
    }

    public RegistryContext(DbContextOptions<RegistryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Credential> Credentials { get; set; } = default!;
    public virtual DbSet<CredentialAuthority> CredentialAuthorities { get; set; } = default!;
    public virtual DbSet<Authority> Authorities { get; set; } = default!;
    public virtual DbSet<CredentialType> CredentialTypes { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");
        modelBuilder.HasDefaultSchema("ssi-authority-schema-registry");

        modelBuilder.Entity<Authority>(e =>
        {
            e.HasKey(a => a.Bpn);
        });

        modelBuilder.Entity<Credential>(e =>
            {
                e.HasMany(c => c.Authorities)
                    .WithOne(c => c.Credential)
                    .HasForeignKey(c => c.CredentialId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            }
        );

        modelBuilder.Entity<CredentialAuthority>(e =>
        {
            e.HasKey(ca => new { ca.CredentialId, ca.Bpn });

            e.HasOne(ca => ca.Credential)
                .WithMany(c => c.Authorities)
                .HasForeignKey(ca => ca.CredentialId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            e.HasOne(ca => ca.Authority)
                .WithMany(c => c.CredentialAuthorities)
                .HasForeignKey(ca => ca.Bpn)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CredentialType>().HasData(
            Enum.GetValues(typeof(CredentialTypeId))
                .Cast<CredentialTypeId>()
                .Select(e => new CredentialType(e))
        );
    }
}
