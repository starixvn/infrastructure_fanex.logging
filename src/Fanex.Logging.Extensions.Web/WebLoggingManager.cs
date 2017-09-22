namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;

    public static class WebLoggingManager
    {
        private static WebLoggingSetting webLoggingSetting;

        public static WebLoggingSetting StartNewSession()
        {
            if (webLoggingSetting == null)
            {
                webLoggingSetting = new WebLoggingSetting();
            }

            return webLoggingSetting;
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
                                .AddClientBrowserInfo(currentRequest)
                                .CensorSensitiveInfo(
                                        webLoggingSetting.SensitiveParams,
                                        webLoggingSetting.RegexSensitiveParamPatterns)
                                .AddInfo(webLoggingSetting.GetDatabaseInfoFunc(exception))
                                .AddInfo(customInfo)
                                .AddWebServerInfo()
                                .AddExceptionInfo(exception);

            if (webLoggingSetting.EnabledLogHttpHeader)
            {
                logEntry.AddHeaderInfo(currentRequest);
            }

            if (webLoggingSetting.EnabledLogHttpSession)
            {
                logEntry.AddSessionInfo(HttpContext.Current.Session);
            }

            LogService.Write(logEntry, traceEventType);
        }
    }
}