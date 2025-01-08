# {{ project-title }}

**// TODO:** Add description of your project's business function.

Generated from the [.NET REST Domain Gateway Archetype](https://github.com/p6m-dev/dotnet-rest-domain-gateway.archetype).

[[_TOC_]]

## Prereqs
Running this service requires .NET 8+ and NuGet to be configured with an Artifactory encrypted key. 
For development, be sure to have Docker installed and running locally.

## Overview


## Build System
This project uses [dotnet](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet#general) as its build system. Common goals include

| Goal    | Description                                        |
|---------|----------------------------------------------------|
| clean   | Clean build outputs.                               |
| build   | Builds a .NET application.                         |
| restore | Restores the dependencies for a given application. |
| run     | Runs the application from source                   |
| test    | Runs tests using a test runner                     |

## **Access Token Setup**
1. Login to [JFrog Artifactory](https://p6m.jfrog.io/)
2. Once logged in, click the `Profile Icon` (top-right corner) > click `Edit Profile` > click `Generate an Identity Token`
3. Add a descriptive name (ie: PLATFORM_TOKEN-front-end-apps) > click `Next` > copy the `Username` and `Reference Token` (for the next step)
4. Add the `Username` and `Reference Token` to your local machine environment variables (for Macs, it usually goes to `~/.zshrc` or `~/.zprofile`)
```bash
export ARTIFACTORY_USERNAME="...."        # generated 'Username'
export ARTIFACTORY_IDENTITY_TOKEN="****"  # generated 'Reference Token'
export ARTIFACTORY_TOKEN=$(echo -n "$ARTIFACTORY_USERNAME:$ARTIFACTORY_IDENTITY_TOKEN" | base64)
```
   - If changes were made to `~/.zshrc`: run `source ~/.zshrc` and then `restart your terminal` to apply the new environment variable changes
   - If changes were made to `~/.zprofile`: run `source ~/.zprofile` and then `restart your terminal` to apply the new environment variable changes


## NuGet with JFrog Artifactory Setup
```bash
dotnet nuget add source --name "Artifactory" --username ${ARTIFACTORY_USERNAME} --password ${ARTIFACTORY_IDENTITY_TOKEN} --store-password-in-clear-text "https://p6m.jfrog.io/artifactory/api/nuget/{{ org_name }}-{{ solution-name }}-nuget"
```

## Running the Server
This server accepts connections on the following ports:
- {{ service-port }}: used for application REST Service traffic.
- {{ management-port }}: used to monitor the application over HTTP (see [Actuator endpoints](https://docs.spring.io/spring-boot/docs/current/reference/html/actuator.html#actuator.endpoints)).
- {{ debug-port }}: remote debugging port


Next, start the server locally or using Docker. You can verify things are up and running by looking at the [/health](http://localhost:{{ management-port }}/health) endpoint:
```bash
curl localhost:{{ management-port }}/health
```

### Local
From the project root, run the server:
```bash
dotnet run --project {{ ProjectName }}.Server
```

### OpenAPI
Swagger UI - [/swagger](http://localhost:{{ service-port }}/swagger) 

### Authentication/Authorization
By Default Authentication and Authorization is enabled to use OAuth2.0 with JWT Bearer.
You need to update `appconfig.json` or `appconfig.{ENVIRONMENT}.json` files to set proper OAuth2.0 Authorization Server URL.
Here is Keycloak example:
```
  "Security": {
    "OAuth2": {
      "Authority": "http://localhost:8080/realms/<your_realm>",
      "AuthorizationUrl": "http://localhost:8080/realms/<your_realm>/protocol/openid-connect/auth",
      "TokenUrl": "http://localhost:8080/realms/<your_realm>/protocol/openid-connect/token",
      "ClientId": "<your_client_id>"
    }
  }
```
`Authority` is used by `JwtBearerHandler` to pull OpenId Connect configuration.
`AuthorizationUrl`, `TokenUrl` and `ClientId` are used by `AuthSwaggerExtensions` to be able to provider `Authorize` button on Swagger UI.
Swagger UI use Standard Authorization Code Flow to be able to authenticate a user.

#### Authorization Checks
`[Authorize]` annotation on the Controller class or methods, will enforce only authenticated users to be 
able to access your methods in the controller.
```
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class MyController() : ControllerBase
{
}
```

`[Authorize(Roles = "admin")]` checks that currently authenticated user has certain roles
```
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class MyController() : ControllerBase
{
    [Authorize(Roles = "admin")]
    public Foo? Foo(Guid id)
    {
        return service.GetFoo(id);
    }
}
```
`[AllowAnonymous]` annotation allows unauthenticated users to access your API.
```
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class MyController() : ControllerBase
{
    [AllowAnonymous]
    public string publicAPI()
    {
        return "public API";
    }
}
```


### Metrics
Prometheus - [Prometheus](https://github.com/prometheus-net/prometheus-net)

You can verify things are up and running by looking at the [/metrics](http://localhost:{{ management-port }}/metrics) endpoint:
```bash
curl localhost:{{ management-port }}/metrics
```


## Modules (UPDATE)

| Directory                                                                 | Description                                                                                |
|---------------------------------------------------------------------------|--------------------------------------------------------------------------------------------|
| [{{ ProjectName }}.Core]({{ ProjectName }}.Core/README.md)                            | Business Logic. Abstracts Persistence, defines Transaction Boundaries. Implements the API. |
| [{{ ProjectName }}.IntegrationTests]({{ ProjectName }}.IntegrationTests/README.md)    | Leverages the Client to test the Server and it's dependencies.                             |
| [{{ ProjectName }}.Server]({{ ProjectName }}.Server/README.md)                        | Transport/Protocol Host.  Wraps Core.                                                      |

## Contributions
**// TODO:** Add description of how you would like issues to be reported and people to reach out.