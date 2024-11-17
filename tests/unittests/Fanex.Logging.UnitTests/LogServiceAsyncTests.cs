using Xunit;

namespace Fanex.Logging.UnitTests;

using System;
using NSubstitute;

[Trait("Unit Tests", "LogService.cs")]
public class LogServiceAsyncTests
{
    private readonly ILogger logger;

    public LogServiceAsyncTests()
    {
        logger = Substitute.For<ILogger>();
        LogService.LoggerInstance = logger;
    }

    #region Debug

    [Fact]
    public void DebugAsync_Overload1_Always_ExecuteLoggerDebug()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.DebugAsync(message).GetAwaiter().GetResult();

        // Assert
        logger.Received().DebugAsync(message);
    }

    [Fact]
    public void DebugAsync_Overload2_Always_ExecuteLoggerDebug()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.DebugAsync(message, category).GetAwaiter().GetResult();

        // Assert
        logger.Received().DebugAsync(message, category);
    }

    [Fact]
    public void DebugAsync_Overload3_Always_ExecuteLoggerDebug()
    {
        // Arrange
        const string message = "Message";
        var args = new object[] { "string 1" };

        // Act
        LogService.DebugAsync(message, args).GetAwaiter().GetResult();

        // Assert
        logger.Received().DebugAsync(message, args);
    }

    [Fact]
    public void DebugAsync_Overload4_Always_ExecuteLoggerDebug()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var args = new object[] { "string 1" };

        // Act
        LogService.DebugAsync(message, category, args).GetAwaiter().GetResult();

        // Assert
        logger.Received().DebugAsync(message, category, args);
    }

    #endregion Debug

    #region Info

    [Fact]
    public void InfoAsync_Overload1_Always_ExecuteLoggerInfo()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.InfoAsync(message).GetAwaiter().GetResult();

        // Assert
        logger.Received().InfoAsync(message);
    }

    [Fact]
    public void InfoAsync_Overload2_Always_ExecuteLoggerInfo()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.InfoAsync(message, category).GetAwaiter().GetResult();

        // Assert
        logger.Received().InfoAsync(message, category);
    }

    [Fact]
    public void InfoAsync_Overload3_Always_ExecuteLoggerInfo()
    {
        // Arrange
        const string message = "Message";
        var args = new object[] { "string 1" };

        // Act
        LogService.InfoAsync(message, args).GetAwaiter().GetResult();

        // Assert
        logger.Received().InfoAsync(message, args);
    }

    [Fact]
    public void InfoAsync_Overload4_Always_ExecuteLoggerInfo()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var args = new object[] { "string 1" };

        // Act
        LogService.InfoAsync(message, category, args).GetAwaiter().GetResult();

        // Assert
        logger.Received().InfoAsync(message, category, args);
    }

    #endregion Info

    #region Warn

    [Fact]
    public void WarnAsync_Overload1_Always_ExecuteLoggerWarn()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.WarnAsync(message).GetAwaiter().GetResult();

        // Assert
        logger.Received().WarnAsync(message);
    }

    [Fact]
    public void WarnAsync_Overload2_Always_ExecuteLoggerWarn()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.WarnAsync(message, category).GetAwaiter().GetResult();

        // Assert
        logger.Received().WarnAsync(message, category);
    }

    [Fact]
    public void WarnAsync_Overload3_Always_ExecuteLoggerWarn()
    {
        // Arrange
        const string message = "Message";
        var args = new object[] { "string 1" };

        // Act
        LogService.WarnAsync(message, args).GetAwaiter().GetResult();

        // Assert
        logger.Received().WarnAsync(message, args);
    }

    [Fact]
    public void WarnAsync_Overload4_Always_ExecuteLoggerWarn()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var args = new object[] { "string 1" };

        // Act
        LogService.WarnAsync(message, category, args).GetAwaiter().GetResult();

        // Assert
        logger.Received().WarnAsync(message, category, args);
    }

    #endregion Warn

    #region CriticalAsync

    [Fact]
    public void CriticalAsync_Overload1_Always_ExecuteLoggerCriticalAsync()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.CriticalAsync(message).GetAwaiter().GetResult();

        // Assert
        logger.Received().CriticalAsync(message);
    }

    [Fact]
    public void CriticalAsync_Overload2_Always_ExecuteLoggerCriticalAsync()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.CriticalAsync(message, category).GetAwaiter().GetResult();

        // Assert
        logger.Received().CriticalAsync(message, category);
    }

    [Fact]
    public void CriticalAsync_Overload3_Always_ExecuteLoggerCriticalAsync()
    {
        // Arrange
        const string message = "Message";
        var args = new object[] { "string 1" };

        // Act
        LogService.CriticalAsync(message, args).GetAwaiter().GetResult();

        // Assert
        logger.Received().CriticalAsync(message, args);
    }

    [Fact]
    public void CriticalAsync_Overload4_Always_ExecuteLoggerCriticalAsync()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var args = new object[] { "string 1" };

        // Act
        LogService.CriticalAsync(message, category, args).GetAwaiter().GetResult();

        // Assert
        logger.Received().CriticalAsync(message, category, args);
    }

    #endregion CriticalAsync

    #region ErrorAsync

    [Fact]
    public void ErrorAsync_Overload1_Always_ExecuteLoggerErrorAsync()
    {
        // Arrange
        const string message = "Message";

        // Act
        LogService.ErrorAsync(message).GetAwaiter().GetResult();

        // Assert
        logger.Received().ErrorAsync(message);
    }

    [Fact]
    public void ErrorAsync_Overload2_Always_ExecuteLoggerErrorAsync()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";

        // Act
        LogService.ErrorAsync(message, category).GetAwaiter().GetResult();

        // Assert
        logger.Received().ErrorAsync(message, category);
    }

    [Fact]
    public void ErrorAsync_Overload3_Always_ExecuteLoggerErrorAsync()
    {
        // Arrange
        const string message = "Message";
        var exception = new Exception();

        // Act
        LogService.ErrorAsync(message, exception).GetAwaiter().GetResult();

        // Assert
        logger.Received().ErrorAsync(message, exception);
    }

    [Fact]
    public void ErrorAsync_Overload4_Always_ExecuteLoggerErrorAsync()
    {
        // Arrange
        const string message = "Message";
        const string category = "category";
        var exception = new Exception();

        // Act
        LogService.ErrorAsync(message, category, exception).GetAwaiter().GetResult();

        // Assert
        logger.Received().ErrorAsync(message, category, exception);
    }

    #endregion ErrorAsync
}