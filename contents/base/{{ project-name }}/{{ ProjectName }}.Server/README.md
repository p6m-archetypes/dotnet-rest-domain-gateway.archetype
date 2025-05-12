# {{ ProjectName }}.Server


## Authentication/Authorization
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