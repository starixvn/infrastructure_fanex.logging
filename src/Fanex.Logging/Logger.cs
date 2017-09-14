namespace Fanex.Logging
{
    using System;
    using System.Diagnostics;
    using Fanex.Logging.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

    /// <summary>
    /// The logger class.
    /// </summary>
    public sealed class Logger : ILogger
    {
        private static readonly string DefaultLogCategory = LoggerConfiguration.Setting.DefaultCategory;
        private static volatile ILogger _logger;
        private static object syncRoot = new object();

        private Logger()
        {
            LoggerConfiguration.Run();
        }

        public static ILogger Log
        {
            get
            {
                if (_logger == null)
                {
                    lock (syncRoot)
                    {
                        if (_logger == null)
                        {
                            _logger = new Logger();
                        }
                    }
                }

                return _logger;
            }
        }

        #region ILogger Members

        #region Debug functions

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        public void Debug(string message, string category)
        {
            WriteLog(message, category, LogLevel.Debug);
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            this.Debug(message, DefaultLogCategory);
        }

        /// <summary>
        /// Debugs the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void Debug(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        /// <summary>
        /// Debugs the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="category">The category.</param>
        /// <param name="args">The arguments.</param>
        public void Debug(string format, string category, params object[] args)
        {
            Debug(string.Format(format, args), category);
        }

        #endregion

        #region Info functions

        /// <summary>
        /// Information the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        public void Info(string message, string category)
        {
            WriteLog(message, category, LogLevel.Info);
        }

        /// <summary>
        /// The information message and default log category.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            Info(message, DefaultLogCategory);
        }

        /// <summary>
        /// Information the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void Info(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        public void Info(string format, string category, params object[] args)
        {
            Info(string.Format(format, args), category);
        }

        #endregion

        #region Warning functions

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        public void Warn(string message, string category)
        {
            WriteLog(message, category, LogLevel.Warn);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
            Warn(message, DefaultLogCategory);
        }

        /// <summary>
        /// Warns the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void Warn(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        public void Warn(string format, string category, params object[] args)
        {
            Warn(string.Format(format, args), category);
        }

        #endregion

        #region Error functions

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        public void Error(string message, string category)
        {
            WriteLog(message, category, LogLevel.Error);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            Error(message, LoggerConfiguration.Setting.DefaultCategory);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, Exception exception)
        {
            Error(message, DefaultLogCategory, exception);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, string category, Exception exception)
        {
            Error(string.Format("{0}\n{1}", message, exception), category);
        }

        #endregion

        #region Critical

        public void Critical(string message, string category)
        {
            WriteLog(message, category, LogLevel.Critical);
        }

        /// <summary>
        /// Critical the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Critical(string message)
        {
            Critical(message, DefaultLogCategory);
        }

        /// <summary>
        /// Critical the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void Critical(string format, params object[] args)
        {
            Critical(string.Format(format, args));
        }

        /// <summary>
        /// Critical the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="category">The category.</param>
        /// <param name="args">The arguments.</param>
        public void Critical(string format, string category, params object[] args)
        {
            Critical(string.Format(format, args), category);
        }

        #endregion

        #region WriteLog functions

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="level">The level.</param>
        private static void WriteLog(string message, string category, LogLevel level)
        {
            try
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

                // Create log entry instance title temporarily set by value category
                var logEntry = new LogEntry
                {
                    Severity = severity,
                    Message = message,
                    Title = category
                };

                if (string.IsNullOrEmpty(category))
                {
                    // Write log with default category
                    WriteLog(message, level);
                }
                else
                {
                    logEntry.Categories.Add(category);
                }

                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="level">The level.</param>
        private static void WriteLog(string message, LogLevel level)
        {
            WriteLog(message, DefaultLogCategory, level);
        }

        #endregion

        #endregion
    }
}
