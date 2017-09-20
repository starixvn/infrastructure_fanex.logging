namespace Fanex.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LogEntry : ILogEntry
    {
        public const string NewSection = "\r\n---------------------------------------------------------------------------------------------------------------\r\n";
        private readonly StringBuilder stringBuilder;

        public LogEntry(string message)
        {
            stringBuilder = new StringBuilder();
            stringBuilder.Append(message);
        }

        public ILogEntry AddInfo(string information)
        {
            if (string.IsNullOrWhiteSpace(information))
            {
                return this;
            }

            stringBuilder.Append(NewSection);
            stringBuilder.Append(information);

            return this;
        }

        public ILogEntry CensorSensitiveInfo(IEnumerable<string> sensitiveParams, params string[] regexSensitiveParams)
        {
            if (sensitiveParams == null
                || !sensitiveParams.Any()
                || regexSensitiveParams == null
                || !regexSensitiveParams.Any())
            {
                return this;
            }

            var message = stringBuilder.ToString();
            stringBuilder.Clear();

            foreach (var regexSensitiveParam in regexSensitiveParams)
            {
                message = LogUtility.CensorSensitiveInformation(message, regexSensitiveParam, sensitiveParams);
            }

            stringBuilder.Append(message);

            return this;
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }
    }
}