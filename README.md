# SSI-Authority-Schema-Registry

This repository contains the backend code for the SSI Authority & Schema Registry (SSI ASR) written in C#.

For **information about the SSI Authority & Schema Registry**, please refer to the documentation, especially the context and scope section in the [architecture documentation](./docs/architecture/Index.md).

For **installation** details, please refer to the [README.md](./charts/ssi-asr/README.md) of the provided helm chart.

## How to build and run

Install the [.NET 8.0 SDK](https://www.microsoft.com/net/download).

Run the following command from the CLI:

```console
dotnet build src
```

Make sure the necessary config is added to the settings of the service you want to run.
Run the following command from the CLI in the directory of the service you want to run:

```console
dotnet run
```

## Notice for Docker image

This application provides container images for demonstration purposes.

See Docker notice files for more information:

- [ssi-authority-schema-registry-service](./docker//notice-registry-service.md)
- [ssi-authority-schema-registry-migrations](./docker/notice-registry-migrations.md)

## Contributing

See [Contribution details](/docs/admin/dev-process/How%20to%20contribute.md).

## License

Distributed under the Apache 2.0 License.
See [LICENSE](./LICENSE) for more information
