namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.SessionState;

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
            => Handle(exception, null, HttpContext.Current);

        public static void Handle(Exception exception, IDictionary<string, object> customData)
            => Handle(exception, null, HttpContext.Current);

        internal static void Handle(
            Exception exception,
            IDictionary<string, object> customData,
            HttpContext context)
        {
            if (config.IgnoredExceptionTypes.Contains(exception.GetType()))
            {
                return;
            }

            if (context != null)
            {
                var logMessage = new LogMessage();
                var request = HttpContext.Current.Request;
                logMessage
                    .AddMessage(exception.Message)
                    .AddSection("request info", () => GetRequestInfo(request))
                    .AddSection("browser info", () => GetBrowserInfo(request.Browser))
                    .AddSection("server info", () => GetServerInfo())
                    .AddSection("database info", () => config.GetDbExceptionInfo(exception))
                    .AddSection("custom info", () => customData)
                    .AddSection("exception info", () => GetExceptionInfo(exception));

                if (config.EnabledLogHttpHeader)
                {
                    logMessage.AddSection("request headers", () => GetRequestHeader(request.Headers));
                }

                if (config.EnabledLogHttpSession)
                {
                    logMessage.AddSection("session info", () => GetSessionInfo(HttpContext.Current.Session));
                }

                WriteError(logMessage.ToCleanMessage(config.Patterns));
            }
            else
            {
                WriteException(exception.Message, exception);
            }
        }

        internal static Func<HttpRequest, IDictionary<string, object>> GetRequestInfo { get; set; } = WebInfoGetters.GetRequestInfo;
        internal static Func<HttpBrowserCapabilities, IDictionary<string, object>> GetBrowserInfo { get; set; } = WebInfoGetters.GetBrowserInfo;
        internal static Func<IDictionary<string, object>> GetServerInfo { get; set; } = CommonInfoGetters.GetServerInfo;
        internal static Func<Exception, IDictionary<string, object>> GetExceptionInfo { get; set; } = CommonInfoGetters.GetExceptionInfo;
        internal static Func<NameValueCollection, IDictionary<string, object>> GetRequestHeader { get; set; } = WebInfoGetters.GetRequestHeader;
        internal static Func<HttpSessionState, IDictionary<string, object>> GetSessionInfo { get; set; } = WebInfoGetters.GetSessionInfo;
        internal static Action<string> WriteError { get; set; } = LogService.Error;
        internal static Action<string, Exception> WriteException { get; set; } = LogService.Error;
    }
}