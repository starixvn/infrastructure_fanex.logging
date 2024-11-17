namespace Fanex.Logging.Extensions;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class MessageProtector
{
#pragma warning disable S4226 // Extensions should be in separate namespaces
    public static string ToCleanMessage(this LogMessage logMessage, List<string> patterns)
#pragma warning restore S4226 // Extensions should be in separate namespaces
        => logMessage.ToString().HideSensitiveInfo(patterns);

    public static string HideSensitiveInfo(this string message, List<string> patterns)
    {
        return patterns == null ? message : patterns.Aggregate(message, HideSensitiveInfo);
    }

    public static string HideSensitiveInfo(this string message, string pattern)
    {
        var cleanMessage = message;

        if (string.IsNullOrWhiteSpace(pattern))
        {
            return cleanMessage;
        }

        var matchCollection = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);

        foreach (Match match in matchCollection)
        {
            if (match.Success)
            {
                cleanMessage = cleanMessage.Replace(match.Groups[Constants.ParamValue].ToString(), Constants.SensitiveInfo);
            }
        }

        return cleanMessage;
    }
}