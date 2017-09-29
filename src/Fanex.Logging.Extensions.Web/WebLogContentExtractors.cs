namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.SessionState;

    public static class WebLogContentExtractors
    {
        public static List<string> GetRequsetInfo(HttpRequest request, NameValueCollection serverVariables = null)
                => new List<string> {
                      $"Url: {request.Url}",
                      $"Url Referrer: {request.UrlReferrer}",
                      $"Form Variables: {request.Form}",
                      $"Body: {request.InputStream.GetRequestBodyData()}",
                      $"Client IP: {(serverVariables ?? request.ServerVariables).GetClientIP()}",
                      $"Current Thread Culture: {Thread.CurrentThread.CurrentUICulture}"};

        public static List<string> GetBrowserInfo(HttpCapabilitiesBase browser, NameValueCollection serverVariables)
                => new List<string> {
                      $"{nameof(browser.Browser)}: {browser.Browser} {browser.Version} ({nameof(browser.Beta)}: {browser.Beta})",
                      $"{nameof(browser.Platform)}: {browser.Platform}",
                      $"{nameof(browser.Crawler)}: {browser.Crawler}",
                      $"{nameof(browser.EcmaScriptVersion)}: {browser.EcmaScriptVersion}",
                      $"Mobile: {browser.IsMobileDevice} ({browser.MobileDeviceModel} {browser.MobileDeviceManufacturer})",
                      $"User Agent: {serverVariables["HTTP_USER_AGENT"]}" };

        public static List<string> GetSessionInfo(HttpSessionState session)
                => session
                        .Keys
                        .Cast<object>()
                        .Select(key => $"{key}: {session[key.ToString()]}").ToList();

        public static List<string> GetRequestHeader(NameValueCollection headers)
                => headers
                         .AllKeys
                         .Select(key => $"{key}: {headers[key]}")
                         .ToList();

    }
}
