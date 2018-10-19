namespace Fanex.Logging.EntLib.UnitTests
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using EntLib;
    using NSubstitute;
    using Xunit;
    using MPEL = Microsoft.Practices.EnterpriseLibrary.Logging;

    [Trait("Unit Tests", "EntLib/EntlibLogging.cs")]
    public class LogEngineAdapterTests
    {
        private readonly EntlibLogging logEngineAdapter;
        private readonly Action<MPEL.LogEntry> writeLog;

        public LogEngineAdapterTests()
        {
            logEngineAdapter = new EntlibLogging(new Configuration.EntLibConfig(string.Empty, string.Empty));
            writeLog = Substitute.For<Action<MPEL.LogEntry>>();

            EntlibLogging.WriteLog = writeLog;
        }

        [Fact]
        public void Write_Always_ExecuteWriteLog()
        {
            // Arrange
            var category = "Category";
            var message = "Message";
            var severity = TraceEventType.Error;

            // Act
            logEngineAdapter.Write(message, severity, category);

            // Assert
            writeLog.Received().Invoke(
                Arg.Is<MPEL.LogEntry>(
                    logEntry => logEntry.Message == message
                                && logEntry.Title == category
                                && logEntry.Severity == severity
                                && logEntry.Categories.Count == 1
                                && logEntry.Categories.ElementAt(0) == category));
        }

        [Fact]
        public void WriteAsync_Always_ExecuteWriteLog()
        {
            // Arrange
            var category = "Category1";
            var message = "Message1";
            var severity = TraceEventType.Information;

            // Act
            logEngineAdapter.WriteAsync(message, severity, category);

            // Assert
            writeLog.Received().Invoke(
                Arg.Is<MPEL.LogEntry>(
                    logEntry => logEntry.Message == message
                                && logEntry.Title == category
                                && logEntry.Severity == severity
                                && logEntry.Categories.Count == 1
                                && logEntry.Categories.ElementAt(0) == category));
        }
    }
}