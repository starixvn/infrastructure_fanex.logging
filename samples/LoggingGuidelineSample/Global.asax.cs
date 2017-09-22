namespace LoggingGuidelineSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;

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

            WebApplicationExceptionHandler
             .Configure(new WebLoggingConfiguration()
                .UseGetDatabaseInfoFunc(DatabaseLoggingExtensions.GetSqlExceptionInfo));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            // Don't write log for HTTP exception

            //var customInfo = LogUtility.CombineInfo(
            //                            "USER INFO",
            //                            "User Id: dummyId",
            //                            "Username: dummyname");

            WebApplicationExceptionHandler.Handle(exception);

            //WebApplicationExceptionHandler.Handle(exception, customInfo);
        }
    }
}