namespace Fanex.Logging.Samples.WebApi
{
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using Filters;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var formaters = GlobalConfiguration.Configuration.Formatters;
            formaters.Clear();
            formaters.Add(new JsonMediaTypeFormatter());

            config.Filters.Add(new ExceptionFilter());
        }
    }
}