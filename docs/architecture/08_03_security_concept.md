# Security Assessment SSI Authority & Schema Registry

|                           |                                                                                                          |
| :------------------------ | :------------------------------------------------------------------------------------------------------- |
| Contact for product       | [@evegufy](https://github.com/evegufy) <br> [@jjeroch](https://github.com/jjeroch)                       |
| Security responsible      |                                                                                                          |
| Version number of product | 1.0.0                                                                                                    |
| Dates of assessment       | 24.07.2024                                                                                               |
| Status of assessment      | ASSESSMENT DONE & APPROVED                                                                               |

## Product Description

The SSI Authority & Schema Registry product is a readonly REST API project, so a pure backend component (without implementation of an user interface).

The main purpose of the product is to provide the possibility to receive information about available credentials and their authorities. Furthermore, it can validate credential schemas.

The SSI Authority & Schema Registry comprises the technical foundation for functional interaction, monitoring and further functionalities.

The product can be run anywhere: it can be deployed as a docker image, e.g. on Kubernetes (platform-independent, cloud, on prem or local).

The SSI Authority & Schema Registry is using following key frameworks:

- .Net
- Entity Framework

[Development Concept](./Development%20Concept.md)

## Data Flow Diagram

```mermaid
flowchart LR

    C(Customer)
    RS(Registry Service)
    PHD[(Registry DB \n Postgres \n EF Core for mapping \n objects to SQL)]

   
    subgraph SSI-Authority-Schema-Registry Product
     RS
     PHD
    end

    C-->|"Request data"|RS
    RS-->|"Read credential and authority data \n validate schema of credentials"|PHD
```

### Additional information

- The customer can request the available credentials with their providing authorities.
- It is possible to validate the schema of credentials.
- All actions are logged within the Registry DB.

### Changes compared to last Security Assessment

N/A

### Features for Upcoming Versions

N/A

## Threats & Risks

N/A - No direct threats & vulnerabilities detected during the assessment, taking into account already implemented security controls & requirements.

### Mitigated Threats

N/A

### Implemented Security Controls

- Authentication & Authorization Concept
  - As per the Association "business requirements", SSI Authority & Schema Registry Product is publicly available, even if the customers are not part of Catena-X. Therefore no authentication, authorization & session management concepts were implemented.  
- Data Storage & Encryption
  - Data Stored within the Registry DB (Postgres) is publicly available and not confidential information. No encryption requirements for data at rest is required.
- API Security
  - API is publicly available and may be accessed by Everyone.
  - Two endpoints are available, for both functionalities of the application :
      1. Read-only Get Endpoint - to receive information about available credentials and their authorities
      2. Read-only Post Endpoint - to validate credential schemas
  - Rate limiting configuration is available, once properly configured by the "Operating Company" will grant the availability controls for the application.
- Logging & Monitoring
  - All actions & requests performed by the Customers are logged in and stored within the application database with possibility for further audit, investigation & active monitoring, which may be configured by the "Operating Company"

### Performed Security Checks

- Static Application Security Testing (SAST) - CodeQL
- Software Composition Analysis (SCA) - Dependabot
- Container Scan conducted - Trivy
- Infrastructure as Code - KICS
- Secret Scanning - GitGuardian
- Dynamic Application Security Testing (DAST) - OWASP ZAP (Unauthenticated)

## NOTICE

This work is licensed under the [Apache-2.0](https://www.apache.org/licenses/LICENSE-2.0).

- SPDX-License-Identifier: Apache-2.0
- SPDX-FileCopyrightText: 2024 Contributors to the Eclipse Foundation
- Source URL: <https://github.com/eclipse-tractusx/ssi-authority-schema-registry>
