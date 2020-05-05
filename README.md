#<span>ASP.NET</span> Authentication

- [OAuth Authentication](#oauth-authentication)
  - [Notes](#notes)
  - [Nugets Installation](#nugets-installation)
  - [Postman testing](#postman-testing)
  - [References](#references)
- [Access OAuth REST Web API](#access-oauth-rest-web-api)
  - [Notes](#notes-1)
  - [Nugets](#nugets)
  - [References](#references-1)

## OAuth Authentication

- Execution test Url --> https://localhost:44340/api/webapi
- Token generation url --> https://localhost:44340/token

### Notes
- Install necessary Nugets
- Create the necessary tables and stored procedures
- [Authorize] attribute should be added to controller
- Add authentication configuration in the startup.cs
```csharp
  // Web API configuration and services  
  // Configure Web API to use only bearer token authentication.  
  config.SuppressDefaultHostAuthentication();  
  config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));  
```
- Json format for the response
```csharp
  // WebAPI when dealing with JSON & JavaScript!  
  // Setup json serialization to serialize classes to camel (std. Json format)  
  var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;  
  formatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(); 
```
- Implementation authentication in start.auth.cs
- Implementation authentication provider in AppOAuthProvider    
            
### Nugets Installation

- Microsoft.Owin.Security.OAuth
- Microsoft.Owin.Cors
- Microsoft.AspNet.WebApi.Core
- Microsoft.AspNet.WebApi.Owin
- Microsoft.Owin.Security.Cookies
- Microsoft.AspNet.Identity.Core
- Microsoft.AspNet.Identity.Owin
- Microsoft.Owin.Host.SystemWeb

### Postman testing

Access Token Generation
![Access Token Generation](/doc/accessToken.png)

WebApi Authentication
![WebApi Authentication](/doc/WebApi.png)

### References

1. https://www.c-sharpcorner.com/article/asp-net-mvc-oauth-2-0-rest-web-api-authorization-using-database-first-approach/
2. https://docs.microsoft.com/en-us/previous-versions/aspnet/dn308223(v=vs.113)?redirectedfrom=MSDN


## Access OAuth REST Web API

### Notes
- Get Authorization code using the client credentials
- Parse the result and get access token
- Send the access token as an authourization header

### Nugets
- Newtonsoft.Json

### References
1. https://www.c-sharpcorner.com/article/c-sharp-net-access-oauth-rest-web-api-method/