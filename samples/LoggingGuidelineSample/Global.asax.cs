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
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;

    public class MvcApplication : System.Web.HttpApplication
    {
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

            var sensitiveParams = new List<string>
            {
                "password",
                "securitycode"
            };

            var errorInfo = exception
                        .Message
                        .AddRequestInfo(HttpContext.Current.Request)
                        .CensorSensitiveParams(
                                sensitiveParams, 
                                "(?:{0}=)(?<paramValue>[^&|^\\s]+)", // For query string & form
                                "(\"{0}\":)(?<paramValue>[^,|^\r]+)") // For body json data
                        .AddSqlInfo(exception)
                        .AddWebServerInfo()
                        .AddExceptionInfo(exception)
                        .AddCustomInfo(
                            "USER INFO",
                            "User Id: dummyId",
                            "Username: dummyname");

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