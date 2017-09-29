namespace Fanex.Logging
{
    using System;
    using System.Diagnostics;
    using Adapters;
    using Configuration;

    public sealed class Logger : ILogger
    {
        private static volatile Logger logger;
        private static object syncRoot = new object();

        private readonly string defaultLogCategory;
        internal readonly ILoggingEngineAdapter logAdapter;

        private Logger() 
            : this(null, null, LoggerConfiguration.Run)
        {
        }

        internal Logger(
            ILoggingEngineAdapter logAdapter, 
            string defaultLogCategory,
            Action initLogConfig)
        {
            this.logAdapter = logAdapter ?? new EntlibLoggingEngine();
            this.defaultLogCategory = defaultLogCategory ?? LoggerConfiguration.Setting.DefaultCategory;
            initLogConfig();
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

        #region Debug functions

        public void Debug(string message, string category)
            => WriteLog(message, category, TraceEventType.Verbose);

        public void Debug(string message)
            => Debug(message, defaultLogCategory);

        public void Debug(string format, params object[] args)
            => Debug(string.Format(format, args));

        public void Debug(string format, string category, params object[] args)
            => Debug(string.Format(format, args), category);

        #endregion Debug functions

        #region Info functions

        public void Info(string message, string category)
            => WriteLog(message, category, TraceEventType.Information);

        public void Info(string message)
            => Info(message, defaultLogCategory);

        public void Info(string format, params object[] args)
            => Info(string.Format(format, args));

        public void Info(string format, string category, params object[] args)
            => Info(string.Format(format, args), category);

        #endregion Info functions

        #region Warning functions

        public void Warn(string message, string category)
            => WriteLog(message, category, TraceEventType.Warning);

        public void Warn(string message)
            => Warn(message, defaultLogCategory);

        public void Warn(string format, params object[] args)
            => Warn(string.Format(format, args));

        public void Warn(string format, string category, params object[] args)
            => Warn(string.Format(format, args), category);

        #endregion Warning functions

        #region Error functions

        public void Error(string message, string category)
            => WriteLog(message, category, TraceEventType.Error);

        public void Error(string message)
            => Error(message, defaultLogCategory);

        public void Error(string message, Exception exception)
            => Error(message, defaultLogCategory, exception);

        public void Error(string message, string category, Exception exception)
            => Error(string.Format("{0}\n{1}", message, exception), category);

        #endregion Error functions

        #region Critical

        public void Critical(string message, string category)
            => WriteLog(message, category, TraceEventType.Critical);

        public void Critical(string message)
            => Critical(message, defaultLogCategory);

        public void Critical(string format, params object[] args)
            => Critical(string.Format(format, args));

        public void Critical(string format, string category, params object[] args)
            => Critical(string.Format(format, args), category);

        #endregion Critical

        #region WriteLog functions

        private void WriteLog(string message, string category, TraceEventType traceEventType)
        {
            var logEntry = new LogEntryAdapter
            {
                Severity = GetSevirity(traceEventType),
                Message = message,
                Category = string.IsNullOrEmpty(category) ? defaultLogCategory : category
            };

            logAdapter.Write(logEntry);
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

        public void Write(ILogContentBuilder logData, TraceEventType traceEventType)
            => WriteLog(
                (logData ?? LogContentBuilder.NoInformationLogData).ToString(),
                defaultLogCategory,
                traceEventType);

        public void Write(ILogContentBuilder logData, TraceEventType traceEventType, string category)
             => WriteLog(
                    (logData ?? LogContentBuilder.NoInformationLogData).ToString(),
                    category,
                    traceEventType);


        #endregion WriteLog functions
    }
}