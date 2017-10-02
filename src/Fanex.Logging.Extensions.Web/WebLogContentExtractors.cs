namespace Fanex.Logging.Extensions.Web
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.SessionState;

    public static class WebLogContentExtractors
    {
        public static List<string> GetRequestInfo(HttpRequest request)
            => GetRequestInfo(request, null);

        internal static List<string> GetRequestInfo(
            HttpRequest request,
            NameValueCollection serverVariables)
                => request == null
                    ? Constants.NoInformationContent
                    : new List<string>
                    {
                        $"Url: {request.Url}",
                        $"Url Referrer: {request.UrlReferrer}",
                        $"Form Variables: {request.Form}",
                        $"Body: {request.InputStream.GetRequestBodyData()}",
                        $"Client IP: {(serverVariables ?? request.ServerVariables).GetClientIP()}",
                        $"Current Thread Culture: {Thread.CurrentThread.CurrentUICulture}"
                    };

        public static List<string> GetBrowserInfo(HttpCapabilitiesBase browser, NameValueCollection serverVariables)
                => browser == null || serverVariables == null
                    ? Constants.NoInformationContent
                    : new List<string>
                    {
                        $"Browser: {browser.Browser} {browser.Version} (Beta: {browser.Beta})",
                        $"Platform: {browser.Platform}",
                        $"Crawler: {browser.Crawler}",
                        $"ECMAScript: {browser.EcmaScriptVersion}",
                        $"Mobile: {browser.IsMobileDevice} ({browser.MobileDeviceModel} {browser.MobileDeviceManufacturer})",
                        $"User Agent: {serverVariables["HTTP_USER_AGENT"]}"
                    };

        public static List<string> GetSessionInfo(HttpSessionState session)
                => session == null
                    ? Constants.NoInformationContent
                    : session
                        .Keys
                        .Cast<object>()
                        .Select(key => $"{key}: {session[key.ToString()]}").ToList();

        public static List<string> GetRequestHeader(NameValueCollection headers)
                => headers == null
                    ? Constants.NoInformationContent
                    : headers
                         .AllKeys
                         .Select(key => $"{key}: {headers[key]}")
                         .ToList();
    }
}