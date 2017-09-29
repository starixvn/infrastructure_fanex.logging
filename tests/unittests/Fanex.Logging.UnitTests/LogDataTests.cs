namespace Fanex.Logging.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Logging;
    using Xunit;

    [Trait("Category", "LogData.cs")]
    public class LogDataTests
    {
        [Fact]
        public void Constructor_DefaultConstructor_ReturnEmptyMessage()
        {
            // Act
            var actualMessage = new LogData().ToString();

            // Assert
            Assert.Empty(actualMessage);
        }

        [Fact]
        public void Constructor_ConstructorWithMessage_ReturnMessage()
        {
            // Arrange
            var message = "Message";

            // Act
            var actualMessage = new LogData(message).ToString();

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
            var logData = new LogData();

            // Act
            logData.AddInfo(information);

            // Assert
            Assert.Empty(logData.ToString());
        }

        [Fact]
        public void AddInfo_InformationIsNotEmpty_ReturnFormattedMessage()
        {
            // Arrange
            var logData = new LogData();
            var information = "Information";

            // Act
            logData.AddInfo(information);

            // Assert
            Assert.Equal("Information", logData.ToString());
        }

        [Fact]
        public void AddContent_ExtractorIsNull_ThrowArgumentNullException()
        {
            var logData = new LogData();

            Assert.Throws<ArgumentNullException>(() => logData.AddContent(null));
        }

        [Theory]
        [InlineData("initial message 1", "message to be append 1" )]
        [InlineData("initial message 2", "message to be append 2" )]        
        public void AddContent_ExtractorIsNotNull_AppendMessageToLogData(string initialMessage, string appendMessage)
        {
            var logData = new LogData(initialMessage);
            
            logData.AddContent(() => appendMessage);

            Assert.Equal(logData.ToString(), initialMessage + appendMessage + Environment.NewLine);
        }

        [Fact]
        public void AddInfo_LogDataHadInformation_InformationIsNotEmpty_ReturnFormattedMessage()
        {
            // Arrange
            var logData = new LogData("Information");
            var information = "New Information";

            // Act
            logData.AddInfo(information);

            // Assert
            Assert.Equal(
                "Information"
                + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                + "New Information",
                logData.ToString());
        }

        [Fact]
        public void HideSensitiveInfo_NoSensitiveParam_ReturnMessage()
        {
            // Arrange
            var logData = new LogData("Information");

            // Act
            logData.HideSensitiveInfo(Enumerable.Empty<string>(), "regexParam1");

            // Assert
            Assert.Equal("Information", logData.ToString());
        }

        [Fact]
        public void HideSensitiveInfo_NoSensitiveRegex_ReturnMessage()
        {
            // Arrange
            var logData = new LogData("Information");

            // Act
            logData.HideSensitiveInfo(
                new List<string>
                {
                    "password",
                    "securitycode"
                });

            // Assert
            Assert.Equal("Information", logData.ToString());
        }

        [Fact]
        public void HideSensitiveInfo_MessageContainsSensitiveInfo_RemoveSensitiveInfo()
        {
            // Arrange
            var logData =
                new LogData("Information?securitycode=122323&password=434334&passerfdf=3434")
                .AddInfo("SECURITYCODE=43434")
                .AddInfo("PASSword=43333klkldkflkdf");

            // Act
            logData.HideSensitiveInfo(
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
                logData.ToString());
        }
    }
}