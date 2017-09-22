namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;

    public static class WebLoggingManager
    {
        private static WebLoggingSession webLoggingSession;

        public static WebLoggingSession StartNewSession()
        {
            if (webLoggingSession == null)
            {
                webLoggingSession = new WebLoggingSession();
            }

            return webLoggingSession;
        }

        public static void Log(
            Exception exception,
            string customInfo = null)
        {
            Log(exception, TraceEventType.Error, customInfo);
        }

        public static void Log(
            Exception exception,
            TraceEventType traceEventType = TraceEventType.Error,
            string customInfo = null)
        {
            if (HttpContext.Current == null
                || exception is HttpException)
            {
                return;
            }

            var currentRequest = HttpContext.Current.Request;
            var logEntry = new LogEntry(exception.Message)
                                .AddRequestInfo(currentRequest)
                                .CensorSensitiveInfo(
                                        webLoggingSession.SensitiveParams,
                                        webLoggingSession.RegexSensitiveParamPatterns)
                                .AddInfo(webLoggingSession.GetDatabaseInfoFunc(exception))
                                .AddInfo(customInfo)
                                .AddWebServerInfo()
                                .AddExceptionInfo(exception);

            if (webLoggingSession.EnabledLogHttpHeader)
            {
                logEntry.AddHeaderInfo(currentRequest);
            }

            if (webLoggingSession.EnabledLogHttpSession)
            {
                logEntry.AddSessionInfo(HttpContext.Current.Session);
            }

            LogService.Write(logEntry, traceEventType);
        }
    }
}