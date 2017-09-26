namespace Fanex.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class LogEntry : ILogEntry
    {
        private const string paramValue = "paramValue";
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
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(NewSection);
                }

                stringBuilder.Append(information);
            }

            return this;
        }

        public ILogEntry HideSensitiveInfo(
            IEnumerable<string> sensitiveParams,
            params string[] regexSensitiveParams)
        {
            if (sensitiveParams != null
                && regexSensitiveParams != null)
            {
                var message = stringBuilder.ToString();
                stringBuilder.Clear();

                foreach (var regexSensitiveParam in regexSensitiveParams)
                {
                    message = HideSensitiveInfo(message, regexSensitiveParam, sensitiveParams);
                }

                stringBuilder.Append(message);
            }

            return this;
        }

        public override string ToString() => stringBuilder.ToString();

        private static string HideSensitiveInfo(string information, string paramPattern, IEnumerable<string> sensitiveParams)
        {
            if (sensitiveParams == null
                || !sensitiveParams.Any())
            {
                return information;
            }

            foreach (var sensitiveParam in sensitiveParams)
            {
                var pattern = string.Format(paramPattern, sensitiveParam);
                var regexMatchCollection = Regex.Matches(information, pattern, RegexOptions.IgnoreCase);

                foreach (Match regexMatch in regexMatchCollection)
                {
                    if (regexMatch.Success)
                    {
                        information = information.Replace(regexMatch.Groups[paramValue].ToString(), string.Empty);
                    }
                }
            }

            return information;
        }

    }
}