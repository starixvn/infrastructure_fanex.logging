namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class LogData : ILogData
    {
        private const string paramValue = "paramValue";
        internal const string NewSection = "\r\n---------------------------------------------------------------------------------------------------------------\r\n";
        internal static readonly ILogData NoInformationLogData = new LogData("This log has no information.");
        private readonly StringBuilder stringBuilder;

        public LogData() : this(string.Empty)
        {
        }

        public LogData(string message)
        {
            stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(message))
            {
                stringBuilder.Append(message);
            }
        }

        public ILogData AddInfo(string information)
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

        public ILogData AddContent(Func<string> contentExtractor)
        {
            if (contentExtractor == null)
            {
                throw new ArgumentNullException(nameof(contentExtractor));
            }

            stringBuilder.Append(contentExtractor());

            return this;
        }

        public ILogData HideSensitiveInfo(
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