# Database Migration Documentation

## Overview

This document describes the database migration structure and history for the SSI Authority Schema Registry project. The project uses Entity Framework Core with PostgreSQL as the database system.

## Database Schema Details

### Schema Information
- Name: `ssi-authority-schema-registry`
- Default Collation: `en_US.utf8`
- Database Provider: PostgreSQL

### Core Tables

#### 1. authorities
- Primary Key: `did` (text)
- Purpose: Stores authority information using decentralized identifiers
- Relations: One-to-many with credential_authorities

#### 2. credential_types
- Primary Key: `id` (integer)
- Fields:
  - `label` (text, required)
- Predefined Values:
  - BusinessPartnerNumber (id: 1)
  - Membership (id: 2)
  - Framework (id: 3)
  - CompanyRole (id: 4)

#### 3. credentials
- Primary Key: `id` (uuid)
- Fields:
  - `name` (text, required)
  - `type_id` (integer, FK to credential_types)
- Relations:
  - Many-to-one with credential_types
  - Many-to-many with authorities through credential_authorities

#### 4. credential_authorities
- Composite Primary Key: (`credential_id`, `did`)
- Fields:
  - `credential_id` (uuid, FK to credentials)
  - `did` (text, FK to authorities)
- Purpose: Junction table implementing many-to-many relationship

## Migration History

### Version 0.1.0-rc.1 (20240305100130)
Initial schema creation with:
- Base table structure setup
- Foreign key relationships
- Initial credential types data

### Version Net9Upgrade (20250205142131)
- Framework upgrade to .NET 9.0.1
- Foreign key constraint optimizations

### Version RenameBpnToDid (20250306085119)
- Column rename from `bpn` to `did`
- Updated foreign key references
- Modified related indexes

## Best Practices

### 1. Naming Conventions

- Database Objects
  - Use `snake_case` naming convention
  - Examples:
    - Tables: `credential_types`, `credential_authorities`
    - Columns: `credential_id`, `type_id`

- Constraints and Indexes
  - Primary Keys: prefix with `pk_`
    - Example: `pk_authorities`, `pk_credential_types`
  - Foreign Keys: prefix with `fk_`
    - Example: `fk_credentials_credential_types_type_id`
  - Indexes: prefix with `ix_`
    - Example: `ix_credentials_type_id`

### 2. Data Integrity

- Foreign Key Constraints
  - Explicitly define relationships between tables
  - Use appropriate ON DELETE rules
    - Cascade: When child records should be deleted with parent
    - No Action: When referential integrity should prevent deletion
  - Example:
    ```sql
    FOREIGN KEY (type_id) REFERENCES credential_types(id) ON DELETE CASCADE
    ```

- Index Strategy
  - Create indexes on:
    - Foreign key columns
    - Frequently queried columns
    - Search/filter fields
  - Consider composite indexes for multi-column queries

### 3. Version Control

- Migration Management
  - Each migration is a separate file with unique timestamp
  - Files contain both Up() and Down() methods for reversibility
  - Clear naming convention: `{Timestamp}_{Description}.cs`

- Documentation Requirements
  - Purpose of migration
  - List of changes
  - Any data transformations
  - Breaking changes or special deployment considerations

## Migration Management

### Creating New Migrations
```bash
dotnet ef migrations add MigrationName --project src/database/SsiAuthoritySchemaRegistry.Migrations
```

### Applying Migrations
```bash
dotnet ef database update --project src/database/SsiAuthoritySchemaRegistry.Migrations
```

## NOTICE

This work is licensed under the [Apache-2.0](https://www.apache.org/licenses/LICENSE-2.0).

- SPDX-License-Identifier: Apache-2.0
- SPDX-FileCopyrightText: 2025 Contributors to the Eclipse Foundation
- Source URL: <https://github.com/eclipse-tractusx/ssi-authority-schema-registry>