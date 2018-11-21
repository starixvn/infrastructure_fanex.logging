[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.AspNetCore.UnitTests")]

namespace Fanex.Logging.Extensions.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Http.Internal;

    public static class WebInfoGetters
    {
        public static IDictionary<string, object> GetRequestInfo(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return Constants.EmptyDictionary;
            }

            var request = httpContext.Request;

            return new Dictionary<string, object>
            {
                ["Url"] = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                ["UrlReferrer"] = request.Headers["Referer"].ToString(),
                ["Form"] = GetForm(request),
                ["IpAddress"] = $"Remote: {request.HttpContext.Connection.RemoteIpAddress},Local: {request.HttpContext.Connection.LocalIpAddress}",
                ["InputStream"] = CommonInfoGetters.ConvertStreamToString(request.Body),
                ["UserAgent"] = request.Headers["User-Agent"].ToString(),
                [nameof(Thread.CurrentThread.CurrentUICulture)] = Thread.CurrentThread.CurrentUICulture
            };
        }

        public static IDictionary<string, object> GetBrowserInfo(HttpRequest request)
        {
            if (request == null)
            {
                return Constants.EmptyDictionary;
            }

            var userAgent = new UserAgent.UserAgent(request.Headers["User-Agent"]);
            return new Dictionary<string, object>
            {
                ["Platform"] = $"{userAgent.OS.Name} {userAgent.OS.Version} ",
                ["Browser"] = $"{userAgent.Browser.Name} {userAgent.Browser.Version}"
            };
        }

        internal static IDictionary<string, object> GetSessionInfo(HttpContext httpContext)
        {
            var session = httpContext == null || httpContext.Features.Get<ISessionFeature>() == null
                ? null
                : httpContext.Session;

            return session == null
                    ? Constants.EmptyDictionary
                    : session
                    .Keys
                    .ToDictionary(key => key, key => (object)session.GetString(key));
        }

        internal static IDictionary<string, object> GetRequestHeader(IHeaderDictionary headers)
                => headers == null
                    ? Constants.EmptyDictionary
                    : headers
                    .Keys
                    .ToDictionary(key => key, key => headers[key] as object);

        internal static IDictionary<string, object> GetForm(HttpRequest request)
        {
            if (!request.HasFormContentType)
            {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            }

            try
            {
                var form = request.Form;

                return form == null
                        ? Constants.EmptyDictionary
                        : form
                        .Keys
                        .ToDictionary(key => key, key => form[key] as object);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>
                {
                    { "FormException", ex.Message }
                };
            }
        }
    }
}