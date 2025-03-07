# Helm chart for SSI Authority & Schema Registry

This helm chart installs the SSI Authority & Schema Registry application.

For further information please refer to [Technical Documentation](/docs/admin).

The referenced container images are for demonstration purposes only.

## Installation

To install the chart with the release name `ssi-asr`:

```shell
$ helm repo add tractusx-dev https://eclipse-tractusx.github.io/charts/dev
$ helm install ssi-asr tractusx-dev/ssi-asr
```

To install the helm chart into your cluster with your values:

```shell
$ helm install -f your-values.yaml ssi-asr tractusx-dev/ssi-asr
```

To use the helm chart as a dependency:

```yaml
dependencies:
  - name: ssi-asr
    repository: https://eclipse-tractusx.github.io/charts/dev
    version: 1.2.0
```

## Requirements

| Repository | Name | Version |
|------------|------|---------|
| https://charts.bitnami.com/bitnami | postgresql | 12.12.x |

## Values

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| authorities | object | `{"authorityOne":{"did":"did:web:example.org:api:administration:staticdata:did:BPNL00000003CRHK"},"authorityTwo":{"did":"did:web:example.org:api:administration:staticdata:did:BPNL00000003CRHL"}}` | Set information related the authorities |
| authorities.authorityOne | object | `{"did":"did:web:example.org:api:administration:staticdata:did:BPNL00000003CRHK"}` | The first authority |
| authorities.authorityTwo | object | `{"did":"did:web:example.org:api:administration:staticdata:did:BPNL00000003CRHL"}` | The second authority |
| service.image.name | string | `"docker.io/tractusx/ssi-authority-schema-registry-service"` |  |
| service.image.tag | string | `""` |  |
| service.image.pullSecrets | list | `[]` |  |
| service.imagePullPolicy | string | `"IfNotPresent"` |  |
| service.resources | object | `{"limits":{"cpu":"45m","memory":"400M"},"requests":{"cpu":"15m","memory":"400M"}}` | We recommend to review the default resource limits as this should a conscious choice. |
| service.logging.businessLogic | string | `"Information"` |  |
| service.logging.default | string | `"Information"` |  |
| service.healthChecks.startup.path | string | `"/health/startup"` |  |
| service.healthChecks.startup.tags[0].name | string | `"HEALTHCHECKS__0__TAGS__1"` |  |
| service.healthChecks.startup.tags[0].value | string | `"registrydb"` |  |
| service.healthChecks.liveness.path | string | `"/healthz"` |  |
| service.healthChecks.readyness.path | string | `"/ready"` |  |
| service.swaggerEnabled | bool | `false` |  |
| migrations.name | string | `"migrations"` |  |
| migrations.image.name | string | `"docker.io/tractusx/ssi-authority-schema-registry-migrations"` |  |
| migrations.image.tag | string | `""` |  |
| migrations.image.pullSecrets | list | `[]` |  |
| migrations.imagePullPolicy | string | `"IfNotPresent"` |  |
| migrations.resources | object | `{"limits":{"cpu":"75m","memory":"200M"},"requests":{"cpu":"25m","memory":"200M"}}` | We recommend to review the default resource limits as this should a conscious choice. |
| migrations.seeding.useInitial | bool | `true` | Enables dynamic seeding of information related to the operator company: operator.did; If set to `true` the data configured in the config map 'configmap-seeding-initialdata.yaml' will be taken to insert the initial data; |
| migrations.logging.default | string | `"Information"` |  |
| dotnetEnvironment | string | `"Production"` |  |
| dbConnection.schema | string | `"asr"` |  |
| dbConnection.sslMode | string | `"Disable"` |  |
| postgresql.enabled | bool | `true` | PostgreSQL chart configuration; default configurations: host: "asr-postgresql-primary", port: 5432; Switch to enable or disable the PostgreSQL helm chart. |
| postgresql.image | object | `{"tag":"15-debian-12"}` | Setting image tag to major to get latest minor updates |
| postgresql.commonLabels."app.kubernetes.io/version" | string | `"15"` |  |
| postgresql.auth.username | string | `"asr"` | Non-root username. |
| postgresql.auth.database | string | `"asr"` | Database name. |
| postgresql.auth.postgresPassword | string | `""` | Password for the root username 'postgres'. Secret-key 'postgres-password'. |
| postgresql.auth.password | string | `""` | Password for the non-root username 'asr'. Secret-key 'password'. |
| postgresql.audit.pgAuditLog | string | `"write, ddl"` |  |
| postgresql.audit.logLinePrefix | string | `"%m %u %d "` |  |
| postgresql.primary.extendedConfiguration | string | `""` | Extended PostgreSQL Primary configuration (increase of max_connections recommended - default is 100) |
| postgresql.primary.initdb.scriptsConfigMap | string | `"{{ .Release.Name }}-asr-cm-postgres"` |  |
| postgresql.readReplicas.extendedConfiguration | string | `""` | Extended PostgreSQL read only replicas configuration (increase of max_connections recommended - default is 100) |
| externalDatabase.host | string | `"asr-postgres-ext"` | External PostgreSQL configuration IMPORTANT: non-root db user needs to be created beforehand on external database. And the init script (02-init-db.sql) available in templates/configmap-postgres-init.yaml needs to be executed beforehand. Database host ('-primary' is added as postfix). |
| externalDatabase.port | int | `5432` | Database port number. |
| externalDatabase.username | string | `"asr"` | Non-root username for asr. |
| externalDatabase.database | string | `"asr"` | Database name. |
| externalDatabase.password | string | `""` | Password for the non-root username (default 'asr'). Secret-key 'password'. |
| externalDatabase.existingSecret | string | `""` | Existing secret containing the password non-root username. |
| ingress.enabled | bool | `false` | SSI Authority & Schema ingress parameters, enable ingress record generation for ssi-authority-schema-registry. |
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
