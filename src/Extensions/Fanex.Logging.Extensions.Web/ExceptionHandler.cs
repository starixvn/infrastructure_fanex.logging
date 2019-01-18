namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.SessionState;

    public static class ExceptionHandler
    {
        internal static LoggingConfiguration config = new LoggingConfiguration();

        public static LoggingConfiguration UseConfig(LoggingConfiguration loggingConfiguration)
        {
            config = loggingConfiguration ?? throw new ArgumentNullException(nameof(loggingConfiguration));

            return config;
        }

        public static LoggingConfiguration UseGetDbExceptionInfo(Func<Exception, IDictionary<string, object>> getDbExceptionInfo)
        {
            config.GetDbExceptionInfo = getDbExceptionInfo ?? throw new ArgumentNullException(nameof(getDbExceptionInfo));

            return config;
        }

        public static void Handle(Exception exception)
            => Handle(exception, null);

        public static void Handle(Exception exception, TraceEventType traceEventType)
            => Handle(exception, null, traceEventType);

        public static void Handle(Exception exception, IDictionary<string, object> customData)
            => Handle(exception, customData, TraceEventType.Error);

        public static void Handle(Exception exception, IDictionary<string, object> customData, TraceEventType traceEventType)
#pragma warning disable S4462 // Calls to "async" methods should not be blocking
            => HandleAsync(exception, customData, HttpContext.Current, traceEventType).GetAwaiter().GetResult();
#pragma warning restore S4462 // Calls to "async" methods should not be blocking

        internal static void Handle(
            Exception exception,
            IDictionary<string, object> customData,
            HttpContext context,
            TraceEventType traceEventType)
#pragma warning disable S4462 // Calls to "async" methods should not be blocking
            => HandleAsync(exception, customData, HttpContext.Current, traceEventType).GetAwaiter().GetResult();
#pragma warning restore S4462 // Calls to "async" methods should not be blocking

        public static Task HandleAsync(Exception exception)
            => HandleAsync(exception, null);

        public static Task HandleAsync(Exception exception, TraceEventType traceEventType)
            => HandleAsync(exception, null, traceEventType);

        public static Task HandleAsync(Exception exception, IDictionary<string, object> customData)
            => HandleAsync(exception, customData, TraceEventType.Error);

        public static Task HandleAsync(Exception exception, IDictionary<string, object> customData, TraceEventType traceEventType)
           => HandleAsync(exception, customData, HttpContext.Current, traceEventType);

        internal static Task HandleAsync(
            Exception exception,
            IDictionary<string, object> customData,
            HttpContext context,
            TraceEventType traceEventType)
        {
            if (config.IgnoredExceptionTypes.Contains(exception.GetType())
                || exception.GetBaseException() is HttpException)
            {
                return Task.FromResult(0);
            }

            try
            {
                return WriteLogAsync(exception, customData, context, traceEventType);
            }
            catch (Exception loggingException)
            {
                return WriteExceptionAsync($"Consumer Exception:{exception},\r\n Fanex.Logging.Extensions.Web Exception:{loggingException}", exception);
            }
        }

        private static Task WriteLogAsync(
            Exception exception,
            IDictionary<string, object> customData,
            HttpContext context,
            TraceEventType traceEventType)
        {
            if (context != null)
            {
                var message = ComposeMessage(exception, customData);
                var writeLogAsync = GetWriteLogMethod(traceEventType);

                return writeLogAsync(message);
            }
            else
            {
                return WriteExceptionAsync(exception.Message, exception);
            }
        }

        private static Func<string, Task> GetWriteLogMethod(TraceEventType traceEventType)
        {
            switch (traceEventType)
            {
                case TraceEventType.Verbose:
                    return WriteDebugAsync;

                case TraceEventType.Warning:
                    return WriteWarnAsync;

                case TraceEventType.Critical:
                    return WriteCriticalAsync;

                case TraceEventType.Error:
                    return WriteErrorAsync;

                default:
                    return WriteInfoAsync;
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
        internal static Func<string, Task> WriteErrorAsync { get; set; } = LogService.ErrorAsync;
        internal static Func<string, Task> WriteCriticalAsync { get; set; } = LogService.CriticalAsync;
        internal static Func<string, Task> WriteWarnAsync { get; set; } = LogService.WarnAsync;
        internal static Func<string, Task> WriteInfoAsync { get; set; } = LogService.InfoAsync;
        internal static Func<string, Task> WriteDebugAsync { get; set; } = LogService.DebugAsync;
        internal static Func<string, Exception, Task> WriteExceptionAsync { get; set; } = LogService.ErrorAsync;
    }
}