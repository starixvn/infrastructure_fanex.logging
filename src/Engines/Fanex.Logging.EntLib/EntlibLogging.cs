namespace Fanex.Logging.EntLib
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

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

        public Task WriteAsync(string message, TraceEventType severity, string category)
        {
            Write(message, severity, category);

            return Task.FromResult(0);
        }

        public string RenderJavascriptLogger()
            => string.Empty;
    }
}