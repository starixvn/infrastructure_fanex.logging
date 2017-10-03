namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using static CommonInfoGetters;
    using static WebInfoGetters;

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

        public static LoggingConfiguration UseGetDbExceptionInfo(Func<Exception, IDictionary<string, object>> getDbExceptionInfo)
        {
            if (getDbExceptionInfo == null)
            {
                throw new ArgumentNullException(nameof(getDbExceptionInfo));
            }

            config.GetDbExceptionInfo = getDbExceptionInfo;

            return config;
        }

        [DebuggerStepThrough]
        public static void Handle(Exception exception)
            => Handle(exception, null);

        public static void Handle(
            Exception exception,
            IDictionary<string, object> customData)
        {
            if (config.IgnoredExceptionTypes.Contains(exception.GetType()))
            {
                return;
            }

            var logMessage = new LogMessage();

            if (HttpContext.Current != null)
            {
                var request = HttpContext.Current.Request;
                logMessage
                    .AddMessage(exception.Message)
                    .AddSection("request info", () => GetRequestInfo(request))
                    .AddSection("browser info", () => GetBrowserInfo(request.Browser))
                    .AddSection("server info", () => GetServerInfo())
                    .AddSection("session info", () => GetSessionInfo(HttpContext.Current.Session))
                    .AddSection("request headers", () => GetRequestHeader(request.Headers))
                    .AddSection("database info", () => config.GetDbExceptionInfo(exception))
                    .AddSection("custom info", () => customData)
                    .AddSection("exception info", () => GetExceptionInfo(exception));
            }

            var message = logMessage.ToCleanMessage(config.Patterns);

            LogService.Error(message);
        }
    }
}