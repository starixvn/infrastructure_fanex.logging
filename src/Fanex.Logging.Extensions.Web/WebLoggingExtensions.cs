namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.SessionState;
    using static WebLogContentExtractors;

    public static class WebLoggingExtensions
    {
        public static ILogEntry AddRequestInfo(this ILogEntry logEntry, HttpRequest request, NameValueCollection serverVariables = null)
        {
            if (logEntry == null || request == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(GetRequsetInfo(request, serverVariables));
        }

        public static ILogEntry AddBrowserInfo(
            this ILogEntry logEntry,
            HttpCapabilitiesBase browser,
            NameValueCollection serverVariables)
        {
            if (logEntry == null || browser == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(GetBrowserInfo(browser, serverVariables));
        }   

        public static ILogEntry AddHeaderInfo(
            this ILogEntry logEntry,
            NameValueCollection headers)
        {
            if (logEntry == null || headers == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(GetRequestHeader(headers));
        }     

        public static ILogEntry AddSessionInfo(this ILogEntry logEntry, HttpSessionState session)
        {
            if (logEntry == null || session == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(GetSessionInfo(session));
        }          
    }
}