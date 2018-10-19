namespace Fanex.Logging.Samples.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Data;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Logging.RabbitMQ;
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
                .UseConnectionStrings(new Dictionary<string, string>
                {
                    { "DBLOG.bodb_CustomerLog.bodbAdminNet", "packet size=4096; Data Source=10.18.200.232;Initial Catalog=bodb_OER;integrated security=false; User Id=bodbAdminNet;Password=i9hNnaZltx04XF6oTNOKTnvvtLC/zfxr;" }
                })
                .UseDefaultDbSettingProvider(
                    resourcePath: "~/App_Data/")
                .Run();

            LogManager
                   .SetDefaultLogCategory("Fanex-Logging-Test-Web-new")
                   //.Use(new EntlibLogging(new EntLibConfig(
                   //    "FormatName:DIRECT=http://10.18.200.103:808/msmq/Private$/entlib",
                   //    @"~\Logs\")));
                   .Use(new RabbitMQLogging("host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw=="));

            ExceptionHandler
                // To use custom config
                .UseConfig(new LoggingConfiguration()
                // To ignore exceptions
                //.IgnoreException<DivideByZeroException>()
                // To hide sensitive information
                .HideSensitiveInfo(
                    new List<string>
                    {
                        "(?:password=)(?<paramValue>[^&|^\\s]+)",
                        "(\"password\":)(?<paramValue>[^,|^\r]+)",
                        "(?:seccode=)(?<paramValue>[^&|^\\s]+)",
                        "(\"seccode\":)(?<paramValue>[^,|^\r]+)",
                        "(?:securitycode=)(?<paramValue>[^&|^\\s]+)",
                        "(\"securitycode\":)(?<paramValue>[^,|^\r]+)"
                    })
                // To Enable Log Http Header
                .GetHttpHeaderInfo()
                // To Enable Log Http Session
                .GetHttpSessionInfo())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            ExceptionHandler.Handle(
                                exception,
                                new Dictionary<string, object> { ["user id"] = 1111111 });

            ExceptionHandler.Handle(
                                exception,
                                TraceEventType.Verbose);

            ExceptionHandler.Handle(
                                exception,
                                new Dictionary<string, object> { ["user id"] = 11111221 },
                                TraceEventType.Warning);
        }
    }
}