namespace Fanex.Logging.UnitTests.Adapters
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Logging.Adapters;
    using NSubstitute;
    using Xunit;
    using MPEL = Microsoft.Practices.EnterpriseLibrary.Logging;

    [Trait("Unit Tests", "Adapters/LogEngineAdapter.cs")]
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
    }
}