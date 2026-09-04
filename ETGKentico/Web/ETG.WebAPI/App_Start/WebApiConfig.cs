using Newtonsoft.Json.Serialization;
using System;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.WebHost;

namespace ETG.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IDependencyResolver dependencyResolver)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler).GetField("_instance",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            }

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.DependencyResolver = dependencyResolver;

            config.EnableSystemDiagnosticsTracing();
            config.EnableCors();
        }
    }
}
