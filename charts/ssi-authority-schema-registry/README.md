# Helm chart for SSI Authority & Schema Registry

This helm chart installs the Catena-X SSI Credential Authority & Schema Registry application.

For further information please refer to [Technical Documentation](./docs/technical-documentation).

The referenced container images are for demonstration purposes only.

## Installation

To install the chart with the release name `ssi-authority-schema-registry`:

```shell
$ helm repo add tractusx-dev https://eclipse-tractusx.github.io/charts/dev
$ helm install ssi-authority-schema-registry tractusx-dev/ssi-authority-schema-registry
```

To install the helm chart into your cluster with your values:

```shell
$ helm install -f your-values.yaml ssi-authority-schema-registry tractusx-dev/ssi-authority-schema-registry
```

To use the helm chart as a dependency:

```yaml
dependencies:
  - name: ssi-authority-schema-registry
    repository: https://eclipse-tractusx.github.io/charts/dev
    version: 0.0.1-rc.1
```

## Requirements

| Repository | Name | Version |
|------------|------|---------|
| https://charts.bitnami.com/bitnami | postgresql | 12.12.x |

## Values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| registry.image.name | string | `"docker.io/tractusx/ssi-authority-schema-registry-service"` |  |
| registry.image.tag | string | `""` |  |
| registry.imagePullPolicy | string | `"IfNotPresent"` |  |
| registry.resources | object | `{"limits":{"cpu":"45m","memory":"400M"},"requests":{"cpu":"15m","memory":"400M"}}` | We recommend to review the default resource limits as this should a conscious choice. |
| registry.logging.businessLogic | string | `"Information"` |  |
| registry.logging.default | string | `"Information"` |  |
| registry.healthChecks.startup.path | string | `"/health/startup"` |  |
| registry.healthChecks.startup.tags[0].name | string | `"HEALTHCHECKS__0__TAGS__1"` |  |
| registry.healthChecks.startup.tags[0].value | string | `"registrydb"` |  |
| registry.healthChecks.liveness.path | string | `"/healthz"` |  |
| registry.healthChecks.readyness.path | string | `"/ready"` |  |
| registry.swaggerEnabled | bool | `false` |  |
| migrations.name | string | `"migrations"` |  |
| migrations.image.name | string | `"docker.io/tractusx/ssi-authority-schema-registry-migrations"` |  |
| migrations.image.tag | string | `""` |  |
| migrations.imagePullPolicy | string | `"IfNotPresent"` |  |
| migrations.resources | object | `{"limits":{"cpu":"45m","memory":"200M"},"requests":{"cpu":"15m","memory":"200M"}}` | We recommend to review the default resource limits as this should a conscious choice. |
| migrations.seeding.testDataEnvironments | string | `""` |  |
| migrations.seeding.testDataPaths | string | `"Seeder/Data"` |  |
| migrations.logging.default | string | `"Information"` |  |
| existingSecret | string | `""` | Secret containing the client-secrets for the connection to portal and wallet as well as encryptionKeys for issuer.credential and processesworker.wallet |
| dotnetEnvironment | string | `"Production"` |  |
| dbConnection.schema | string | `"registry"` |  |
| dbConnection.sslMode | string | `"Disable"` |  |
| postgresql.enabled | bool | `true` | PostgreSQL chart configuration; default configurations: host: "registry-postgresql-primary", port: 5432; Switch to enable or disable the PostgreSQL helm chart. |
| postgresql.image | object | `{"tag":"15-debian-12"}` | Setting image tag to major to get latest minor updates |
| postgresql.commonLabels."app.kubernetes.io/version" | string | `"15"` |  |
| postgresql.auth.username | string | `"registry"` | Non-root username. |
| postgresql.auth.database | string | `"registry"` | Database name. |
| postgresql.auth.existingSecret | string | `"{{ .Release.Name }}-registry-postgres"` | Secret containing the passwords for root usernames postgres and non-root username registry. Should not be changed without changing the "registry-postgresSecretName" template as well. |
| postgresql.auth.postgrespassword | string | `""` | Password for the root username 'postgres'. Secret-key 'postgres-password'. |
| postgresql.auth.password | string | `""` | Password for the non-root username 'registry'. Secret-key 'password'. |
| postgresql.auth.replicationPassword | string | `""` | Password for the non-root username 'repl_user'. Secret-key 'replication-password'. |
| postgresql.architecture | string | `"replication"` |  |
| postgresql.audit.pgAuditLog | string | `"write, ddl"` |  |
| postgresql.audit.logLinePrefix | string | `"%m %u %d "` |  |
| postgresql.primary.extendedConfiguration | string | `""` | Extended PostgreSQL Primary configuration (increase of max_connections recommended - default is 100) |
| postgresql.primary.initdb.scriptsConfigMap | string | `"{{ .Release.Name }}-registry-cm-postgres"` |  |
| postgresql.readReplicas.extendedConfiguration | string | `""` | Extended PostgreSQL read only replicas configuration (increase of max_connections recommended - default is 100) |
| externalDatabase.host | string | `"registry-postgres-ext"` | External PostgreSQL configuration IMPORTANT: non-root db user needs to be created beforehand on external database. And the init script (02-init-db.sql) available in templates/configmap-postgres-init.yaml needs to be executed beforehand. Database host ('-primary' is added as postfix). |
| externalDatabase.port | int | `5432` | Database port number. |
| externalDatabase.username | string | `"registry"` | Non-root username for registry. |
| externalDatabase.database | string | `"registry"` | Database name. |
| externalDatabase.password | string | `""` | Password for the non-root username (default 'registry'). Secret-key 'password'. |
| externalDatabase.existingSecret | string | `"registry-external-db"` | Secret containing the password non-root username, (default 'registry'). |
| centralidp | object | `{"address":"https://centralidp.example.org","authRealm":"CX-Central","jwtBearerOptions":{"metadataPath":"/auth/realms/CX-Central/.well-known/openid-configuration","refreshInterval":"00:00:30","requireHttpsMetadata":"true","tokenValidationParameters":{"validAudience":"empty","validIssuerPath":"/auth/realms/CX-Central"}},"tokenPath":"/auth/realms/CX-Central/protocol/openid-connect/token","useAuthTrail":true}` | Provide details about centralidp (CX IAM) Keycloak instance. |
| centralidp.address | string | `"https://centralidp.example.org"` | Provide centralidp base address (CX IAM), without trailing '/auth'. |
| centralidp.useAuthTrail | bool | `true` | Flag if the api should be used with an leading /auth path |
| ingress.enabled | bool | `false` | SSI Credential Authority & Schema ingress parameters, enable ingress record generation for ssi-authority-schema-registry. |
| ingress.tls | list | `[]` | Ingress TLS configuration |
| ingress.hosts[0] | object | `{"host":"","paths":[{"backend":{"port":8080},"path":"/api","pathType":"Prefix"}]}` | Provide default path for the ingress record. |
| portContainer | int | `8080` |  |
| portService | int | `8080` |  |
| replicaCount | int | `3` |  |
| nodeSelector | object | `{}` | Node labels for pod assignment |
| tolerations | list | `[]` | Tolerations for pod assignment |
| affinity.podAntiAffinity | object | `{"preferredDuringSchedulingIgnoredDuringExecution":[{"podAffinityTerm":{"labelSelector":{"matchExpressions":[{"key":"app.kubernetes.io/name","operator":"DoesNotExist"}]},"topologyKey":"kubernetes.io/hostname"},"weight":100}]}` | Following Catena-X Helm Best Practices, [reference](https://kubernetes.io/docs/concepts/scheduling-eviction/assign-pod-node/#affinity-and-anti-affinity). |
| updateStrategy.type | string | `"RollingUpdate"` | Update strategy type, rolling update configuration parameters, [reference](https://kubernetes.io/docs/concepts/workloads/controllers/statefulset/#update-strategies). |
| updateStrategy.rollingUpdate.maxSurge | int | `1` |  |
| updateStrategy.rollingUpdate.maxUnavailable | int | `0` |  |
| startupProbe | object | `{"failureThreshold":30,"initialDelaySeconds":10,"periodSeconds":10,"successThreshold":1,"timeoutSeconds":1}` | Following Catena-X Helm Best Practices, [reference](https://github.com/helm/charts/blob/master/stable/nginx-ingress/values.yaml#L210). |
| livenessProbe.failureThreshold | int | `3` |  |
| livenessProbe.initialDelaySeconds | int | `10` |  |
| livenessProbe.periodSeconds | int | `10` |  |
| livenessProbe.successThreshold | int | `1` |  |
| livenessProbe.timeoutSeconds | int | `10` |  |
| readinessProbe.failureThreshold | int | `3` |  |
| readinessProbe.initialDelaySeconds | int | `10` |  |
| readinessProbe.periodSeconds | int | `10` |  |
| readinessProbe.successThreshold | int | `1` |  |
| readinessProbe.timeoutSeconds | int | `1` |  |

Autogenerated with [helm docs](https://github.com/norwoodj/helm-docs)
