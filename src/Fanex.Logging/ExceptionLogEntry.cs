using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Logging
{
    public class LogEntry<TException> : ILogEntry
    {
        readonly StringBuilder builder = new StringBuilder();
        protected TException exception;

        public LogEntry(TException exception)
        {
            this.exception = exception;
        }

        public ILogEntry CensorSensitiveInfo(IEnumerable<string> sensitiveParams, params string[] regexSensitiveParams)
         => this;

        public override string ToString()
        => builder.ToString();

        public ILogEntry AddInfo<T>(T information)
        {
            throw new NotImplementedException();
        }
    }

    public static class LogEntryFactory
    {
        public static ILogEntry CreateInstance()
            => new LogEntry();

        public static ILogEntry CreateInstance(string message)
            => new LogEntry(message);

        public static ILogEntry CreateInstanceFromException<TException>(TException exception)
            => new LogEntry<TException>(exception);
    }
}
