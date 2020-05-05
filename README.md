<span>ASP.NET</span> Authentication

- [OAuth Authentication](#oauth-authentication)
  - [Nugets Installation](#nugets-installation)
  - [References](#references)
  - [Postman testing](#postman-testing)

# OAuth Authentication

- Execution test Url --> https://localhost:44340/api/webapi

## Nugets Installation

- Microsoft.Owin.Security.OAuth
- Microsoft.Owin.Cors
- Microsoft.AspNet.WebApi.Core
- Microsoft.AspNet.WebApi.Owin
- Microsoft.Owin.Security.Cookies
- Microsoft.AspNet.Identity.Core
- Microsoft.AspNet.Identity.Owin
- Microsoft.Owin.Host.SystemWeb

## References

1. https://www.c-sharpcorner.com/article/asp-net-mvc-oauth-2-0-rest-web-api-authorization-using-database-first-approach/
2. https://docs.microsoft.com/en-us/previous-versions/aspnet/dn308223(v=vs.113)?redirectedfrom=MSDN

## Postman testing

![Access Token Generation](../doc/accessToken.png)

![WebApi Authentication](../doc/WebApi.png)
