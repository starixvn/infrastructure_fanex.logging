namespace Fanex.Logging.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using NSubstitute;
    using Xunit;

    [Trait("Unit Tests", "LoggerAsync.cs")]
    public class LoggerAsyncTests
    {
        private readonly Logger logger;
        private readonly IEnumerable<ILogEngine> logEngines;
        private readonly ILogEngine logEngine;
        private readonly string defaultCategory;

        public LoggerAsyncTests()
        {
            logEngine = Substitute.For<ILogEngine>();
            logEngines = new List<ILogEngine> { logEngine };
            defaultCategory = "defaultCategory";
            logger = new Logger(logEngines, defaultCategory);
        }

        #region Write Debug Log

        [Fact]
        public void DebugAsync_MessageWithoutCategory_ExecuteWriteLogAsyncEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.DebugAsync(message).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Verbose, defaultCategory);
        }

        [Fact]
        public void DebugAsync_MessageWithSpecificCategory_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.DebugAsync(message, category).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Verbose, category);
        }

        [Fact]
        public void DebugAsync_MessageWithCustomData_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };

            // Act
            logger.DebugAsync(message, args).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message Custom Data 1 Custom Data 2", TraceEventType.Verbose, defaultCategory);
        }

        [Fact]
        public void DebugAsync_MessageWithSpecificCategoryAndCustomData_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };
            var category = "SpecificCategory";

            // Act
            logger.DebugAsync(message, category, args).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message Custom Data 1 Custom Data 2", TraceEventType.Verbose, category);
        }

        #endregion Write Debug Log

        #region Write Information Log

        [Fact]
        public void InfoAsync_MessageWithoutCategory_ExecuteWriteLogAsyncEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.InfoAsync(message).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Information, defaultCategory);
        }

        [Fact]
        public void InfoAsync_MessageWithSpecificCategory_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.InfoAsync(message, category).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Information, category);
        }

        [Fact]
        public void InfoAsync_MessageWithCustomData_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };

            // Act
            logger.InfoAsync(message, args).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message Custom Data 1 Custom Data 2", TraceEventType.Information, defaultCategory);
        }

        [Fact]
        public void InfoAsync_MessageWithSpecificCategoryAndCustomData_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };
            var category = "SpecificCategory";

            // Act
            logger.InfoAsync(message, category, args).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message Custom Data 1 Custom Data 2", TraceEventType.Information, category);
        }

        #endregion Write Information Log

        #region Write Warning Log

        [Fact]
        public void WarnAsync_MessageWithoutCategory_ExecuteWriteLogAsyncEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.WarnAsync(message).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Warning, defaultCategory);
        }

        [Fact]
        public void WarnAsync_MessageWithSpecificCategory_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.WarnAsync(message, category).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Warning, category);
        }

        [Fact]
        public void WarnAsync_MessageWithCustomData_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };

            // Act
            logger.WarnAsync(message, args).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message Custom Data 1 Custom Data 2", TraceEventType.Warning, defaultCategory);
        }

        [Fact]
        public void WarnAsync_MessageWithSpecificCategoryAndCustomData_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };
            var category = "SpecificCategory";

            // Act
            logger.WarnAsync(message, category, args).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message Custom Data 1 Custom Data 2", TraceEventType.Warning, category);
        }

        #endregion Write Warning Log

        #region Write Critical Log

        [Fact]
        public void CriticalAsync_MessageWithoutCategory_ExecuteWriteLogAsyncEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.CriticalAsync(message).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Critical, defaultCategory);
        }

        [Fact]
        public void CriticalAsync_MessageWithSpecificCategory_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.CriticalAsync(message, category).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Critical, category);
        }

        [Fact]
        public void CriticalAsync_MessageWithCustomData_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };

            // Act
            logger.CriticalAsync(message, args).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message Custom Data 1 Custom Data 2", TraceEventType.Critical, defaultCategory);
        }

        [Fact]
        public void CriticalAsync_MessageWithSpecificCategoryAndCustomData_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message {0} {1}";
            var args = new object[] { "Custom Data 1", "Custom Data 2" };
            var category = "SpecificCategory";

            // Act
            logger.CriticalAsync(message, category, args).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message Custom Data 1 Custom Data 2", TraceEventType.Critical, category);
        }

        #endregion Write Critical Log

        #region Write Error Log

        [Fact]
        public void ErrorAsync_MessageWithoutCategory_ExecuteWriteLogAsyncEntryWithDefaultCategory()
        {
            // Arrange
            var message = "Message";

            // Act
            logger.ErrorAsync(message).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Error, defaultCategory);
        }

        [Fact]
        public void ErrorAsync_MessageWithSpecificCategory_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.ErrorAsync(message, category).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Error, category);
        }

        [Fact]
        public void ErrorAsync_MessageWithException_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message";
            var exception = new NotSupportedException();

            // Act
            logger.ErrorAsync(message, exception).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message\nSystem.NotSupportedException: Specified method is not supported.", TraceEventType.Error, defaultCategory);
        }

        [Fact]
        public void ErrorAsync_MessageWithSpecificCategoryAndException_ExecuteWriteLogAsyncEntry()
        {
            // Arrange
            var message = "Message";
            var exception = new NotSupportedException();
            var category = "SpecificCategory";

            // Act
            logger.ErrorAsync(message, category, exception).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync("Message\nSystem.NotSupportedException: Specified method is not supported.", TraceEventType.Error, category);
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
        public void WriteLogAsync_MessageWithTraceEventType_ExecuteWriteLogAsync(
            TraceEventType traceEventType,
            TraceEventType expectedTraceEventType)
        {
            // Arrange
            var message = "Message";

            // Act
            logger.WriteLogAsync(message, null, traceEventType).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, expectedTraceEventType, defaultCategory);
        }

        [Fact]
        public void WriteLogAsync_MessageWithSpecificCategory_ExecuteWriteLogAsync()
        {
            // Arrange
            var message = "Message";
            var category = "SpecificCategory";

            // Act
            logger.WriteLogAsync(message, category, TraceEventType.Error).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Error, category);
        }

        #endregion General Write Log

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WriteLogAsync_CategoryIsNullOrEmpty_ExcuteWriteWithDefaultCategory(string category)
        {
            // Arrange
            var message = "Message";

            // Act
            logger.WriteLogAsync(message, category, TraceEventType.Error).GetAwaiter().GetResult();

            // Assert
            logEngine.Received().WriteAsync(message, TraceEventType.Error, defaultCategory);
        }
    }
}