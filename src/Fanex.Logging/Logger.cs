namespace Fanex.Logging
{
    using System;
    using System.Diagnostics;
    using Adapters;
    using Configuration;

    public sealed class Logger : ILogger
    {
        private static readonly string DefaultLogCategory = LoggerConfiguration.Setting.DefaultCategory;
        private static volatile Logger logger;
        private static object syncRoot = new object();
        internal readonly ILogAdapter logAdapter;

        private Logger() : this(null)
        {
            LoggerConfiguration.Run();
        }

        internal Logger(ILogAdapter logAdapter)
        {
            this.logAdapter = logAdapter ?? new LogAdapter();
        }

        public static ILogger Log
        {
            get
            {
                if (logger == null)
                {
                    lock (syncRoot)
                    {
                        if (logger == null)
                        {
                            logger = new Logger();
                        }
                    }
                }

                return logger;
            }
        }

        #region ILogger Members

        #region Debug functions

        public void Debug(string message, string category)
            => WriteLog(message, category, TraceEventType.Verbose);

        public void Debug(string message)
        {
            this.Debug(message, DefaultLogCategory);
        }

        public void Debug(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        public void Debug(string format, string category, params object[] args)
        {
            Debug(string.Format(format, args), category);
        }

        #endregion Debug functions

        #region Info functions

        public void Info(string message, string category)
        {
            WriteLog(message, category, TraceEventType.Information);
        }

        public void Info(string message)
        {
            Info(message, DefaultLogCategory);
        }

        public void Info(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public void Info(string format, string category, params object[] args)
        {
            Info(string.Format(format, args), category);
        }

        #endregion Info functions

        #region Warning functions

        public void Warn(string message, string category)
        {
            WriteLog(message, category, TraceEventType.Warning);
        }

        public void Warn(string message)
        {
            Warn(message, DefaultLogCategory);
        }

        public void Warn(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        public void Warn(string format, string category, params object[] args)
        {
            Warn(string.Format(format, args), category);
        }

        #endregion Warning functions

        #region Error functions

        public void Error(string message, string category)
        {
            WriteLog(message, category, TraceEventType.Error);
        }

        public void Error(string message)
        {
            Error(message, LoggerConfiguration.Setting.DefaultCategory);
        }

        public void Error(string message, Exception exception)
        {
            Error(message, DefaultLogCategory, exception);
        }

        public void Error(string message, string category, Exception exception)
        {
            Error(string.Format("{0}\n{1}", message, exception), category);
        }

        #endregion Error functions

        #region Critical

        public void Critical(string message, string category)
        {
            WriteLog(message, category, TraceEventType.Critical);
        }

        public void Critical(string message)
        {
            Critical(message, DefaultLogCategory);
        }

        public void Critical(string format, params object[] args)
        {
            Critical(string.Format(format, args));
        }

        public void Critical(string format, string category, params object[] args)
        {
            Critical(string.Format(format, args), category);
        }

        #endregion Critical

        #region WriteLog functions

        private static void WriteLog(string message, string category, TraceEventType traceEventType)
        {
            var severity = GetSevirity(traceEventType);

            // Create log entry instance title temporarily set by value category
            var logEntry = new LogEntryAdapter
            {
                Severity = severity,
                Message = message,
                Title = category
            };

            if (string.IsNullOrEmpty(category))
            {
                // Write log with default category
                WriteLog(message, traceEventType);
            }
            else
            {
                logEntry.Category = category;
            }

            logger.logAdapter.Write(logEntry);
        }

        private static TraceEventType GetSevirity(TraceEventType traceEventType)
        {
            switch (traceEventType)
            {
                case TraceEventType.Verbose:
                case TraceEventType.Warning:
                case TraceEventType.Critical:
                case TraceEventType.Error:
                    return traceEventType;

                default:
                    return TraceEventType.Information;
            }
        }

        private static void WriteLog(string message, TraceEventType traceEventType)
            => WriteLog(message, DefaultLogCategory, traceEventType);

        public void Write(ILogEntry logEntry, TraceEventType traceEventType)
            => WriteLog(
                (logEntry ?? LogEntry.NoInformationLogEntry).ToString(),
                traceEventType);

        public void Write(ILogEntry logEntry, TraceEventType traceEventType, string category)
             => WriteLog(
                    (logEntry ?? LogEntry.NoInformationLogEntry).ToString(),
                    category,
                    traceEventType);

        #endregion WriteLog functions

        #endregion ILogger Members
    }
}