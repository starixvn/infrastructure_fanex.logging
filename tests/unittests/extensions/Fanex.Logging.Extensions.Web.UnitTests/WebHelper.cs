namespace Fanex.Logging.Extensions.Web.UnitTests
{
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.SessionState;

    public static class WebHelper
    {
        internal static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        internal static HttpContext CreateFakeHttpContext(string queryString, SessionStateItemCollection sessionStateItemCollection)
        {
            SimpleWorkerRequest request = new SimpleWorkerRequest(string.Empty, string.Empty, string.Empty, queryString, new StringWriter());

            HttpContext.Current = new HttpContext(request);

            var sessionContainer = new HttpSessionStateContainer(
                                                "id",
                                               sessionStateItemCollection,
                                               new HttpStaticObjectsCollection(),
                                               10,
                                               true,
                                               HttpCookieMode.AutoDetect,
                                               SessionStateMode.InProc,
                                               false);

            HttpContext.Current.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                    BindingFlags.NonPublic | BindingFlags.Instance,
                                    null,
                                    CallingConventions.Standard,
                                    new[] { typeof(HttpSessionStateContainer) },
                                    null)
                .Invoke(new object[] { sessionContainer });

            return HttpContext.Current;
        }
    }
}