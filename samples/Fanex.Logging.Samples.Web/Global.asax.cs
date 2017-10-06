namespace Fanex.Logging.Samples.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging.Extensions.Web;
    using static Fanex.Logging.Extensions.Database.DatabaseInfoGetters;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //DbSettartiderManager.Current.SetConfig();
            // Fanex.Data version 2
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
                .GetHttpHeaderInfo())
                // To Enable Log Http Session
                //.GetHttpSessionInfo())
                .UseGetDbExceptionInfo(GetDbExceptionInfo);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ExceptionHandler.Handle(
                                Server.GetLastError()
                                // To Write Custom Data
                                // new Dictionary<string, object> { ["user id"] = 1111111 }
                                );
        }
    }
}