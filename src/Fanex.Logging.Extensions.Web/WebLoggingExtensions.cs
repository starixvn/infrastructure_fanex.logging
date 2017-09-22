namespace Fanex.Logging.Extensions.Web
{
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.SessionState;

    public static class WebLoggingExtensions
    {
        public static ILogEntry AddRequestInfo(this ILogEntry logEntry, HttpRequest request)
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
                      $"Client IP: {GetClientIP(request)}",
                      $"Current Thread Culture: {Thread.CurrentThread.CurrentUICulture}"));                      
        }

        public static ILogEntry AddBrowserInfo(this ILogEntry logEntry, HttpRequest request)
        {
            if (logEntry == null || request == null)
            {
                return logEntry;
            }

            var browser = request.Browser;

            return logEntry.AddInfo(
                   LogUtility.CombineInfo(
                     "BROWSER INFO",
                        $"Browser: {browser.Browser} {browser.Version} (beta: {browser.Beta})",
                        $"Platform: {browser.Platform}",                       
                        $"Crawler: {browser.Crawler}",
                        $"ECMAScript: {browser.EcmaScriptVersion}",
                        $"Mobile: {browser.IsMobileDevice} ({browser.MobileDeviceModel} {browser.MobileDeviceManufacturer})",                        
                        $"User Agent: {request.ServerVariables["HTTP_USER_AGENT"]}"));
        }

        public static ILogEntry AddHeaderInfo(this ILogEntry logEntry, HttpRequest request)
        {
            if (logEntry == null || request == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(
                    LogUtility.CombineInfo(
                      "HTTP HEADERS",
                      request.Headers
                            .AllKeys
                            .Select(key => $"{key}: {request.Headers[key]}")
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

        static string GetRequestBodyData(Stream requestStream)
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

        private static string GetClientIP(HttpRequest request)
        {
            var ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.ServerVariables["REMOTE_ADDR"];
#pragma warning disable S1313 // IP addresses should not be hardcoded
            return string.IsNullOrEmpty(ip) || ip.Equals("::1")
                ? "127.0.0.1"
#pragma warning restore S1313 // IP addresses should not be hardcoded
                : ip.Split(',')[0];
        }
    }
}