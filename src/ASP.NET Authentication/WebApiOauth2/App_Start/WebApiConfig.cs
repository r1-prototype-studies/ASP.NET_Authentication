using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApiOauth2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Configure Web API to use only bearer token authentication.  
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(Microsoft.Owin.Security.OAuth.OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // WebAPI when dealing with JSON and Javascript
            // Setup JSON serialization 
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            config.Formatters.Clear();
            config.Formatters.Add(formatter);
        }
    }
}
