namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class LogEntry : ILogEntry
    {
        private const string paramValue = "paramValue";
        internal static readonly ILogEntry NoInformationLogEntry = new LogEntry("This log has no information.");
        private readonly StringBuilder stringBuilder;

        public LogEntry() : this(string.Empty)
        {
        }

        public LogEntry(string message)
        {
            stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(message))
            {
                stringBuilder.AppendLine(message);
            }
        }

        public ILogEntry AddContent(Func<string> contentExtractor)
        {
            if (contentExtractor == null)
            {
                throw new ArgumentNullException(nameof(contentExtractor));
            }

            string content = contentExtractor();

            if (!string.IsNullOrWhiteSpace(content))
            {
                stringBuilder.AppendLine(content);
            }

            return this;
        }

        public ILogEntry Section(string sectionName)
        {
            if (!string.IsNullOrWhiteSpace(sectionName))
            {
                stringBuilder
                    .AppendLine()
                    .AppendLine(sectionName);
            }

            return this;
        }

        public ILogEntry AddContent(List<string> contents)
        {
            if (contents == null)
            {
                throw new ArgumentNullException(nameof(contents));
            }

            contents.ForEach(content =>
            {
                stringBuilder.AppendLine($"{Constants.Tab}{content}");
            });

            return this;
        }

        public ILogEntry HideSensitiveInfo(
            IEnumerable<string> sensitiveParams,
            params string[] regexSensitiveParams)
        {
            if (sensitiveParams != null && regexSensitiveParams != null)
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

        private static string HideSensitiveInfo(
            string information,
            string paramPattern,
            IEnumerable<string> sensitiveParams)
        {
            if (sensitiveParams != null
                && sensitiveParams.Any())
            {
                foreach (var sensitiveParam in sensitiveParams)
                {
                    information = HideSensitiveInfo(information, paramPattern, sensitiveParam);
                }
            }

            return information;
        }

        private static string HideSensitiveInfo(
            string information,
            string paramPattern,
            string sensitiveParam)
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

            return information;
        }
    }
}