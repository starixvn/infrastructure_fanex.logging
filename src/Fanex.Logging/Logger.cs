namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public sealed partial class Logger : ILogger
    {
        private static volatile Logger logger;
        private static object syncRoot = new object();

        private readonly string defaultLogCategory;
        internal readonly IEnumerable<ILogEngine> logEngines;

        public Logger()
            : this(null, null)
        {
        }

        public Logger(
            IEnumerable<ILogEngine> logEngines,
            string defaultLogCategory)
        {
            this.logEngines = logEngines ?? LogManager.Instance.LogEngines;
            this.defaultLogCategory = defaultLogCategory ?? LogManager.Instance.DefaultLogCategory;
        }

        public static ILogger Log
        {
            get
            {
                if (logger == null)
                {
                    lock (syncRoot)
                    {
                        logger = logger?? new Logger();
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

        public void WriteLog(string message, string category, TraceEventType traceEventType)
        {
            foreach (var logEngine in logEngines)
            {
                logEngine.Write(
                            message,
                            GetSeverity(traceEventType),
                            string.IsNullOrEmpty(category)
                                ? defaultLogCategory
                                : category);
            }
        }

        private static TraceEventType GetSeverity(TraceEventType traceEventType)
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

        #endregion WriteLog functions
    }
}