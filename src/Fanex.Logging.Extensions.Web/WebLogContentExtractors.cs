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
    using Fanex.Logging;

    public static class WebLogContentExtractors
    {
        public static string GetRequsetInfo(HttpRequest request, NameValueCollection serverVariables = null)
            => LogUtility.CombineInfo(
                      "REQUEST INFO",
                      $"Url: {request.Url}",
                      $"Url Referrer: {request.UrlReferrer}",
                      $"Form Variables: {request.Form}",
                      $"Body: {request.InputStream.GetRequestBodyData()}",
                      $"Client IP: {(serverVariables ?? request.ServerVariables).GetClientIP()}",
                      $"Current Thread Culture: {Thread.CurrentThread.CurrentUICulture}");

        public static string GetBrowserInfo(HttpCapabilitiesBase browser, NameValueCollection serverVariables)
              => LogUtility.CombineInfo(
                   "BROWSER INFO",
                      $"{nameof(browser.Browser)}: {browser.Browser} {browser.Version} ({nameof(browser.Beta)}: {browser.Beta})",
                      $"{nameof(browser.Platform)}: {browser.Platform}",
                      $"{nameof(browser.Crawler)}: {browser.Crawler}",
                      $"{nameof(browser.EcmaScriptVersion)}: {browser.EcmaScriptVersion}",
                      $"Mobile: {browser.IsMobileDevice} ({browser.MobileDeviceModel} {browser.MobileDeviceManufacturer})",
                      $"User Agent: {serverVariables["HTTP_USER_AGENT"]}");

        public static string GetSessionInfo(HttpSessionState session)
             => LogUtility.CombineInfo("HTTP SESSION", session
                        .Keys
                        .Cast<object>()
                        .Select(key => $"{key}: {session[key.ToString()]}").ToArray());

        public static string GetRequestHeader(NameValueCollection headers)
            => LogUtility.CombineInfo(
                   "REQUEST HEADERS",
                         headers
                         .AllKeys
                         .Select(key => $"{key}: {headers[key]}")
                         .ToArray());
    }
}
