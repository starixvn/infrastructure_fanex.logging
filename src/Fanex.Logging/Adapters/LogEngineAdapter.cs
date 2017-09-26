namespace Fanex.Logging.Adapters
{
    using System;
    using MPEL = Microsoft.Practices.EnterpriseLibrary.Logging;

    internal interface ILogEngineAdapter
    {
        void Write(LogEntryAdapter logEntryAdapter);
    }

    internal class LogEngineAdapter : ILogEngineAdapter
    {
        [ThreadStatic]
        private static Action<MPEL.LogEntry> writeLog;

        internal static Action<MPEL.LogEntry> WriteLog
        {
            get
            {
                return writeLog ?? MPEL.Logger.Write;
            }

            set
            {
                writeLog = value;
            }
        }

        public void Write(LogEntryAdapter logEntryAdapter)
        {
            var logEntry = new MPEL.LogEntry
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