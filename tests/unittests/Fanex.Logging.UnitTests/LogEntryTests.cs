namespace Fanex.Logging.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Logging;
    using Xunit;

    [Trait("Category", "LogEntry.cs")]
    public class LogEntryTests
    {
        [Fact]
        public void Constructor_DefaultConstructor_ReturnEmptyMessage()
        {
            // Act
            var actualMessage = new LogEntry().ToString();

            // Assert
            Assert.Empty(actualMessage);
        }

        [Fact]
        public void Constructor_ConstructorWithMessage_ReturnMessage()
        {
            // Arrange
            var message = "Message";

            // Act
            var actualMessage = new LogEntry(message).ToString();

            // Assert
            Assert.Equal(message, actualMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddInfo_InfomationIsNullOrWhiteSpace_ReturnEmptyMessage(string information)
        {
            // Arrange
            var logEntry = new LogEntry();

            // Act
            logEntry.AddInfo(information);

            // Assert
            Assert.Empty(logEntry.ToString());
        }

        [Fact]
        public void AddInfo_InformationIsNotEmpty_ReturnFormattedMessage()
        {
            // Arrange
            var logEntry = new LogEntry();
            var information = "Information";

            // Act
            logEntry.AddInfo(information);

            // Assert
            Assert.Equal("Information", logEntry.ToString());
        }

        [Fact]
        public void AddInfo_LogEntryHadInformation_InformationIsNotEmpty_ReturnFormattedMessage()
        {
            // Arrange
            var logEntry = new LogEntry("Information");
            var information = "New Information";

            // Act
            logEntry.AddInfo(information);

            // Assert
            Assert.Equal(
                "Information" 
                + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                + "New Information", 
                logEntry.ToString());
        }

        [Fact]
        public void HideSensitiveInfo_NoSensitiveParam_ReturnMessage()
        {
            // Arrange
            var logEntry = new LogEntry("Information");

            // Act
            logEntry.HideSensitiveInfo(Enumerable.Empty<string>(), "regexParam1");

            // Assert
            Assert.Equal("Information", logEntry.ToString());
        }

        [Fact]
        public void HideSensitiveInfo_NoSensitiveRegex_ReturnMessage()
        {
            // Arrange
            var logEntry = new LogEntry("Information");

            // Act
            logEntry.HideSensitiveInfo(
                new List<string>
                {
                    "password",
                    "securitycode"
                });

            // Assert
            Assert.Equal("Information", logEntry.ToString());
        }

        [Fact]
        public void HideSensitiveInfo_MessageContainsSensitiveInfo_RemoveSensitiveInfo()
        {
            // Arrange
            var logEntry = 
                new LogEntry("Information?securitycode=122323&password=434334&passerfdf=3434")
                .AddInfo("SECURITYCODE=43434")
                .AddInfo("PASSword=43333klkldkflkdf");

            // Act
            logEntry.HideSensitiveInfo(
                new List<string>
                {
                    "password",
                    "securitycode"
                },
                "(?:{0}=)(?<paramValue>[^&|^\\s]+)",
                "regex2");

            // Assert
            Assert.Equal(
                "Information?securitycode=&password=&passerfdf=3434"
                + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                + "SECURITYCODE="
                + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                + "PASSword=",
                logEntry.ToString());
        }
    }
}