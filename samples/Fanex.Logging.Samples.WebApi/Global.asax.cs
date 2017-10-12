namespace Fanex.Logging.Samples.WebApi
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Data;
    using Data.CrossCutting.WrappingException;
    using Data.OldConnectionStringProvider;
    using Extensions.Database;
    using Extensions.Web;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbSettingProviderManager
               .StartNewSession()
               .Use(new OldConnectionStringProvider())
               .UseDefaultDbSettingProvider(
                   resourcePath: "~/App_Data/")
               .Use((settingKey, commandTimeout) => new WrappingExceptionObjectDb(settingKey, commandTimeout))
               .Run();

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