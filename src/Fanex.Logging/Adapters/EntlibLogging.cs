namespace Fanex.Logging.Adapters
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    internal class EntlibLogging : ILoggingEngine
    {
        [ThreadStatic]
        private static Action<LogEntry> writeLog;

        internal static Action<LogEntry> WriteLog
        {
            get
            {
                return writeLog ?? Logger.Write;
            }

            set
            {
                writeLog = value;
            }
        }

        public void Write(LogEntryAdapter logEntryAdapter)
        {
            var logEntry = new LogEntry
            {
                Message = logEntryAdapter.Message,
                Title = logEntryAdapter.Category,
                Severity = logEntryAdapter.Severity
            };

            logEntry.Categories.Add(logEntryAdapter.Category);

            WriteLog(logEntry);
        }
    }
}