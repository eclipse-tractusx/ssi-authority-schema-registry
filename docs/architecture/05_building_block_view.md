# Building Block View

## Summary

In the following image you see the overall system overview of the SSI Authority & Schema Registry

```mermaid
flowchart LR

    C(Customer)
    ING(Ingress)
    RS(Registry Service)
    PHD[("Postgres Database \n \n (Base data created with \n application seeding)")]

    subgraph SSI Authority & Schema Registry Product
        ING
        PHD
        RS
    end

    C-->|"Requests data"|ING
    ING-->|"Forward Request"|RS
    RS-->|"Reads credentials & authorities"|PHD

```

## NOTICE

This work is licensed under the [Apache-2.0](https://www.apache.org/licenses/LICENSE-2.0).

- SPDX-License-Identifier: Apache-2.0
- SPDX-FileCopyrightText: 2024 Contributors to the Eclipse Foundation
- Source URL: <https://github.com/eclipse-tractusx/ssi-authority-schema-registry>
