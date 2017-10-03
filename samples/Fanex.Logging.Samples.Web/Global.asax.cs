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
                // To ignore exceptions
                //.UseConfig(new LoggingConfiguration().IgnoreException<DivideByZeroException>())
                // To ignore exceptions
                //.UseConfig(new LoggingConfiguration().HideSensitiveInfo(new List<string> { "password", "securitycode" }))
                .UseGetDbExceptionInfo(GetDbExceptionInfo);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ExceptionHandler
                .Handle(Server.GetLastError());
        }
    }
}