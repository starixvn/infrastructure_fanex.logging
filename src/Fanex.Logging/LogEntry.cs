namespace Fanex.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LogEntry : ILogEntry
    {
        public const string NewSection = "\r\n---------------------------------------------------------------------------------------------------------------\r\n";
        public static readonly ILogEntry NoInformationLogEntry = new LogEntry("This log has no information.");
        private readonly StringBuilder stringBuilder;

        public LogEntry() : this(string.Empty)
        {
        }

        public LogEntry(string message)
        {
            stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(message))
            {
                stringBuilder.Append(message);
            }
        }

        public ILogEntry AddInfo(string information)
        {
            if (!string.IsNullOrWhiteSpace(information))
            {
                stringBuilder
                   .Append(NewSection)
                   .Append(information);
            }

            return this;
        }

        public ILogEntry CensorSensitiveInfo(IEnumerable<string> sensitiveParams, params string[] regexSensitiveParams)
        {
            if (sensitiveParams != null
                && regexSensitiveParams != null)
            {
                var message = stringBuilder.ToString();
                stringBuilder.Clear();

                foreach (var regexSensitiveParam in regexSensitiveParams)
                {
                    message = LogUtility.CensorSensitiveInformation(message, regexSensitiveParam, sensitiveParams);
                }

                stringBuilder.Append(message);
            }

            return this;
        }

        public override string ToString() 
            => stringBuilder.ToString();
    }
}