namespace Fanex.Logging.UnitTests.Adapters
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Logging.Adapters;
    using NSubstitute;
    using Xunit;
    using MPEL = Microsoft.Practices.EnterpriseLibrary.Logging;

    [Trait("Category", "Adapters/LogEngineAdapter.cs")]
    public class LogEngineAdapterTests
    {
        private readonly EntlibLogging logEngineAdapter;
        private readonly Action<MPEL.LogEntry> writeLog;

        public LogEngineAdapterTests()
        {
            logEngineAdapter = new EntlibLogging();
            writeLog = Substitute.For<Action<MPEL.LogEntry>>();

            EntlibLogging.WriteLog = writeLog;
        }

        [Fact]
        public void Write_Always_ExecuteWriteLog()
        {
            // Arrange
            var logEntryAdapter = new LogEntryAdapter
            {
                Category = "Category",
                Message = "Message",
                Severity = TraceEventType.Error
            };

            // Act
            logEngineAdapter.Write(logEntryAdapter);

            // Assert
            writeLog.Received().Invoke(
                Arg.Is<MPEL.LogEntry>(
                    logEntry => logEntry.Message == logEntryAdapter.Message
                                && logEntry.Title == logEntryAdapter.Category
                                && logEntry.Severity == logEntryAdapter.Severity
                                && logEntry.Categories.ElementAt(0) == logEntryAdapter.Category));
        }
    }
}