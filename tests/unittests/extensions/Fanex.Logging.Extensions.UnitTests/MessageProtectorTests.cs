namespace Fanex.Logging.Extensions.UnitTests
{
    using System.Collections.Generic;
    using Web;
    using Xunit;

    [Trait("Category", "Extensions/MessageProtector.cs")]
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

        [Fact]
        public void ToCleanMessage_MessageContainsSensitiveInfo_RemoveSensitiveInfo()
        {
            // Arrange
            var logMessage = new LogMessage("Information?securitycode=122323&password=434334&passerfdf=3434&pasSWORD=4545454");

            // Act
            var actualLogMessage = logMessage.ToCleanMessage(
                new List<string>
                {
                    "(?:password=)(?<paramValue>[^&|^\\s]+)",
                    "(?:securitycode=)(?<paramValue>[^&|^\\s]+)"
                });

            // Assert
            Assert.Equal(
                "Information?securitycode=*****&password=*****&passerfdf=3434&pasSWORD=*****\r\n",
                actualLogMessage.ToString());
        }
    }
}