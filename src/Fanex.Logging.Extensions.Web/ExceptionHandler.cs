namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;

    public static class ExceptionHandler
    {
        static LoggingConfiguration config = new LoggingConfiguration();

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
                                .HideSensitiveInfo(
                                        config.SensitiveParams,
                                        config.RegexSensitiveParamPatterns)
                                .AddInfo(config.GetDbExceptionInfo(exception))
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
        }
    }
}