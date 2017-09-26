namespace Fanex.Logging
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    public static class LogUtility
    {
        public const string QueryStringParamRegexPattern = "(?:{0}=)(?<paramValue>[^&|^\\s]+)";
        public const string BodyDataJsonParamRegexPattern = "(\"{0}\":)(?<paramValue>[^,|^\r]+)";

        public static ILogEntry AddExceptionInfo(this ILogEntry logEntry, Exception exception)
        {
            if (logEntry == null || exception == null)
            {
                return logEntry;
            }

            return logEntry.AddInfo(
                        CombineInfo(
                        "EXCEPTION",
                        $"Source: {exception.Source}",
                        $"Type: {exception.GetType()}",
                        $"Target Site: {exception.TargetSite}",
                        $"Stack Trace: {exception.StackTrace}",
                        $"Details: {exception}"));
        }

        /// <summary>
        /// Combine information with format:
        /// <para>Info group:</para>
        ///     <para>_____Info item 1</para>
        ///     <para>_____Info item 2</para>
        ///     <para>_____Info item n</para>
        /// </summary>
        /// <returns></returns>
        public static string CombineInfo(string infoGroup, params string[] infoItems)
            => string.Join(
                "\r\n",
                $"{infoGroup}:",
                $"\t{JoinWithoutEmptyItem("\r\n\t", infoItems)}");

        public static string JoinWithoutEmptyItem(string separator, params string[] values)
        {
            var removedEmptyItemValues = values.Where(value => !string.IsNullOrWhiteSpace(value));

            return string.Join(separator, removedEmptyItemValues);
        }

        public static ILogEntry AddWebServerInfo(this ILogEntry logEntry)
            => logEntry == null
            ? logEntry
            : logEntry.AddInfo(CombineInfo(
                        "WEB SERVER INFO",
                        $"Server IP: {GetMachineIP()}",
                        $"Machine Name: {Environment.MachineName}",
                        $"OS: {Environment.OSVersion}",
                        $"System Time: {DateTime.Now}",
                        $"User Domain Name: {Environment.UserDomainName}/{Environment.UserName}"));

        private static string GetMachineIP()
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