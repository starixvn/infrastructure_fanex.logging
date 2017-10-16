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

        public static void Handle(Exception exception)
            => Handle(exception, null, HttpContext.Current, TraceEventType.Error);

        public static void Handle(Exception exception, TraceEventType traceEventType)
            => Handle(exception, null, HttpContext.Current, traceEventType);

        public static void Handle(Exception exception, IDictionary<string, object> customData)
            => Handle(exception, customData, HttpContext.Current, TraceEventType.Error);

        public static void Handle(Exception exception, IDictionary<string, object> customData, TraceEventType traceEventType)
           => Handle(exception, customData, HttpContext.Current, traceEventType);

        internal static void Handle(
            Exception exception,
            IDictionary<string, object> customData,
            HttpContext context, 
            TraceEventType traceEventType)
        {
            if (config.IgnoredExceptionTypes.Contains(exception.GetType())
                || exception.GetBaseException() is HttpException)
            {
                return;
            }

            try
            {
                WriteLog(exception, customData, context, traceEventType);
            }
            catch (Exception loggingException)
            {
                WriteException(loggingException.Message, loggingException);
            }
        }

        private static void WriteLog(
            Exception exception, 
            IDictionary<string, object> customData, 
            HttpContext context,
            TraceEventType traceEventType)
        {
            if (context != null)
            {
                var message = ComposeMessage(exception, customData);
                var writeLog = GetWriteLogMethod(traceEventType);

                writeLog(message);
            }
            else
            {
                WriteException(exception.Message, exception);
            }
        }

        private static Action<string> GetWriteLogMethod(TraceEventType traceEventType)
        {
            switch (traceEventType)
            {
                case TraceEventType.Verbose:
                    return WriteDebug;

                case TraceEventType.Warning:
                    return WriteWarn;

                case TraceEventType.Critical:
                    return WriteCritical;

                case TraceEventType.Error:
                    return WriteError;

                default:
                    return WriteInfo;
            }
        }

        private static string ComposeMessage(
            Exception exception, 
            IDictionary<string, object> customData)
        {
            var request = HttpContext.Current.Request;
            var logMessage = new LogMessage()
                                .AddMessage(exception.Message)
                                .AddSection("request info", () => GetRequestInfo(request))
                                .AddSection("browser info", () => GetBrowserInfo(request.Browser))
                                .AddSection("server info", () => GetServerInfo())
                                .AddSection("database info", () => config.GetDbExceptionInfo != null 
                                                                    ? config.GetDbExceptionInfo(exception)
                                                                    : Constants.EmptyDictionary)
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

            return logMessage.ToCleanMessage(config.Patterns);
        }

        internal static Func<HttpRequest, IDictionary<string, object>> GetRequestInfo { get; set; } = WebInfoGetters.GetRequestInfo;
        internal static Func<HttpBrowserCapabilities, IDictionary<string, object>> GetBrowserInfo { get; set; } = WebInfoGetters.GetBrowserInfo;
        internal static Func<IDictionary<string, object>> GetServerInfo { get; set; } = CommonInfoGetters.GetServerInfo;
        internal static Func<Exception, IDictionary<string, object>> GetExceptionInfo { get; set; } = CommonInfoGetters.GetExceptionInfo;
        internal static Func<NameValueCollection, IDictionary<string, object>> GetRequestHeader { get; set; } = WebInfoGetters.GetRequestHeader;
        internal static Func<HttpSessionState, IDictionary<string, object>> GetSessionInfo { get; set; } = WebInfoGetters.GetSessionInfo;
        internal static Action<string> WriteError { get; set; } = LogService.Error;
        internal static Action<string> WriteCritical { get; set; } = LogService.Critical;
        internal static Action<string> WriteWarn { get; set; } = LogService.Warn;
        internal static Action<string> WriteInfo { get; set; } = LogService.Info;
        internal static Action<string> WriteDebug { get; set; } = LogService.Debug;
        internal static Action<string, Exception> WriteException { get; set; } = LogService.Error;
    }
}