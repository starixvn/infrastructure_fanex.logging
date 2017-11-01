namespace Fanex.Logging.UnitTests
{
    using System;
    using System.Diagnostics;
    using NSubstitute;
    using Xunit;

    [Trait("Unit Tests", "Logger.cs")]
    public class LoggerTests
    {
        private readonly Logger logger;
        private readonly ILogEngine logEngine;
        private readonly string defaultCategory;

        public LoggerTests()
        {
            logEngine = Substitute.For<ILogEngine>();
            defaultCategory = "defaultCategory";
            logger = new Logger(logEngine, defaultCategory);
        }

        #region Write Debug Log

        [Fact]
        public void Debug_MessageWithoutCategory_ExecuteWriteLogEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.Debug(message);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Verbose, defaultCategory);
        }

        [Fact]
        public void Debug_MessageWithSpecificCategory_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.Debug(message, category);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Verbose, category);
        }

        [Fact]
        public void Debug_MessageWithCustomData_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };

            // Act
            logger.Debug(message, args);

            // Assert
            logEngine.Received().Write("Message Custom Data 1 Custom Data 2", TraceEventType.Verbose, defaultCategory);
        }

        [Fact]
        public void Debug_MessageWithSpecificCategoryAndCustomData_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };
            var category = "SpecificCategory";

            // Act
            logger.Debug(message, category, args);

            // Assert
            logEngine.Received().Write("Message Custom Data 1 Custom Data 2", TraceEventType.Verbose, category);
        }

        #endregion Write Debug Log

        #region Write Information Log

        [Fact]
        public void Info_MessageWithoutCategory_ExecuteWriteLogEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.Info(message);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Information, defaultCategory);
        }

        [Fact]
        public void Info_MessageWithSpecificCategory_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.Info(message, category);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Information, category);
        }

        [Fact]
        public void Info_MessageWithCustomData_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };

            // Act
            logger.Info(message, args);

            // Assert
            logEngine.Received().Write("Message Custom Data 1 Custom Data 2", TraceEventType.Information, defaultCategory);
        }

        [Fact]
        public void Info_MessageWithSpecificCategoryAndCustomData_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };
            var category = "SpecificCategory";

            // Act
            logger.Info(message, category, args);

            // Assert
            logEngine.Received().Write("Message Custom Data 1 Custom Data 2", TraceEventType.Information, category);
        }

        #endregion Write Information Log

        #region Write Warning Log

        [Fact]
        public void Warn_MessageWithoutCategory_ExecuteWriteLogEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.Warn(message);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Warning, defaultCategory);
        }

        [Fact]
        public void Warn_MessageWithSpecificCategory_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.Warn(message, category);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Warning, category);
        }

        [Fact]
        public void Warn_MessageWithCustomData_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };

            // Act
            logger.Warn(message, args);

            // Assert
            logEngine.Received().Write("Message Custom Data 1 Custom Data 2", TraceEventType.Warning, defaultCategory);
        }

        [Fact]
        public void Warn_MessageWithSpecificCategoryAndCustomData_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };
            var category = "SpecificCategory";

            // Act
            logger.Warn(message, category, args);

            // Assert
            logEngine.Received().Write("Message Custom Data 1 Custom Data 2", TraceEventType.Warning, category);
        }

        #endregion Write Warning Log

        #region Write Critical Log

        [Fact]
        public void Critical_MessageWithoutCategory_ExecuteWriteLogEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.Critical(message);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Critical, defaultCategory);
        }

        [Fact]
        public void Critical_MessageWithSpecificCategory_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.Critical(message, category);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Critical, category);
        }

        [Fact]
        public void Critical_MessageWithCustomData_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };

            // Act
            logger.Critical(message, args);

            // Assert
            logEngine.Received().Write("Message Custom Data 1 Custom Data 2", TraceEventType.Critical, defaultCategory);
        }

        [Fact]
        public void Critical_MessageWithSpecificCategoryAndCustomData_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };
            var category = "SpecificCategory";

            // Act
            logger.Critical(message, category, args);

            // Assert
            logEngine.Received().Write("Message Custom Data 1 Custom Data 2", TraceEventType.Critical, category);
        }

        #endregion Write Critical Log

        #region Write Error Log

        [Fact]
        public void Error_MessageWithoutCategory_ExecuteWriteLogEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.Error(message);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Error, defaultCategory);
        }

        [Fact]
        public void Error_MessageWithSpecificCategory_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.Error(message, category);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Error, category);
        }

        [Fact]
        public void Error_MessageWithException_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message";
            var exception = new NotSupportedException();

            // Act
            logger.Error(message, exception);

            // Assert
            logEngine.Received().Write("Message\nSystem.NotSupportedException: Specified method is not supported.", TraceEventType.Error, defaultCategory);
        }

        [Fact]
        public void Error_MessageWithSpecificCategoryAndException_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message";
            var exception = new NotSupportedException();
            var category = "SpecificCategory";

            // Act
            logger.Error(message, category, exception);

            // Assert
            logEngine.Received().Write("Message\nSystem.NotSupportedException: Specified method is not supported.", TraceEventType.Error, category);
        }

        #endregion Write Error Log

        #region General Write Log

        [Theory]
        [InlineData(TraceEventType.Error, TraceEventType.Error)]
        [InlineData(TraceEventType.Warning, TraceEventType.Warning)]
        [InlineData(TraceEventType.Critical, TraceEventType.Critical)]
        [InlineData(TraceEventType.Information, TraceEventType.Information)]
        [InlineData(TraceEventType.Verbose, TraceEventType.Verbose)]
        [InlineData(TraceEventType.Start, TraceEventType.Information)]
        [InlineData(TraceEventType.Stop, TraceEventType.Information)]
        [InlineData(TraceEventType.Suspend, TraceEventType.Information)]
        [InlineData(TraceEventType.Resume, TraceEventType.Information)]
        [InlineData(TraceEventType.Transfer, TraceEventType.Information)]
        public void WriteLog_MessageWithTraceEventType_ExecuteWriteLog(
            TraceEventType traceEventType,
            TraceEventType expectedTraceEventType)
        {
            // Arrange
            var message = "Message";

            // Act
            logger.WriteLog(message, null, traceEventType);

            // Assert
            logEngine.Received().Write(message, expectedTraceEventType, defaultCategory);
        }

        [Fact]
        public void WriteLog_MessageWithSpecificCategory_ExecuteWriteLog()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.WriteLog(message, category, TraceEventType.Error);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Error, category);
        }

        #endregion General Write Log

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WriteLog_CategoryIsNullOrEmpty_ExcuteWriteWithDefaultCategory(string category)
        {
            // Arrange
            var message = "Message";

            // Act
            logger.WriteLog(message, category, TraceEventType.Error);

            // Assert
            logEngine.Received().Write(message, TraceEventType.Error, defaultCategory);
        }
    }
}