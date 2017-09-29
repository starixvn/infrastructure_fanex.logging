namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;

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

            var currentRequest = HttpContext.Current.Request;
            var logData = new LogData(exception.Message)
                                .AddRequestInfo(currentRequest)
                                .AddBrowserInfo(currentRequest.Browser, currentRequest.ServerVariables)
                                .HideSensitiveInfo(
                                        config.SensitiveParams,
                                        config.RegexSensitiveParamPatterns)
                                .AddInfo(config.GetDbExceptionInfo(exception))
                                .AddInfo(customInfo)
                                .AddWebServerInfo()
                                .AddExceptionInfo(exception);

            logData
                .AddContent(() => exception.Message)
                .AddContent(() => WebLoggingExtensions.GetRequsetInfo(currentRequest));

                
                

            if (config.EnabledLogHttpHeader)
            {
                logData.AddHeaderInfo(currentRequest.Headers);
            }

            if (config.EnabledLogHttpSession)
            {
                logData.AddSessionInfo(HttpContext.Current.Session);
            }

            LogService.Write(logData, traceEventType, category);
        }
    }
}