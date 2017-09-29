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

    public static class WebLoggingExtensions
    {
        public static ILogEntry AddRequestInfo(this ILogEntry logEntry, HttpRequest request, NameValueCollection serverVariables = null)
        {
            if (logEntry == null || request == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(
                    LogUtility.CombineInfo(
                      "REQUEST INFO",
                      $"Url: {request.Url}",
                      $"Url Referrer: {request.UrlReferrer}",
                      $"Form Variables: {request.Form}",
                      $"Body: {GetRequestBodyData(request.InputStream)}",
                      $"Client IP: {GetClientIP(serverVariables ?? request.ServerVariables)}",
                      $"Current Thread Culture: {Thread.CurrentThread.CurrentUICulture}"));
        }

        public static string GetRequsetInfo(HttpRequest request, NameValueCollection serverVariables = null)
            => LogUtility.CombineInfo(
                      "REQUEST INFO",
                      $"Url: {request.Url}",
                      $"Url Referrer: {request.UrlReferrer}",
                      $"Form Variables: {request.Form}",
                      $"Body: {GetRequestBodyData(request.InputStream)}",
                      $"Client IP: {GetClientIP(serverVariables ?? request.ServerVariables)}",
                      $"Current Thread Culture: {Thread.CurrentThread.CurrentUICulture}");

        public static ILogEntry AddBrowserInfo(
            this ILogEntry logEntry,
            HttpCapabilitiesBase browser,
            NameValueCollection serverVariables)
        {
            if (logEntry == null || browser == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(
                   LogUtility.CombineInfo(
                     "BROWSER INFO",
                        $"Browser: {browser.Browser} {browser.Version} (beta: {browser.Beta})",
                        $"Platform: {browser.Platform}",
                        $"Crawler: {browser.Crawler}",
                        $"ECMAScript: {browser.EcmaScriptVersion}",
                        $"Mobile: {browser.IsMobileDevice} ({browser.MobileDeviceModel} {browser.MobileDeviceManufacturer})",
                        $"User Agent: {serverVariables["HTTP_USER_AGENT"]}"));
        }

        public static string GetBrowserInfo(HttpCapabilitiesBase browser, NameValueCollection serverVariables)
                => LogUtility.CombineInfo(
                     "BROWSER INFO",
                        $"Browser: {browser.Browser} {browser.Version} (beta: {browser.Beta})",
                        $"Platform: {browser.Platform}",
                        $"Crawler: {browser.Crawler}",
                        $"ECMAScript: {browser.EcmaScriptVersion}",
                        $"Mobile: {browser.IsMobileDevice} ({browser.MobileDeviceModel} {browser.MobileDeviceManufacturer})",
                        $"User Agent: {serverVariables["HTTP_USER_AGENT"]}");

        public static ILogEntry AddHeaderInfo(
            this ILogEntry logEntry,
            NameValueCollection headers)
        {
            if (logEntry == null || headers == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(
                    LogUtility.CombineInfo(
                      "HTTP HEADERS",
                            headers
                            .AllKeys
                            .Select(key => $"{key}: {headers[key]}")
                            .ToArray()));
        }        

        public static ILogEntry AddSessionInfo(this ILogEntry logEntry, HttpSessionState session)
        {
            if (logEntry == null || session == null)
            {
                return logEntry;
            }

            var sessionData = session
                        .Keys
                        .Cast<object>()
                        .Select(key => $"{key}: {session[key.ToString()]}");

            return logEntry.AddInfo(
                    LogUtility.CombineInfo(
                      "HTTP SESSION",
                      sessionData.ToArray()));
        }

        internal static string GetRequestBodyData(Stream requestStream)
        {
            if (requestStream != null)
            {
                requestStream.Seek(0, SeekOrigin.Begin);
                var bodyData = new StreamReader(requestStream).ReadToEnd();
                requestStream.Seek(0, SeekOrigin.Begin);
                if (!string.IsNullOrWhiteSpace(bodyData)
                    && bodyData.Contains("{"))
                {
                    return HttpUtility.UrlDecode(bodyData).Replace(@"\", string.Empty);
                }
            }

            return string.Empty;
        }

        internal static string GetClientIP(NameValueCollection serverVariables)
        {
            var clientIP = serverVariables["HTTP_X_FORWARDED_FOR"]
                ?? serverVariables["REMOTE_ADDR"]
                ?? string.Empty;

            return string.IsNullOrEmpty(clientIP) || clientIP.Equals("::1")
                ? "127.0.0.1"
                : clientIP;
        }
    }
}