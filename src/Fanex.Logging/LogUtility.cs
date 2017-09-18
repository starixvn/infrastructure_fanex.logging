namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text.RegularExpressions;

    public static class LogUtility
    {
        private const string censoredInformation = "******";
        private const string paramValue = "paramValue";
        public const string NewSection = "\r\n---------------------------------------------------------------------------------------------------------------\r\n";

        public static string BuildLogInfo(
            Exception exception,
            params Func<Exception, string>[] logInfoBuilders)
        {
            if (exception == null
                && (logInfoBuilders == null || !logInfoBuilders.Any()))
            {
                return string.Empty;
            }

            logInfoBuilders = logInfoBuilders.Concat(new List<Func<Exception, string>> {
                ex => BuildWebServerInfo(),
                BuildExceptionInfo
            }).ToArray();

            return JoinWithoutEmptyItem(
                       NewSection,
                       logInfoBuilders.Select(builder => builder(exception)).ToArray());
        }

        public static string BuildExceptionInfo(Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            return CombineInfo(
                "EXCEPTION",
                $"Source: {exception.Source}",
                $"Type: {exception.GetType()}",
                $"Target Site: {exception.TargetSite}",
                $"Stack Trace: {exception.StackTrace}",
                $"Details: {exception}");
        }

        public static string CombineInfo(string infoGroup, params string[] infoItems) 
            => string.Join(
                "\r\n",
                $"{infoGroup}:",
                $"\t{string.Join("\r\n\t", infoItems)}");

        public static string JoinWithoutEmptyItem(string separator, params string[] values)
        {
            var removedEmptyItemValues = values.Where(value => !string.IsNullOrWhiteSpace(value));

            return string.Join(separator, removedEmptyItemValues);
        }

        public static string CensorSensitiveInformation(string information, string paramPattern, IEnumerable<string> sensitiveParams)
        {
            if (sensitiveParams == null
                || !sensitiveParams.Any())
            {
                return information;
            }

            foreach (var sensitiveParam in sensitiveParams)
            {
                var regexMatch = Regex.Match(information, string.Format(paramPattern, sensitiveParam), RegexOptions.IgnoreCase);

                if (regexMatch.Success)
                {
#pragma warning disable S1226 // Method parameters and caught exceptions should not be reassigned
                    information = information.Replace(regexMatch.Groups[paramValue].ToString(), censoredInformation);
#pragma warning restore S1226 // Method parameters and caught exceptions should not be reassigned
                }
            }

            return information;
        }

        public static string BuildWebServerInfo() 
            => CombineInfo(
                        "WEB SERVER INFO",
                        $"Server IP: {GetServerIP()}",
                        $"Machine Name: {Environment.MachineName}",
                        $"OS: {Environment.OSVersion}",
                        $"System Time: {DateTime.Now}",
                        $"User Domain Name: {Environment.UserDomainName}/{Environment.UserName}");

        private static string GetServerIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }
    }
}