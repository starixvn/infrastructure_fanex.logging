namespace Fanex.Logging.EntLib
{
    using Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using System;
    using System.Diagnostics;

    public class EntlibLogging : ILogEngine
    {
        public EntlibLogging(EntLibConfig config) : this(LoggerConfiguration.Run, config)
        { }

        internal EntlibLogging(Action<EntLibConfig> initLogConfig, EntLibConfig config)
        {
            initLogConfig(config);
        }

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