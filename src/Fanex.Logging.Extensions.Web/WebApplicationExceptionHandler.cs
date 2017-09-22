namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;

    public static class WebApplicationExceptionHandler
    {
        static WebLoggingConfiguration config;

        public static WebLoggingConfiguration Configure(WebLoggingConfiguration loggingConfiguration)
        {
            if (loggingConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggingConfiguration));
            }

            config = loggingConfiguration;

            return config;
        }

        public static void Handle(Exception exception)
            => Handle(exception, string.Empty);
        public static void Handle(Exception exception, string customInfo)
            => Handle(exception, TraceEventType.Error, customInfo);

        public static void Handle(
            Exception exception,
            TraceEventType traceEventType,
            string customInfo = null)
        {
            if (HttpContext.Current == null
                || config.IgnoredExceptionTypes.Contains(exception.GetType()))
            {
                return;
            }

            var currentRequest = HttpContext.Current.Request;
            var logEntry = new LogEntry(exception.Message)
                                .AddRequestInfo(currentRequest)
                                .AddBrowserInfo(currentRequest)
                                .CensorSensitiveInfo(
                                        config.SensitiveParams,
                                        config.RegexSensitiveParamPatterns)
                                .AddInfo(config.GetDatabaseInfoFunc(exception))
                                .AddInfo(customInfo)
                                .AddWebServerInfo()
                                .AddExceptionInfo(exception);

            if (config.EnabledLogHttpHeader)
            {
                logEntry.AddHeaderInfo(currentRequest);
            }

            if (config.EnabledLogHttpSession)
            {
                logEntry.AddSessionInfo(HttpContext.Current.Session);
            }

            LogService.Write(logEntry, traceEventType);

#if DEBUG
            //// this code for dev to view message on browser, dont bring it to production
            HttpContext.Current.Response.Write(logEntry.ToString().Replace("\n", "<br/>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Close();
#endif
        }
    }
}