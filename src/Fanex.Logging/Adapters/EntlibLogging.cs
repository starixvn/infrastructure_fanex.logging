namespace Fanex.Logging.Adapters
{
    using System;
    using System.Diagnostics;
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

        public void Write(string message, TraceEventType severity, string category)
        {
            var logEntry = new LogEntry
            {
                Message = message,
                Title = category,
                Severity = severity
            };

            logEntry.Categories.Add(category);

            WriteLog(logEntry);
        }
    }
}