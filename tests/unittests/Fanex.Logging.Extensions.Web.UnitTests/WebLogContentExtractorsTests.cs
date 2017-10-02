namespace Fanex.Logging.Extensions.Web.UnitTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
    using static Fanex.Logging.Extensions.Web.Extensions;

    [Trait("Category", "Extensions/WebLogContentExtractors.cs")]
    public class WebLogContentExtractorsTests
    {
        private readonly HttpRequest request;
        private ILogEntry logEntry;
        private const string requestUrl = "/url/suburl?querystring=1";
        private const string Message = "Message";
        private readonly SessionStateItemCollection sessionStateItemCollection;

        public WebLogContentExtractorsTests()
        {
            sessionStateItemCollection = new SessionStateItemCollection();
            logEntry = new LogEntry(Message);
            request = CreateFakeHttpRequest(requestUrl);
        }

        [Fact]
        public void GetRequestInfo_HttpRequestIsNull_ReturnNoInformationContent()
        {
            // Act
            var actualLogEntry = WebLogContentExtractors.GetRequestInfo(null);

            // Assert
            Assert.Same(Constants.NoInformationContent, actualLogEntry);
        }

        [Fact]
        public void GetRequestInfo_Always_ReturnRequestInfo()
        {
            // Arrange
            var serverVariables = new NameValueCollection();
            serverVariables.Add("HTTP_X_FORWARDED_FOR", "10.19.10.52");

            // Act
            var requestInfo = WebLogContentExtractors.GetRequestInfo(HttpContext.Current.Request, serverVariables);

            // Assert
            Assert.Equal(
                new List<string>
                {
                    "Url: http://127.0.0.1/?/url/suburl?querystring=1",
                    "Url Referrer: ",
                    "Form Variables: ",
                    "Body: N/A",
                    "Client IP: 10.19.10.52",
                    "Current Thread Culture: en-US"
                },
                requestInfo);
        }

        [Fact]
        public void GetClientIP_ServerVariablesContainsHTTP_X_FORWARDED_FOR_ReturnIP()
        {
            // Arrange
            var serverVariables = new NameValueCollection();
            serverVariables.Add("HTTP_X_FORWARDED_FOR", "10.19.10.52");

            // Act
            var clientIP = Extensions.GetClientIP(serverVariables);

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
            var clientIP = Extensions.GetClientIP(serverVariables);

            // Assert
            Assert.Equal("10.19.10.52", clientIP);
        }

        [Fact]
        public void GetClientIP_ServerVariablesHasNoData_ReturnDefaultIP()
        {
            // Arrange
            var serverVariables = new NameValueCollection();

            // Act
            var clientIP = Extensions.GetClientIP(serverVariables);

            // Assert
            Assert.Equal("127.0.0.1", clientIP);
        }

        [Fact]
        public void GetRequestBodyData_RequestStreamIsNull_ReturnNAString()
        {
            // Act
            var bodyData = Extensions.GetRequestBodyData(null);

            // Assert
            Assert.Equal("N/A", bodyData);
        }

        [Fact]
        public void GetRequestBodyData_RequestStreamDoesNotContainJsonData_ReturnNAString()
        {
            // Arrange
            using (Stream stream = GenerateStreamFromString("Not Json"))
            {
                // Act
                var bodyData = Extensions.GetRequestBodyData(stream);

                // Assert
                Assert.Equal("N/A", bodyData);
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
                var bodyData = Extensions.GetRequestBodyData(stream);

                // Assert
                Assert.Equal(expectedJson, bodyData);
            }
        }

        [Fact]
        public void GetSessionInfo_SessionIsNull_ReturnNoInfoContent()
        {
            // Act
            var sessionInfo = WebLogContentExtractors.GetSessionInfo(null);

            // Assert
            Assert.Equal(Constants.NoInformationContent, sessionInfo);
        }

        [Fact]
        public void GetSessionInfo_SessionContainsData_ReturnSessionInfo()
        {
            // Arrange
            sessionStateItemCollection["sessiondata1"] = 1;
            sessionStateItemCollection["sessiondata2"] = "data";
            sessionStateItemCollection["sessiondata3"] = new object();

            // Act
            var sessionInfo = WebLogContentExtractors.GetSessionInfo(HttpContext.Current.Session);

            // Assert
            Assert.Equal(
                new List<string>
                {
                    "sessiondata1: 1",
                    "sessiondata2: data",
                    "sessiondata3: System.Object"
                },
                sessionInfo);
        }

        [Fact]
        public void GetRequestHeader_HeadersIsNull_ReturnNoInfoContent()
        {
            // Act
            var requestHeader = WebLogContentExtractors.GetRequestHeader(null);

            // Assert
            Assert.Equal(Constants.NoInformationContent, requestHeader);
        }

        [Fact]
        public void GetRequestHeader_HeadersContainsData_ReturnRequestHeader()
        {
            // Arrange
            var headers = new NameValueCollection();
            headers.Add("headerdata1", "1");
            headers.Add("headerdata2", "data");

            // Act
            var requestHeader = WebLogContentExtractors.GetRequestHeader(headers);

            // Assert
            Assert.Equal(
                new List<string>
                {
                    "headerdata1: 1",
                    "headerdata2: data"
                },
                requestHeader);
        }

        [Fact]
        public void GetBrowserInfo_BrowserIsNull_ReturnNoInfoContent()
        {
            // Act
            var browserInfo = WebLogContentExtractors.GetBrowserInfo(null, new NameValueCollection());

            // Assert
            Assert.Equal(Constants.NoInformationContent, browserInfo);
        }

        [Fact]
        public void GetBrowserInfo_ServerVariablesAreNull_ReturnNoInfoContent()
        {
            // Act
            var browserInfo = WebLogContentExtractors.GetBrowserInfo(new HttpBrowserCapabilities(), null);

            // Assert
            Assert.Equal(Constants.NoInformationContent, browserInfo);
        }

        [Fact]
        public void GetBrowserInfo_Always_ReturnBrowserInfo()
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
            var browserInfo = WebLogContentExtractors.GetBrowserInfo(browser, serverVariables);

            // Assert
            Assert.Equal(
                new List<string>
                {
                    "Browser: Firefox 3.6 (Beta: False)",
                    "Platform: WinNT",
                    "Crawler: False",
                    "ECMAScript: 3.0",
                    "Mobile: False (Unknown Unknown)",
                    "User Agent: Browser UserAgent"
                },
                browserInfo);
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