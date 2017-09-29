namespace Fanex.Logging
{
    using System;
    using static DefaultLogContentExtractors;

    public static class LogEntryExtensions
    {
        public static ILogEntry AddExceptionInfo(this ILogEntry logEntry, Exception exception)
        {
            if (logEntry == null || exception == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(GetExceptionInfo(exception));
        }

        public static ILogEntry AddServerInfo(this ILogEntry logEntry)
        {
            if (logEntry == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(GetWebServerInfo());
        }
    }
}
