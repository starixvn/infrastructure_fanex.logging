namespace Fanex.Logging.Extensions.Web.UnitTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Hosting;
    using System.Web.SessionState;
    using Extensions;
    using Web;
    using Xunit;

    [Trait("Category", "Extensions/WebInfoGetters.cs")]
    public class WebInfoGettersTests
    {
        private readonly HttpRequest request;
        private LogMessage logMessage;
        private const string requestUrl = "/url/suburl?querystring=1";
        private const string message = "Message";
        private readonly SessionStateItemCollection sessionStateItemCollection;

        public WebInfoGettersTests()
        {
            sessionStateItemCollection = new SessionStateItemCollection();
            logMessage = new LogMessage(message);
            request = CreateFakeHttpRequest(requestUrl);
        }

        [Fact]
        public void GetRequestInfo_HttpRequestIsNull_ReturnNoInformationContent()
        {
            // Act
            var actualLogEntry = WebInfoGetters.GetRequestInfo(null);

            // Assert
            Assert.Same(Constants.DefaultDictionary, actualLogEntry);
        }

        [Fact]
        public void GetRequestInfo_Always_ReturnRequestInfo()
        {
            // Arrange
            var serverVariables = new NameValueCollection();
            serverVariables.Add("HTTP_X_FORWARDED_FOR", "10.19.10.52");
            serverVariables.Add("HTTP_USER_AGENT", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10");

            // Act
            var requestInfo = WebInfoGetters.GetRequestInfo(HttpContext.Current.Request, serverVariables);

            // Assert
            Assert.Equal(
                new Dictionary<string, object>
                {
                    ["Url"] = new Uri("http://127.0.0.1/?/url/suburl?querystring=1"),
                    ["UrlReferrer"] = null,
                    ["Form"] = HttpContext.Current.Request.Form,
                    ["UserHostAddress"] = "127.0.0.1",
                    ["InputStream"] = "N/A",
                    ["UserAgent"] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10",
                    ["Client IP"] = "10.19.10.52",
                    ["CurrentUICulture"] = Thread.CurrentThread.CurrentUICulture
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
            var clientIP = WebInfoGetters.GetClientIP(serverVariables);

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
            var clientIP = WebInfoGetters.GetClientIP(serverVariables);

            // Assert
            Assert.Equal("10.19.10.52", clientIP);
        }

        [Fact]
        public void GetClientIP_ServerVariablesHasNoData_ReturnDefaultIP()
        {
            // Arrange
            var serverVariables = new NameValueCollection();

            // Act
            var clientIP = WebInfoGetters.GetClientIP(serverVariables);

            // Assert
            Assert.Equal("127.0.0.1", clientIP);
        }

        [Fact]
        public void ConvertStreamToString_RequestStreamIsNull_ReturnNAString()
        {
            // Act
            var bodyData = WebInfoGetters.ConvertStreamToString(null);

            // Assert
            Assert.Equal("N/A", bodyData);
        }

        [Theory]
        [InlineData("{ 'demo': 1, 'demo2': '33333' }", "{ 'demo': 1, 'demo2': '33333' }")]
        [InlineData("{ \\\"demo\\\": 1, \\\"demo2\\\": '33333' }", "{ \"demo\": 1, \"demo2\": '33333' }")]
        public void ConvertStreamToString_RequestStreamContainsJsonData_ReturnEmpty(
            string requestJson,
            string expectedJson)
        {
            // Arrange
            using (Stream stream = GenerateStreamFromString(requestJson))
            {
                // Act
                var bodyData = WebInfoGetters.ConvertStreamToString(stream);

                // Assert
                Assert.Equal(expectedJson, bodyData);
            }
        }

        [Fact]
        public void GetSessionInfo_SessionIsNull_ReturnNoInfoContent()
        {
            // Act
            var sessionInfo = WebInfoGetters.GetSessionInfo(null);

            // Assert
            Assert.Equal(Constants.DefaultDictionary, sessionInfo);
        }

        [Fact]
        public void GetSessionInfo_SessionContainsData_ReturnSessionInfo()
        {
            // Arrange
            sessionStateItemCollection["sessiondata1"] = 1;
            sessionStateItemCollection["sessiondata2"] = "data";
            sessionStateItemCollection["sessiondata3"] = new object();

            // Act
            var sessionInfo = WebInfoGetters.GetSessionInfo(HttpContext.Current.Session);

            // Assert
            Assert.Equal(
                new Dictionary<string, object>
                {
                    ["sessiondata1"] = 1,
                    ["sessiondata2"] = "data",
                    ["sessiondata3"] = sessionStateItemCollection["sessiondata3"]
                },
                sessionInfo);
        }

        [Fact]
        public void GetRequestHeader_HeadersIsNull_ReturnNoInfoContent()
        {
            // Act
            var requestHeader = WebInfoGetters.GetRequestHeader(null);

            // Assert
            Assert.Equal(Constants.DefaultDictionary, requestHeader);
        }

        [Fact]
        public void GetRequestHeader_HeadersContainsData_ReturnRequestHeader()
        {
            // Arrange
            var headers = new NameValueCollection();
            headers.Add("headerdata1", "1");
            headers.Add("headerdata2", "data");

            // Act
            var requestHeader = WebInfoGetters.GetRequestHeader(headers);

            // Assert
            Assert.Equal(
                new Dictionary<string, object>
                {
                    ["headerdata1"] = "1",
                    ["headerdata2"] = "data"
                },
                requestHeader);
        }

        [Fact]
        public void GetBrowserInfo_BrowserIsNull_ReturnNoInfoContent()
        {
            // Act
            var browserInfo = WebInfoGetters.GetBrowserInfo(null);

            // Assert
            Assert.Equal(Constants.DefaultDictionary, browserInfo);
        }

        [Fact]
        public void GetBrowserInfo_Always_ReturnBrowserInfo()
        {
            // Arrange
            var browser = new HttpBrowserCapabilities
            {
                Capabilities = new Hashtable { { string.Empty, "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10" } }
            };
            var factory = new BrowserCapabilitiesFactory();
            factory.ConfigureBrowserCapabilities(new NameValueCollection(), browser);

            // Act
            var browserInfo = WebInfoGetters.GetBrowserInfo(browser);

            // Assert
            Assert.Equal(
                new Dictionary<string, object>
                {
                    ["Browser"] = "Firefox 3.6 (Beta: False)",
                    ["Platform"] = "WinNT",
                    ["Crawler"] = false,
                    ["EcmaScriptVersion"] = browser.EcmaScriptVersion,
                    ["MobileDeviceModel"] = "Unknown"
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
    }
}