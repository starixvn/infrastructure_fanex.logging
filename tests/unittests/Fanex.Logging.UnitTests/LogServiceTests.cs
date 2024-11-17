using Xunit;

namespace Fanex.Logging.UnitTests;

using System;
using NSubstitute;

[Trait("Unit Tests", "LogService.cs")]
public class LogServiceTests
{
    private readonly ILogger logger;

    public LogServiceTests()
    {
        logger = Substitute.For<ILogger>();
        LogService.LoggerInstance = logger;
    }

    #region Debug

    [Fact]
    public void Debug_Overload1_Always_ExecuteLoggerDebug()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.Debug(message);

        // Assert
        logger.Received().Debug(message);
    }

    [Fact]
    public void Debug_Overload2_Always_ExecuteLoggerDebug()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.Debug(message, category);

        // Assert
        logger.Received().Debug(message, category);
    }

    [Fact]
    public void Debug_Overload3_Always_ExecuteLoggerDebug()
    {
        // Arrange
        const string message = "Message";
        var args = new object[] { "string 1" };

        // Act
        LogService.Debug(message, args);

        // Assert
        logger.Received().Debug(message, args);
    }

    [Fact]
    public void Debug_Overload4_Always_ExecuteLoggerDebug()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var args = new object[] { "string 1" };

        // Act
        LogService.Debug(message, category, args);

        // Assert
        logger.Received().Debug(message, category, args);
    }

    #endregion Debug

    #region Info

    [Fact]
    public void Info_Overload1_Always_ExecuteLoggerInfo()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.Info(message);

        // Assert
        logger.Received().Info(message);
    }

    [Fact]
    public void Info_Overload2_Always_ExecuteLoggerInfo()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.Info(message, category);

        // Assert
        logger.Received().Info(message, category);
    }

    [Fact]
    public void Info_Overload3_Always_ExecuteLoggerInfo()
    {
        // Arrange
        const string message = "Message";
        var args = new object[] { "string 1" };

        // Act
        LogService.Info(message, args);

        // Assert
        logger.Received().Info(message, args);
    }

    [Fact]
    public void Info_Overload4_Always_ExecuteLoggerInfo()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var args = new object[] { "string 1" };

        // Act
        LogService.Info(message, category, args);

        // Assert
        logger.Received().Info(message, category, args);
    }

    #endregion Info

    #region Warn

    [Fact]
    public void Warn_Overload1_Always_ExecuteLoggerWarn()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.Warn(message);

        // Assert
        logger.Received().Warn(message);
    }

    [Fact]
    public void Warn_Overload2_Always_ExecuteLoggerWarn()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.Warn(message, category);

        // Assert
        logger.Received().Warn(message, category);
    }

    [Fact]
    public void Warn_Overload3_Always_ExecuteLoggerWarn()
    {
        // Arrange
        const string message = "Message";
        var args = new object[] { "string 1" };

        // Act
        LogService.Warn(message, args);

        // Assert
        logger.Received().Warn(message, args);
    }

    [Fact]
    public void Warn_Overload4_Always_ExecuteLoggerWarn()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var args = new object[] { "string 1" };

        // Act
        LogService.Warn(message, category, args);

        // Assert
        logger.Received().Warn(message, category, args);
    }

    #endregion Warn

    #region Critical

    [Fact]
    public void Critical_Overload1_Always_ExecuteLoggerCritical()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.Critical(message);

        // Assert
        logger.Received().Critical(message);
    }

    [Fact]
    public void Critical_Overload2_Always_ExecuteLoggerCritical()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.Critical(message, category);

        // Assert
        logger.Received().Critical(message, category);
    }

    [Fact]
    public void Critical_Overload3_Always_ExecuteLoggerCritical()
    {
        // Arrange
        const string message = "Message";
        var args = new object[] { "string 1" };

        // Act
        LogService.Critical(message, args);

        // Assert
        logger.Received().Critical(message, args);
    }

    [Fact]
    public void Critical_Overload4_Always_ExecuteLoggerCritical()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var args = new object[] { "string 1" };

        // Act
        LogService.Critical(message, category, args);

        // Assert
        logger.Received().Critical(message, category, args);
    }

    #endregion Critical

    #region Error

    [Fact]
    public void Error_Overload1_Always_ExecuteLoggerError()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.Error(message);

        // Assert
        logger.Received().Error(message);
    }

    [Fact]
    public void Error_Overload2_Always_ExecuteLoggerError()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.Error(message, category);

        // Assert
        logger.Received().Error(message, category);
    }

    [Fact]
    public void Error_Overload3_Always_ExecuteLoggerError()
    {
        // Arrange
        const string message = "Message";
        var exception = new Exception();

        // Act
        LogService.Error(message, exception);

        // Assert
        logger.Received().Error(message, exception);
    }

    [Fact]
    public void Error_Overload4_Always_ExecuteLoggerError()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var exception = new Exception();

        // Act
        LogService.Error(message, category, exception);

        // Assert
        logger.Received().Error(message, category, exception);
    }

    #endregion Error
}