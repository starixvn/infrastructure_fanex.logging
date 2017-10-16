namespace Fanex.Logging.Extensions
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class MessageProtector
    {
        public static string ToCleanMessage(this LogMessage logMessage, List<string> patterns)
            => logMessage.ToString().HideSensitiveInfo(patterns);

        public static string HideSensitiveInfo(this string message, List<string> patterns)
        {
            string cleanMessage = message;

            if (patterns != null)
            {
                foreach (var pattern in patterns)
                {
                    cleanMessage = HideSensitiveInfo(cleanMessage, pattern);
                }
            }

            return cleanMessage;
        }

        public static string HideSensitiveInfo(this string message, string pattern)
        {
            string cleanMessage = message;

            if (!string.IsNullOrWhiteSpace(pattern))
            {
                var matchCollection = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);

                foreach (Match match in matchCollection)
                {
                    if (match.Success)
                    {
                        cleanMessage = cleanMessage.Replace(match.Groups[Constants.ParamValue].ToString(), Constants.SensitiveInfo);
                    }
                }
            }

            return cleanMessage;
        }
    }
}