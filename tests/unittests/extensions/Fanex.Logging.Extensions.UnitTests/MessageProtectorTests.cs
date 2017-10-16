namespace Fanex.Logging.Extensions.UnitTests
{
    using System.Collections.Generic;
    using Xunit;

    [Trait("Unit Tests", "Extensions/MessageProtector.cs")]
    public class MessageProtectorTests
    {
        [Fact]
        public void HideSensitiveInfo_PatternDoesNotMatch_ReturnMessage()
        {
            // Act
            var message = "Information".HideSensitiveInfo(new List<string> { "regexParam1" });

            // Assert
            Assert.Equal("Information", message);
        }

        [Fact]
        public void HideSensitiveInfo_MessageContainsSensitiveInfo_RemoveSensitiveInfo()
        {
            // Arrange
            var message = "Information?securitycode=122323&password=434334&passerfdf=3434&pasSWORD=4545454";

            // Act
            var actualMessage = message.HideSensitiveInfo(
                new List<string>
                {
                    "(?:password=)(?<paramValue>[^&|^\\s]+)",
                    "(?:securitycode=)(?<paramValue>[^&|^\\s]+)"
                });

            // Assert
            Assert.Equal(
                "Information?securitycode=*****&password=*****&passerfdf=3434&pasSWORD=*****",
                actualMessage);
        }

        [Theory]
        [InlineData(
            "Information?securitycode=122323&password=434334&passerfdf=3434&pasSWORD=4545454",
            "Information?securitycode=*****&password=*****&passerfdf=3434&pasSWORD=*****\r\n")]
        [InlineData(
            "{\"password\":BB4,\"seccode\":BB5,\"securitycode\":BB6,\"coug\":BB7}",
            "{\"password\":*****,\"seccode\":*****,\"securitycode\":*****,\"coug\":BB7}\r\n")]
        [InlineData(
            "?password=password&seccode=BB2&securitycode=password&count=3&brc={\"password\":BB4,\"seccode\":BB5,\"securitycode\":BB6,\"coug\":BB7}",
            "?*****=*****&seccode=*****&securitycode=*****&count=3&brc={\"*****\":BB4,\"seccode\":*****,\"securitycode\":*****,\"coug\":BB7}\r\n")]
        public void ToCleanMessage_MessageContainsSensitiveInfo_RemoveSensitiveInfo(string message, string expectedCleanMessage)
        {
            // Arrange
            var logMessage = new LogMessage(message);

            // Act
            var actualLogMessage = logMessage.ToCleanMessage(
                new List<string>
                {
                    "(?:password=)(?<paramValue>[^&|^\\s]+)",
                    "(\"password\":)(?<paramValue>[^,|^\r]+)",
                    "(?:seccode=)(?<paramValue>[^&|^\\s]+)",
                    "(\"seccode\":)(?<paramValue>[^,|^\r]+)",
                    "(?:securitycode=)(?<paramValue>[^&|^\\s]+)",
                    "(\"securitycode\":)(?<paramValue>[^,|^\r]+)"
                });

            // Assert
            Assert.Equal(
                expectedCleanMessage,
                actualLogMessage.ToString());
        }
    }
}