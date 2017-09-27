namespace Fanex.Logging
{
    using System;
    using System.Diagnostics;

    public static class LogService
    {
        [ThreadStatic]
        private static ILogger loggerInstance;

        internal static ILogger LoggerInstance
        {
            get
            {
                return loggerInstance ?? Logger.Log;
            }

            set
            {
                loggerInstance = value;
            }
        }

        #region Debug

        public static void Debug(string message)
        {
            LoggerInstance.Debug(message);
        }

        public static void Debug(string message, string category)
        {
            LoggerInstance.Debug(message, category);
        }

        public static void Debug(string format, params object[] args)
        {
            LoggerInstance.Debug(format, args);
        }

        public static void Debug(string format, string category, params object[] args)
        {
            LoggerInstance.Debug(format, category, args);
        }

        #endregion

        #region Info

        public static void Info(string message)
        {
            LoggerInstance.Info(message);
        }

        public static void Info(string message, string category)
        {
            LoggerInstance.Info(message, category);
        }

        public static void Info(string format, params object[] args)
        {
            LoggerInstance.Info(format, args);
        }

        public static void Info(string format, string category, params object[] args)
        {
            LoggerInstance.Info(format, category, args);
        }

        #endregion

        #region Warn

        public static void Warn(string message)
        {
            LoggerInstance.Warn(message);
        }

        public static void Warn(string message, string category)
        {
            LoggerInstance.Warn(message, category);
        }

        public static void Warn(string format, params object[] args)
        {
            LoggerInstance.Warn(format, args);
        }

        public static void Warn(string format, string category, params object[] args)
        {
            LoggerInstance.Warn(format, category, args);
        }

        #endregion

        #region Error

        public static void Error(string message)
        {
            LoggerInstance.Error(message);
        }

        public static void Error(string message, string category)
        {
            LoggerInstance.Error(message, category);
        }

        public static void Error(string message, Exception exception)
        {
            LoggerInstance.Error(message, exception);
        }

        public static void Error(string message, string category, Exception exception)
        {
            LoggerInstance.Error(message, category, exception);
        }

        #endregion

        #region Critical

        public static void Critical(string message)
        {
            LoggerInstance.Critical(message);
        }

        public static void Critical(string message, string category)
        {
            LoggerInstance.Critical(message, category);
        }

        public static void Critical(string format, params object[] args)
        {
            LoggerInstance.Critical(format, args);
        }

        public static void Critical(string format, string category, params object[] args)
        {
            LoggerInstance.Critical(format, category, args);
        }

        #endregion

        public static void Write(ILogEntry logEntry, TraceEventType traceEventType)
        {
            LoggerInstance.Write(logEntry, traceEventType);
        }

        public static void Write(ILogEntry logEntry, TraceEventType traceEventType, string category)
        {
            LoggerInstance.Write(logEntry, traceEventType, category);
        }
    }
}
