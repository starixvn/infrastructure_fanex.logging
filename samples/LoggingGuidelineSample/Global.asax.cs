namespace LoggingGuidelineSample
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging;
    using Fanex.Logging.WebExtensions;

    public class MvcApplication : System.Web.HttpApplication
    {
        private static IEnumerable<string> sampleSensitiveParams = new List<string>
        {
            "password",
            "securitycode"
        };

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //DbSettingProviderManager.Current.Start();
            // Fanex.Data version 2
            DbSettingProviderManager
                .StartNewSession()
                .Use(new OldConnectionStringProvider())
                .UseDefaultDbSettingProvider(
                    resourcePath: "~/App_Data/")
                .Use((settingKey, commandTimeout) => new WrappingExceptionObjectDb(settingKey, commandTimeout))
                .Run();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            // Don't write log for HTTP exception
            if (exception is HttpException)
            {
                return;
            }

            //// Custom Section For each site logs more information.
            var customLogSection = LogUtility.CombineInfo(
                                            "USER INFO",
                                            "User Id: dummyId",
                                            "Username: dummyname");

            var errorInfo = WebLoggingUtility.BuildLogInfo(
                exception.Message, 
                exception, 
                customLogSection, 
                sampleSensitiveParams);

            LogService.Error(errorInfo);

#if DEBUG
            //// this code for dev to view message on browser, dont bring it to production
            HttpContext.Current.Response.Write(errorInfo.Replace("\n", "<br/>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Close();
#endif
        }
    }
}