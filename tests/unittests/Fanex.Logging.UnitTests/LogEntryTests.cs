namespace Fanex.Logging.UnitTests
{
    using System;
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
            Assert.Equal("Message\r\n", actualMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddContent_InfomationIsNullOrWhiteSpace_ReturnEmptyMessage(string information)
        {
            // Arrange
            var logEntry = new LogEntry();

            // Act
            logEntry.AddContent(() => information);

            // Assert
            Assert.Empty(logEntry.ToString());
        }

        [Fact]
        public void AddContent_InformationIsNotEmpty_ReturnLogEntryWithInformation()
        {
            // Arrange
            var logEntry = new LogEntry();
            var information = "Information";

            // Act
            logEntry.AddContent(() => information);

            // Assert
            Assert.Equal("Information\r\n", logEntry.ToString());
        }

        [Fact]
        public void AddContent_ExtractorIsNull_ThrowArgumentNullException()
        {
            var logEntry = new LogEntry();
            Func<string> getContentFunc = null;

            Assert.Throws<ArgumentNullException>(() => logEntry.AddContent(getContentFunc));
        }

        [Theory]
        [InlineData("initial message 1", "message to be append 1" )]
        [InlineData("initial message 2", "message to be append 2" )]        
        public void AddContent_ExtractorIsNotNull_AppendMessageToLogEntry(string initialMessage, string appendMessage)
        {
            var logEntry = new LogEntry(initialMessage);
            
            logEntry.AddContent(() => appendMessage);

            Assert.Equal(
                logEntry.ToString(), 
                initialMessage + Environment.NewLine + appendMessage + Environment.NewLine);
        }

        [Fact]
        public void HideSensitiveInfo_NoSensitiveParam_ReturnMessage()
        {
            // Arrange
            var logEntry = new LogEntry("Information");

            // Act
            logEntry.HideSensitiveInfo(Enumerable.Empty<string>(), "regexParam1");

            // Assert
            Assert.Equal("Information\r\n", logEntry.ToString());
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
            Assert.Equal("Information\r\n", logEntry.ToString());
        }

        [Fact]
        public void HideSensitiveInfo_MessageContainsSensitiveInfo_RemoveSensitiveInfo()
        {
            // Arrange
            var logEntry = new LogEntry("Information?securitycode=122323&password=434334&passerfdf=3434");
            logEntry.AddContent(() => "SECURITYCODE=43434")
                .AddContent(() => "PASSword=43333klkldkflkdf");

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
                + "\r\n"
                + "SECURITYCODE="
                + "\r\n"
                + "PASSword="
                + "\r\n",
                logEntry.ToString());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Section_SectionNameIsNullOrWhiteSpace_NotAddSection(string sectionName)
        {
            // Arrange
            var logEntry = new LogEntry("Information");

            // Act
            logEntry.Section(sectionName);

            // Assert
            Assert.Equal("Information\r\n", logEntry.ToString());
        }

        [Fact]
        public void Section_Always_AddSection()
        {
            // Arrange
            var logEntry = new LogEntry("Information");
            var sectionName = "SectionName";

            // Act
            logEntry.Section(sectionName);

            // Assert
            Assert.Equal("Information\r\n\r\nSectionName\r\n", logEntry.ToString());
        }


        [Fact]
        public void AddContent_ContentsIsNull_ThrowArgumentNullException()
        {
            // Arrange
            var logEntry = new LogEntry("Information");
            List<string> nullContents = null;

            // Act & Arrange
            Assert.Throws<ArgumentNullException>(() => logEntry.AddContent(nullContents));
        }

        [Fact]
        public void AddContent_Always_AddContents()
        {
            // Arrange
            var logEntry = new LogEntry("Information");
            var contents = new List<string>
            {
                "Content1",
                "Content2",
                "Content3",
            };

            // Act
            logEntry.AddContent(contents);

            // Assert
            Assert.Equal("Information\r\n\tContent1\r\n\tContent2\r\n\tContent3\r\n", logEntry.ToString());
        }
    }
}