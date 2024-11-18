namespace Fanex.Logging.Extensions.AspNetCore.UnitTests;

using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using Xunit;

[Trait("Unit Tests", "Extensions/AspNetCore/WebInfoGetters.cs")]
public class WebInfoGettersTests
{
    private readonly HttpRequest httpRequest;
    private readonly HttpContext httpContext;

    public WebInfoGettersTests()
    {
        httpContext = Substitute.For<HttpContext>();
        httpRequest = Substitute.For<HttpRequest>();
        httpContext.Request.Returns(httpRequest);
    }

    [Fact]
    public void GetRequestInfo_HttpRequestIsNull_ReturnNoInformationContent()
    {
        // Act
        var actualLogEntry = WebInfoGetters.GetRequestInfo(null);

        // Assert
        Assert.Empty(actualLogEntry);
    }

    [Fact]
    public void GetRequestInfo_Always_ReturnRequestInfo()
    {
        // Arrange
        httpRequest.Headers["User-Agent"].Returns(new StringValues("Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10"));
        httpRequest.Scheme.Returns("http");
        httpRequest.Host.Returns(new HostString("demo.com"));
        httpRequest.Path.Returns(new PathString("/path"));
        httpRequest.QueryString.Returns(new QueryString("?name=value"));
        httpRequest.HttpContext.Returns(httpContext);
        httpRequest.HasFormContentType.Returns(true);
        httpRequest.Form.Returns(new FormCollection(new Dictionary<string, StringValues>
        {
            { "name1", new StringValues("value1") },
            { "name2", new StringValues("value2") }
        }));

        var connectionInfo = Substitute.For<ConnectionInfo>();
        connectionInfo.RemoteIpAddress = new IPAddress(889852682);
        connectionInfo.LocalIpAddress = new IPAddress(873075466);

        httpContext.Connection.Returns(connectionInfo);

        // Act
        var requestInfo = WebInfoGetters.GetRequestInfo(httpContext);

        // Assert
        AssertExtensions.Equal(
            new Dictionary<string, object>
            {
                ["Url"] = "http://demo.com/path?name=value",
                ["UrlReferrer"] = string.Empty,
                ["Form"] = new Dictionary<string, object>
                {
                    { "name1", new[] { "value1" } },
                    { "name2", new[] { "value2" } }
                },
                ["IpAddress"] = "Remote: 10.19.10.53,Local: 10.19.10.52",
                ["InputStream"] = "N/A",
                ["UserAgent"] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10",
                ["CurrentUICulture"] = Thread.CurrentThread.CurrentUICulture
            },
            requestInfo);
    }

    [Fact]
    public void GetBrowserInfo_Always_ReturnBrowserInfo()
    {
        // Arrange
        httpRequest.Headers.ContainsKey("User-Agent").Returns(true);
        httpRequest.Headers["User-Agent"].Returns(new StringValues("Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10"));

        // Act
        var browserInfo = WebInfoGetters.GetBrowserInfo(httpRequest);

        // Assert
        AssertExtensions.Equal(
            new Dictionary<string, object>
            {
                ["Platform"] = "Windows 7",
                ["Browser"] = "Firefox 3.6.10"
            },
            browserInfo);
    }

    [Fact]
    public void GetBrowserInfo_BrowserIsNull_ReturnNoInfoContent()
    {
        // Act
        var browserInfo = WebInfoGetters.GetBrowserInfo(null);

        // Assert
        Assert.Equal(Constants.EmptyDictionary, browserInfo);
    }

    [Fact]
    public void GetRequestHeader_HeadersContainsData_ReturnRequestHeader()
    {
        // Arrange
        var headers = new HeaderDictionary
        {
            { "headerdata1", "1" },
            { "headerdata2", "data" }
        };

        // Act
        var requestHeader = WebInfoGetters.GetRequestHeader(headers);

        // Assert
        AssertExtensions.Equal(
            new Dictionary<string, object>
            {
                ["headerdata1"] = new[] { "1" },
                ["headerdata2"] = new[] { "data" }
            },
            requestHeader);
    }

    [Fact]
    public void GetRequestHeader_HeadersIsNull_ReturnNoInfoContent()
    {
        // Act
        var requestHeader = WebInfoGetters.GetRequestHeader(null);

        // Assert
        Assert.Equal(Constants.EmptyDictionary, requestHeader);
    }

    [Fact]
    public void GetSessionInfo_SessionIsNull_ReturnNoInfoContent()
    {
        // Act
        var sessionInfo = WebInfoGetters.GetSessionInfo(null);

        // Assert
        Assert.Equal(Constants.EmptyDictionary, sessionInfo);
    }

    [Fact]
    public void GetSessionInfo_SessionContainsData_ReturnSessionInfo()
    {
        // Arrange
        var sessionDictionary = new Dictionary<string, string>
        {
            ["sessiondata1"] = "1",
            ["sessiondata2"] = "data"
        };

        var session = Substitute.For<ISession>();
        session.Keys.Returns(sessionDictionary.Keys);
        session
            .TryGetValue(Arg.Any<string>(), out _)
            .Returns(x =>
            {
                var key = x[0].ToString() ?? string.Empty;
                x[1] = Encoding.UTF8.GetBytes(sessionDictionary[key]);

                return true;
            });

        httpContext.Features.Get<ISessionFeature>().Returns(Substitute.For<ISessionFeature>());
        httpContext.Session.Returns(session);

        // Act
        var sessionInfo = WebInfoGetters.GetSessionInfo(httpContext);

        // Assert
        AssertExtensions.Equal(
            new Dictionary<string, object>
            {
                ["sessiondata1"] = "1",
                ["sessiondata2"] = "data"
            },
            sessionInfo);
    }
}