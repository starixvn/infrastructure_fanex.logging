namespace Fanex.Logging
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using static Constants;

    public static class LogUtility
    {
        public static ILogData AddExceptionInfo(this ILogData logData, Exception exception)
        {
            if (logData == null || exception == null)
            {
                return logData;
            }                      

            return logData.AddInfo(
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
                NewLine,
                $"{infoGroup}:",
                $"{Tab}{JoinWithoutEmptyItem(Segment, infoItems)}");

        public static string JoinWithoutEmptyItem(string separator, params string[] values)
        {
            var removedEmptyItemValues = values.Where(value => !string.IsNullOrWhiteSpace(value));

            return string.Join(separator, removedEmptyItemValues);
        }

        public static ILogData AddWebServerInfo(this ILogData logData)
            => logData == null
            ? logData
            : logData.AddInfo(CombineInfo(
                        "WEB SERVER INFO",
                        $"Server IP: {GetMachineIP(Dns.GetHostEntry(Dns.GetHostName()))}",
                        $"Machine: {Environment.MachineName}",
                        $"OS: {Environment.OSVersion}",
                        $"System Time: {DateTime.Now}",
                        $"User Domain: {Environment.UserDomainName}/{Environment.UserName}"));

        internal static string GetMachineIP(IPHostEntry hostEntry)
        {
            if (hostEntry.AddressList != null)
            {
                foreach (var ip in hostEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }

            return string.Empty;
        }
    }
}