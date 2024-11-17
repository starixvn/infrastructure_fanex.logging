namespace Fanex.Logging.Extensions.UnitTests;

using System;
using System.Collections.Generic;
using Xunit;

[Trait("Unit Tests", "Extensions/LogMessage.cs")]
public class LogMessageTests
{
    [Fact]
    public void Constructor_DefaultConstructor_ReturnEmptyMessage()
    {
        // Act
        var actualMessage = new LogMessage().ToString();

        // Assert
        Assert.Empty(actualMessage);
    }

    [Fact]
    public void Constructor_ConstructorWithMessage_ReturnMessage()
    {
        // Arrange
        const string message = "Message";

        // Act
        var actualMessage = new LogMessage(message);

        // Assert
        Assert.Equal("Message\r\n", actualMessage.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void AddSection_SectionIsNullOrWhiteSpace_ReturnEmptyMessage(string information)
    {
        // Arrange
        var logMessage = new LogMessage();

        // Act
        logMessage.AddSection(information, () => new Dictionary<string, object>());

        // Assert
        Assert.Empty(logMessage.ToString());
    }

    [Fact]
    public void AddSection_SectionHasData_ReturnLogMessageWithSection()
    {
        // Arrange
        var logMessage = new LogMessage("Message");

        // Act
        logMessage.AddSection("section", () => new Dictionary<string, object>
        {
            ["data key"] = "data value"
        });

        // Assert
        Assert.Equal("Message\r\n\r\nSECTION\r\n\tdata key: data value\r\n", logMessage.ToString());
    }

    [Theory]
    [InlineData("initial message 1", "message to be append 1")]
    [InlineData("initial message 2", "message to be append 2")]
    public void AddMessage_MessageIsNotNull_AppendMessageToLogMessage(string initialMessage, string appendMessage)
    {
        var logMessage = new LogMessage(initialMessage);

        logMessage.AddMessage(appendMessage);

        Assert.Equal(
            logMessage.ToString(),
            initialMessage + Environment.NewLine + appendMessage + Environment.NewLine);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Section_SectionNameIsNullOrWhiteSpace_NotAddSection(string sectionName)
    {
        // Arrange
        var logMessage = new LogMessage("Information");

        // Act
        logMessage.AddSection(sectionName, new Dictionary<string, object>());

        // Assert
        Assert.Equal("Information\r\n", logMessage.ToString());
    }

    [Fact]
    public void AddSection_NoData_ReturnSectionWithNoInfo()
    {
        // Arrange
        var logMessage = new LogMessage("Information");
        const string sectionName = "SectionName";

        // Act
        logMessage.AddSection(sectionName, new Dictionary<string, object>());

        // Assert
        Assert.Equal("Information\r\n\r\nSECTIONNAME\r\nNo Information is available.\r\n", logMessage.ToString());
    }

    [Fact]
    public void AddData_Always_AppendData()
    {
        // Arrange
        var logMessage = new LogMessage("Information");
        var data = new Dictionary<string, object>
        {
            ["data1"] = "Content1",
            ["data2"] = "Content2",
            ["data3"] = "Content3"
        };

        // Act
        logMessage.AddData(data);

        // Assert
        Assert.Equal("Information\r\n\tdata1: Content1\r\n\tdata2: Content2\r\n\tdata3: Content3\r\n", logMessage.ToString());
    }

    [Fact]
    public void AddData_Always_RunAndAppendData()
    {
        // Arrange
        var logMessage = new LogMessage("Information");

        // Act
        logMessage.AddData(() => new Dictionary<string, object>
        {
            ["data1"] = "Content1",
            ["data2"] = "Content2",
            ["data3"] = "Content3"
        });

        // Assert
        Assert.Equal("Information\r\n\tdata1: Content1\r\n\tdata2: Content2\r\n\tdata3: Content3\r\n", logMessage.ToString());
    }
}