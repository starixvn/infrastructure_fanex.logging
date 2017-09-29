namespace Fanex.Logging.UnitTests
{
    using System;
    using System.Diagnostics;
    using Logging.Adapters;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Logger.cs")]
    public class LoggerTests
    {
        private readonly Logger logger;
        private readonly ILoggingEngine logEngine;
        private readonly string defaultCategory;

        public LoggerTests()
        {
            logEngine = Substitute.For<ILoggingEngine>();
            defaultCategory = "defaultCategory";
            logger = new Logger(logEngine, defaultCategory, () => { });
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Verbose
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Verbose
                                && logEntry.Category == category));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message Custom Data 1 Custom Data 2"
                                && logEntry.Severity == TraceEventType.Verbose
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message Custom Data 1 Custom Data 2"
                                && logEntry.Severity == TraceEventType.Verbose
                                && logEntry.Category == category));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Information
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Information
                                && logEntry.Category == category));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message Custom Data 1 Custom Data 2"
                                && logEntry.Severity == TraceEventType.Information
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message Custom Data 1 Custom Data 2"
                                && logEntry.Severity == TraceEventType.Information
                                && logEntry.Category == category));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Warning
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Warning
                                && logEntry.Category == category));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message Custom Data 1 Custom Data 2"
                                && logEntry.Severity == TraceEventType.Warning
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message Custom Data 1 Custom Data 2"
                                && logEntry.Severity == TraceEventType.Warning
                                && logEntry.Category == category));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Critical
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Critical
                                && logEntry.Category == category));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message Custom Data 1 Custom Data 2"
                                && logEntry.Severity == TraceEventType.Critical
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message Custom Data 1 Custom Data 2"
                                && logEntry.Severity == TraceEventType.Critical
                                && logEntry.Category == category));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Error
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == message
                                && logEntry.Severity == TraceEventType.Error
                                && logEntry.Category == category));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message\nSystem.NotSupportedException: Specified method is not supported."
                                && logEntry.Severity == TraceEventType.Error
                                && logEntry.Category == defaultCategory));
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
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    logEntry => logEntry.Message == "Message\nSystem.NotSupportedException: Specified method is not supported."
                                && logEntry.Severity == TraceEventType.Error
                                && logEntry.Category == category));
        }

        #endregion Write Error Log

        #region General Write Log

        [Fact]
        public void Write_LogDataIsNull_ExecuteWriteLogEntryWithDefaultMessage()
        {
            // Arrange
            ILogData logEntry = null;

            // Act
            logger.Write(logEntry, TraceEventType.Error);

            // Assert
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    le => le.Message == "This log has no information."
                                && le.Severity == TraceEventType.Error
                                && le.Category == defaultCategory));
        }

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
        public void Write_LogDataWithTraceEventType_ExecuteWriteLogEntry(
            TraceEventType traceEventType,
            TraceEventType expectedTraceEventType)
        {
            // Arrange
            ILogData logEntry = null;

            // Act
            logger.Write(logEntry, traceEventType);

            // Assert
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    le => le.Message == "This log has no information."
                                && le.Severity == expectedTraceEventType
                                && le.Category == defaultCategory));
        }

        [Fact]
        public void Write_LogDataWithSpecificCategory_ExecuteWriteLogEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";
            var logEntry = new LogData(message);

            // Act
            logger.Write(logEntry, TraceEventType.Error, category);

            // Assert
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    le => le.Message == message
                                && le.Severity == TraceEventType.Error
                                && le.Category == category));
        }

        #endregion General Write Log

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Write_CategoryIsNullOrEmpty_ExcuteWriteWithDefaultCategory(string category)
        {
            // Arrange
            var message = "Message";
            var logEntry = new LogData(message);

            // Act
            logger.Write(logEntry, TraceEventType.Error, category);

            // Assert
            logEngine.Received().Write(
                Arg.Is<LogEntryAdapter>(
                    le => le.Message == message
                                && le.Severity == TraceEventType.Error
                                && le.Category == defaultCategory));
        }
    }
}