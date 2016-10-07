namespace Fanex.Logging
{
    using System;
    using System.Diagnostics;
    using Configuration;
    using MPEL = Microsoft.Practices.EnterpriseLibrary.Logging;

    public sealed class Logger : ILogger
    {
        private static readonly string DefaultLogCategory = LoggerConfiguration.Setting.DefaultCategory;
        private static volatile ILogger logger;
        private static object syncRoot = new object();

        public Logger()
        {
            LoggerConfiguration.Run();
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

        #region Critical

        public void Critical(string message)
            => WriteLog(message, DefaultLogCategory, LogLevel.Critical);

        public void Critical(string format, params object[] args)
            => WriteLog(string.Format(format, args), DefaultLogCategory, LogLevel.Critical);

        public void Critical(string message, string category)
            => WriteLog(message, category, LogLevel.Critical);

        public void Critical(string format, string category, params object[] args)
            => WriteLog(string.Format(format, args), category, LogLevel.Critical);

        #endregion Critical

        #region Debug

        public void Debug(string message)
            => WriteLog(message, DefaultLogCategory, LogLevel.Debug);

        public void Debug(string format, params object[] args)
            => WriteLog(string.Format(format, args), DefaultLogCategory, LogLevel.Debug);

        public void Debug(string message, string category)
            => WriteLog(message, category, LogLevel.Debug);

        public void Debug(string format, string category, params object[] args)
            => WriteLog(string.Format(format, args), category, LogLevel.Debug);

        #endregion Debug

        #region Error

        public void Error(string message)
            => WriteLog(message, DefaultLogCategory, LogLevel.Error);

        public void Error(string message, Exception exception)
        => WriteLog($"{0}{exception}", DefaultLogCategory, LogLevel.Error);

        public void Error(string message, string category)
            => WriteLog(message, category, LogLevel.Error);

        public void Error(string message, string category, Exception exception)
            => WriteLog($"{0}{exception}", category, LogLevel.Error);

        #endregion Error

        #region Info

        public void Info(string message)
            => WriteLog(message, DefaultLogCategory, LogLevel.Info);

        public void Info(string format, params object[] args)
            => WriteLog(string.Format(format, args), DefaultLogCategory, LogLevel.Info);

        public void Info(string message, string category)
            => WriteLog(message, category, LogLevel.Info);

        public void Info(string format, string category, params object[] args)
            => WriteLog(string.Format(format, args), category, LogLevel.Info);

        #endregion Info

        #region Warning

        public void Warn(string message)
            => WriteLog(message, DefaultLogCategory, LogLevel.Warn);

        public void Warn(string format, params object[] args)
            => WriteLog(string.Format(format, args), DefaultLogCategory, LogLevel.Warn);

        public void Warn(string message, string category)
            => WriteLog(message, category, LogLevel.Warn);

        public void Warn(string format, string category, params object[] args)
            => WriteLog(string.Format(format, args), category, LogLevel.Warn);

        #endregion Warning

        #region Writing Logs

        private static void WriteLog(string message, string category, LogLevel level)
        {
            var severity = TraceEventType.Information;

            switch (level)
            {
                case LogLevel.Debug:
                    severity = TraceEventType.Verbose;
                    break;

                case LogLevel.Warn:
                    severity = TraceEventType.Warning;
                    break;

                case LogLevel.Critical:
                    severity = TraceEventType.Critical;
                    break;

                case LogLevel.Error:
                    severity = TraceEventType.Error;
                    break;
            }

            if (string.IsNullOrEmpty(category))
            {
                category = DefaultLogCategory;
            }

            var logEntry = new MPEL.LogEntry
            {
                Severity = severity,
                Message = message,
                Title = category
            };

            logEntry.Categories.Add(category);
            MPEL.Logger.Write(logEntry);
        }

        #endregion Writing Logs
    }
}