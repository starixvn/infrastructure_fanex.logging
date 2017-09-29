namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using static WebLogContentExtractors;
    using static Fanex.Logging.DefaultLogContentExtractors;

    public static class ExceptionHandler
    {
        internal static LoggingConfiguration config = new LoggingConfiguration();

        public static LoggingConfiguration UseConfig(LoggingConfiguration loggingConfiguration)
        {
            if (loggingConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggingConfiguration));
            }

            config = loggingConfiguration;

            return config;
        }

        public static LoggingConfiguration UseGetDbExceptionInfo(Func<Exception, string> getDbExceptionInfo)
        {
            if (getDbExceptionInfo == null)
            {
                throw new ArgumentNullException(nameof(getDbExceptionInfo));
            }

            config.GetDbExceptionInfo = getDbExceptionInfo;

            return config;
        }

        public static void Handle(Exception exception)
            => Handle(exception, string.Empty);

        public static void Handle(Exception exception, string customInfo)
            => Handle(exception, TraceEventType.Error, customInfo);

        public static void Handle(Exception exception, TraceEventType traceEventType, string category)
            => Handle(exception, traceEventType, string.Empty, category);

        public static void Handle(
            Exception exception,
            TraceEventType traceEventType,
            string customInfo,
            string category)
        {
            if (HttpContext.Current == null
                || config.IgnoredExceptionTypes.Contains(exception.GetType()))
            {
                return;
            }

            var request = HttpContext.Current.Request;

            var logEntry = new LogEntry()
                .AddContent(() => exception.Message)
                .Section("REQUEST INFO").AddContent(GetRequsetInfo(request))
                .Section("BROWSER INFO").AddContent(GetBrowserInfo(request.Browser, request.ServerVariables))                
                .HideSensitiveInfo(config.SensitiveParams, config.RegexSensitiveParamPatterns)
                .Section("DATABASE").AddContent(() => config.GetDbExceptionInfo(exception))
                .Section("WEB SERVER").AddContent(GetServerInfo())      
                .AddContent(() => GetExceptionInfo(exception));

            if (config.EnabledLogHttpHeader)
            {
                logEntry
                    .Section("REQUEST HEADERS").AddContent(GetRequestHeader(request.Headers));             
            }

            if (config.EnabledLogHttpSession)
            {
                logEntry
                     .Section("REQUEST SESSION").AddContent(GetSessionInfo(HttpContext.Current.Session));
            }

            LogService.Write(logEntry, traceEventType, category);

#if DEBUG
            //// this code for dev to view message on browser, dont bring it to production
            HttpContext.Current.Response.Write(logEntry.ToString().Replace("\n", "<br/>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Close();
#endif
        }
    }
}