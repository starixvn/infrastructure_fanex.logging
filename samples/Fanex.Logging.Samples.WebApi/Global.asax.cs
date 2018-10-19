namespace Fanex.Logging.Samples.WebApi
{
    using System.Configuration;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Data;
    using Extensions.Database;
    using Extensions.Web;
    using Fanex.Logging.RabbitMQ;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var connectionStrings = ConfigurationManager.ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .ToDictionary(connection => connection.Name, connection => connection.ConnectionString);

            DbSettingProviderManager
               .StartNewSession()
               .UseConnectionStrings(connectionStrings)
               .UseDefaultDbSettingProvider(
                   resourcePath: "~/App_Data/")
               .Run();

            LogManager
                    .SetDefaultLogCategory("Fanex-Logging-Test-new")
                    .Use(new RabbitMQLogging("host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw=="));

            ExceptionHandler
                // To use custom config
                .UseConfig(new LoggingConfiguration()
                // To ignore exceptions
                //.IgnoreException<DivideByZeroException>()
                // To hide sensitive information
                //.HideSensitiveInfo(
                //    new List<string>
                //    {
                //        "(?:password=)(?<paramValue>[^&|^\\s]+)",
                //        "(\"password\":)(?<paramValue>[^,|^\r]+)"
                //    })
                // To Enable Log Http Header
                .GetHttpHeaderInfo()
                // To Enable Log Http Session
                .GetHttpSessionInfo())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
        }
    }
}