namespace Fanex.Logging.Extensions.Web
{
    using System.IO;
    using System.Threading;
    using System.Web;
    using System.Web.Configuration;

    public static class WebLoggingUtility
    {
        public static ILogEntry AddRequestInfo(this ILogEntry logEntry, HttpRequest request)
        {
            if (logEntry == null || request == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(
                    LogUtility.CombineInfo(
                      "REQUEST & BROWSER INFO",
                      $"Url: {request.Url}",
                      $"Url Referrer: {request.UrlReferrer}",
                      $"Form Variables: {request.Form}",
                      $"Body Data: {GetRequestBodyData(request.InputStream)}",
                      $"Client IP: {GetClientIP(request)}",
                      $"Current Thread Culture: {Thread.CurrentThread.CurrentUICulture}",
                      BuildClientBrowserInfo(request.Browser),
                      $"User Agent: {request.ServerVariables["HTTP_USER_AGENT"]}"));
        }

        private static string BuildClientBrowserInfo(HttpCapabilitiesBase browser)
            => browser == null
                    ? string.Empty
                    : string.Join(
                        "\r\n\t",
                        $"Browser: {browser.Browser} {browser.Version}.",
                        $"Client Platform: {browser.Platform}",
                        $"Is Beta Version: {browser.Beta}",
                        $"Is Search Engine WEB Crawler: {browser.Crawler}",
                        $"Ecma Script Version: {browser.EcmaScriptVersion}",
                        $"Mobile Device: {browser.MobileDeviceModel}");

        private static string GetRequestBodyData(Stream requestStream)
        {
            if (requestStream == null)
            {
                return string.Empty;
            }

            requestStream.Seek(0, SeekOrigin.Begin);
            var bodyData = new StreamReader(requestStream).ReadToEnd();
            requestStream.Seek(0, SeekOrigin.Begin);
            if (!string.IsNullOrWhiteSpace(bodyData)
                && bodyData.Contains("{"))
            {
                return HttpUtility.UrlDecode(bodyData).Replace(@"\", string.Empty);
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