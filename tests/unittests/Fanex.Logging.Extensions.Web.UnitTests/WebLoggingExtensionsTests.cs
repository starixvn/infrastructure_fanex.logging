using Xunit;
namespace Fanex.Logging.Extensions.Web.UnitTests
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Hosting;
    using System.Web.SessionState;
    using NSubstitute;
    using Web;
    using Xunit;

    [Trait("Category", "Extensions/WebLoggingExtensions.cs")]
    public class WebLoggingExtensionsTests
    {
        private readonly HttpRequest request;
        private ILogEntry logEntry;
        private const string requestUrl = "/url/suburl?querystring=1";
        private const string Message = "Message";
        private readonly SessionStateItemCollection sessionStateItemCollection;

        public WebLoggingExtensionsTests()
        {
            sessionStateItemCollection = new SessionStateItemCollection();
            logEntry = new LogEntry(Message);
            request = CreateFakeHttpRequest(requestUrl);
        }

        [Fact]
        public void AddRequestInfo_LogEntryIsNull_ReturnNull()
        {
            // Arrange
            logEntry = null;

            // Act
            logEntry = logEntry.AddRequestInfo(request);

            // Assert
            Assert.Null(logEntry);
        }

        [Fact]
        public void AddRequestInfo_HttpRequestIsNull_ReturnLogEntry()
        {
            // Act
            var actualLogEntry = logEntry.AddRequestInfo(null);

            // Assert
            Assert.Equal(logEntry.ToString(), actualLogEntry.ToString());
        }


        [Fact]
        public void AddRequestInfo_Always_ReturnLogEntryWithRequestInfo()
        {
            // Arrange
            var serverVariables = new NameValueCollection();
            serverVariables.Add("HTTP_X_FORWARDED_FOR", "10.19.10.52");

            // Act
            var actualLogEntry = logEntry.AddRequestInfo(HttpContext.Current.Request, serverVariables);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "REQUEST INFO:"
                        + "\r\n\tUrl: http://127.0.0.1/?/url/suburl?querystring=1"
                        + "\r\n\tUrl Referrer: "
                        + "\r\n\tForm Variables: "
                        + "\r\n\tBody: "
                        + "\r\n\tClient IP: 10.19.10.52"
                        + "\r\n\tCurrent Thread Culture: en-US",
                        actualLogEntry.ToString());
        }

        [Fact]
        public void GetClientIP_ServerVariablesContainsHTTP_X_FORWARDED_FOR_ReturnIP()
        {
            // Arrange
            var serverVariables = new NameValueCollection();
            serverVariables.Add("HTTP_X_FORWARDED_FOR", "10.19.10.52");

            // Act
            var clientIP = WebLoggingExtensions.GetClientIP(serverVariables);

            // Assert
            Assert.Equal("10.19.10.52", clientIP);
        }

        [Fact]
        public void GetClientIP_ServerVariablesContainsREMOTE_ADDR_ReturnIP()
        {
            // Arrange
            var serverVariables = new NameValueCollection();
            serverVariables.Add("REMOTE_ADDR", "10.19.10.52");

            // Act
            var clientIP = WebLoggingExtensions.GetClientIP(serverVariables);

            // Assert
            Assert.Equal("10.19.10.52", clientIP);
        }

        [Fact]
        public void GetClientIP_ServerVariablesHasNoData_ReturnDefaultIP()
        {
            // Arrange
            var serverVariables = new NameValueCollection();

            // Act
            var clientIP = WebLoggingExtensions.GetClientIP(serverVariables);

            // Assert
            Assert.Equal("127.0.0.1", clientIP);
        }

        [Fact]
        public void GetRequestBodyData_RequestStreamIsNull_ReturnEmpty()
        {
            // Act
            var bodyData = WebLoggingExtensions.GetRequestBodyData(null);

            // Assert
            Assert.Empty(bodyData);
        }

        [Fact]
        public void GetRequestBodyData_RequestStreamDoesNotContainJsonData_ReturnEmpty()
        {
            // Arrange
            using (Stream stream = GenerateStreamFromString("Not Json"))
            {
                // Act
                var bodyData = WebLoggingExtensions.GetRequestBodyData(stream);

                // Assert
                Assert.Empty(bodyData);
            }
        }

        [Theory]
        [InlineData("{ 'demo': 1, 'demo2': '33333' }", "{ 'demo': 1, 'demo2': '33333' }")]
        [InlineData("{ \\\"demo\\\": 1, \\\"demo2\\\": '33333' }", "{ \"demo\": 1, \"demo2\": '33333' }")]
        public void GetRequestBodyData_RequestStreamContainsJsonData_ReturnEmpty(
            string requestJson, 
            string expectedJson)
        {
            // Arrange
            using (Stream stream = GenerateStreamFromString(requestJson))
            {
                // Act
                var bodyData = WebLoggingExtensions.GetRequestBodyData(stream);

                // Assert
                Assert.Equal(expectedJson, bodyData);
            }
        }

        [Fact]
        public void AddSessionInfo_LogEntryIsNull_ReturnNull()
        {
            // Arrange
            logEntry = null;

            // Act
            logEntry = logEntry.AddSessionInfo(null);

            // Assert
            Assert.Null(logEntry);
        }

        [Fact]
        public void AddSessionInfo_SessionIsNull_ReturnLogEntry()
        {
            // Act
            var actualLogEntry = logEntry.AddSessionInfo(null);

            // Assert
            Assert.Equal(logEntry.ToString(), actualLogEntry.ToString());
        }

        [Fact]
        public void AddSessionInfo_SessionContainsData_ReturnLogEntryWithSessionData()
        {
            // Arrange
            sessionStateItemCollection["sessiondata1"] = 1;
            sessionStateItemCollection["sessiondata2"] = "data";
            sessionStateItemCollection["sessiondata3"] = new object();

            // Act
            var actualLogEntry = logEntry.AddSessionInfo(HttpContext.Current.Session);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "HTTP SESSION:"
                        + "\r\n\tsessiondata1: 1"
                        + "\r\n\tsessiondata2: data"
                        + "\r\n\tsessiondata3: System.Object", 
                        actualLogEntry.ToString());
        }

        [Fact]
        public void AddHeaderInfo_LogEntryIsNull_ReturnNull()
        {
            // Arrange
            logEntry = null;

            // Act
            logEntry = logEntry.AddHeaderInfo(null);

            // Assert
            Assert.Null(logEntry);
        }

        [Fact]
        public void AddHeaderInfo_HeadersIsNull_ReturnLogEntry()
        {
            // Act
            var actualLogEntry = logEntry.AddHeaderInfo(null);

            // Assert
            Assert.Equal(logEntry.ToString(), actualLogEntry.ToString());
        }

        [Fact]
        public void AddHeaderInfo_HeadersContainsData_ReturnLogEntryWithHeaderData()
        {
            // Arrange
            var headers = new NameValueCollection();
            headers.Add("headerdata1", "1");
            headers.Add("headerdata2", "data");

            // Act
            var actualLogEntry = logEntry.AddHeaderInfo(headers);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "HTTP HEADERS:"
                        + "\r\n\theaderdata1: 1"
                        + "\r\n\theaderdata2: data",
                        actualLogEntry.ToString());
        }

        [Fact]
        public void AddBrowserInfo_LogEntryIsNull_ReturnNull()
        {
            // Arrange
            logEntry = null;

            // Act
            logEntry = logEntry.AddBrowserInfo(Substitute.For<HttpCapabilitiesBase>(), new NameValueCollection());

            // Assert
            Assert.Null(logEntry);
        }

        [Fact]
        public void AddBrowserInfo_BrowserIsNull_ReturnLogEntry()
        {
            // Act
            var actualLogEntry = logEntry.AddBrowserInfo(null, new NameValueCollection());

            // Assert
            Assert.Equal(logEntry.ToString(), actualLogEntry.ToString());
        }

        [Fact]
        public void AddBrowserInfo_Always_ReturnLogEntryWithBrowserData()
        {
            // Arrange
            var userAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10";
            var browser = new HttpBrowserCapabilities
            {
                Capabilities = new Hashtable { { string.Empty, userAgent } }
            };

            var factory = new BrowserCapabilitiesFactory();
            factory.ConfigureBrowserCapabilities(new NameValueCollection(), browser);

            var serverVariables = new NameValueCollection();
            serverVariables.Add("HTTP_USER_AGENT", "Browser UserAgent");

            // Act
            var actualLogEntry = logEntry.AddBrowserInfo(browser, serverVariables);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "BROWSER INFO:"
                        + "\r\n\tBrowser: Firefox 3.6 (beta: False)"
                        + "\r\n\tPlatform: WinNT"
                        + "\r\n\tCrawler: False"
                        + "\r\n\tECMAScript: 3.0"
                        + "\r\n\tMobile: False (Unknown Unknown)"
                        + "\r\n\tUser Agent: Browser UserAgent",
                        actualLogEntry.ToString());
        }


        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private HttpRequest CreateFakeHttpRequest(string queryString)
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

            return HttpContext.Current.Request;
        }

        public static void AddServerVariable(HttpRequest request, string key, string value)
        {
            BindingFlags temp = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            MethodInfo addStatic = null;
            MethodInfo makeReadOnly = null;
            MethodInfo makeReadWrite = null;

            Type type = request.ServerVariables.GetType();
            MethodInfo[] methods = type.GetMethods(temp);
            foreach (MethodInfo method in methods)
            {
                switch (method.Name)
                {
                    case "MakeReadWrite":
                        makeReadWrite = method;
                        break;
                    case "MakeReadOnly":
                        makeReadOnly = method;
                        break;
                    case "AddStatic":
                        addStatic = method;
                        break;
                }
            }
            makeReadWrite.Invoke(request.ServerVariables, null);
            string[] values = { key, value };
            addStatic.Invoke(request.ServerVariables, values);
            makeReadOnly.Invoke(request.ServerVariables, null);
        }
    }
}