using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using Microsoft.ApplicationInsights.Extensibility;

namespace WebAPIIntegrationTests
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            TelemetryConfiguration.Active.InstrumentationKey =
               ConfigurationManager.AppSettings["InstrumentationKey"];
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
