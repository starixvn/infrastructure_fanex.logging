namespace Fanex.Logging.Extensions.Web
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Configuration;

    public static class WebLoggingUtility
    {
        public const string RegexSensitiveQueryStringParam = "(?:{0}=)(?<paramValue>[^&]+)";
        public const string RegexSensitiveBodyJsonParam = "(\"{0}\":)(?<paramValue>[^,]+)";

        public static string AddRequestInfo(this string message, HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(message)
                || request == null)
            {
                return message;
            }

            return message.AddLogSection(
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

        public static string CensorSensitiveParams(
            this string message,
            IEnumerable<string> sensitiveParams,
            params string[] regexSensitiveQueryStringParams)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return message;
            }

            if (sensitiveParams == null 
                || !sensitiveParams.Any()
                || regexSensitiveQueryStringParams == null
                || !regexSensitiveQueryStringParams.Any())
            {
                return message;
            }

            foreach (var regexSensitiveQueryStringParam in regexSensitiveQueryStringParams)
            {
                message = LogUtility.CensorSensitiveInformation(message, regexSensitiveQueryStringParam, sensitiveParams);
            }

            return message;
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