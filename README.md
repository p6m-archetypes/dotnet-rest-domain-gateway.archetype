# .NET REST Domain Gateway Archetype

## Usage

To get started, [install archetect](https://github.com/p6m-dev/development-handbook)
and render this template to your current working directory:

```bash
archetect render git@github.com:p6m-dev/dotnet-grpc-service.archetype.git
```

For information about interacting with the service, refer to the README at the generated
project's root.

## Prompts

When rendering the archetype, you'll be prompted for the following values:

| Property          | Description                                                                                                         | Example                       |
| ----------------- | ------------------------------------------------------------------------------------------------------------------- | ----------------------------- |
| `org-name`        | Organization Name                                                                                                   | afi, cpd, a1p                 |
| `solution-name`   | Solution Name                                                                                                       | apps, xyz                     |
| `prefix`          | General name that represents the service domain that is used to set the entity, service, and RPC stub names         | invoice, order, booking       |

For a list of all derived properties and examples of the property relationships, see [archetype.yml](./archetype.yml).

## What's Inside

Features include:
- Simple CRUD over REST API
- Authentication and Authorization setup
- Integration with gRPC service 
- Docker image publication to Artifactory
- Open API (Swagger UI)
- Integration tests
- GitHub Actions SDLC pipelines
- Kubernetes manifests
- Open Telemetry Configuration
- Serilog
- Heath Checks Endpoint
