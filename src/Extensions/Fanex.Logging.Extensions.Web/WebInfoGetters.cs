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

    public static class WebInfoGetters
    {
        public static IDictionary<string, object> GetRequestInfo(HttpRequest request)
            => GetRequestInfo(request, null);

        internal static IDictionary<string, object> GetRequestInfo(
            HttpRequest request,
            NameValueCollection serverVariables)
            => request == null
                ? Constants.EmptyDictionary
                : new Dictionary<string, object>
                {
                    [nameof(request.Url)] = request.Url,
                    [nameof(request.UrlReferrer)] = request.UrlReferrer,
                    [nameof(request.Form)] = request.Form,
                    [nameof(request.UserHostAddress)] = request.UserHostAddress,
                    ["InputStream"] = ConvertStreamToString(GetInputStream(request)),
                    [nameof(request.UserAgent)] = GetUserAgent(serverVariables ?? request.ServerVariables),
                    ["Client IP"] = GetClientIP(serverVariables ?? request.ServerVariables),
                    [nameof(Thread.CurrentThread.CurrentUICulture)] = Thread.CurrentThread.CurrentUICulture
                };

        public static IDictionary<string, object> GetBrowserInfo(HttpCapabilitiesBase browser)
          => browser == null
                ? Constants.EmptyDictionary
                : new Dictionary<string, object>
                {
                    [nameof(browser.Browser)] = $"{browser.Browser} {browser.Version} (Beta: {browser.Beta})",
                    [nameof(browser.Platform)] = browser.Platform,
                    [nameof(browser.Crawler)] = browser.Crawler,
                    [nameof(browser.EcmaScriptVersion)] = browser.EcmaScriptVersion,
                    [nameof(browser.MobileDeviceModel)] = browser.MobileDeviceModel
                };

        /// <summary>
        /// This is workaround for this exception:
        /// Either BinaryRead, Form, Files, or InputStream was accessed before the internal storage was filled by the caller of HttpRequest.GetBufferedInputStream
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal static Stream GetInputStream(HttpRequest request)
        {
            try
            {
                return request.InputStream;
            }
            catch(Exception ex)
            {
                if(!(ex is InvalidOperationException))
                {
                    LogService.Info(ex.ToString());
                }
            }

            return null;
        }

        internal static string ConvertStreamToString(Stream requestStream)
        {
            if (requestStream != null)
            {
                string bodyData = GetStreamData(requestStream);

                if (requestStream.CanSeek)
                {
                    requestStream.Seek(0, SeekOrigin.Begin);
                }

                if (!string.IsNullOrWhiteSpace(bodyData))
                {
                    return HttpUtility.UrlDecode(bodyData).Replace(@"\", string.Empty);
                }
            }

            return Constants.NotAvailableAbbr;
        }

        private static string GetStreamData(Stream requestStream)
        {
            string bodyData = string.Empty;
            var tempStream = new MemoryStream();

            requestStream.CopyTo(tempStream);

            tempStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(tempStream))
            {
                bodyData = streamReader.ReadToEnd();
            }

            return bodyData;
        }

        public static IDictionary<string, object> GetSessionInfo(HttpSessionState session)
                => session == null
                    ? Constants.EmptyDictionary
                    : session
                    .Keys
                    .Cast<object>()
                    .ToDictionary(key => key.ToString(), key => session[key.ToString()]);

        public static IDictionary<string, object> GetRequestHeader(NameValueCollection headers)
                => headers == null
                    ? Constants.EmptyDictionary
                    : headers
                    .AllKeys
                    .ToDictionary(key => key, key => headers[key] as object);

        internal static string GetClientIP(NameValueCollection serverVariables)
        {
            var clientIP = serverVariables["HTTP_X_FORWARDED_FOR"]
                ?? serverVariables["REMOTE_ADDR"]
                ?? string.Empty;

            return string.IsNullOrEmpty(clientIP) || clientIP.Equals("::1")
                ? "127.0.0.1"
                : clientIP;
        }

        private static string GetUserAgent(NameValueCollection serverVariables)
            => serverVariables["HTTP_USER_AGENT"];
    }
}