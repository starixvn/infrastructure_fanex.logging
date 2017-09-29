namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    public static class DefaultLogContentExtractors
    {
        public static string GetExceptionInfo(Exception exception)
         => LogUtility.CombineInfo("EXCEPTION",
                     $"Source: {exception.Source}",
                     $"Type: {exception.GetType()}",
                     $"Target Site: {exception.TargetSite}",
                     $"Stack Trace: {exception.StackTrace}",
                     $"Details: {exception}");

        public static string GetWebServerInfo()
          => LogUtility.CombineInfo(
                      "WEB SERVER INFO",
                      $"Server IP: {GetMachineIP(Dns.GetHostEntry(Dns.GetHostName()))}",
                      $"Machine: {Environment.MachineName}",
                      $"OS: {Environment.OSVersion}",
                      $"System Time: {DateTime.Now}",
                      $"User Domain: {Environment.UserDomainName}/{Environment.UserName}");

        public static List<string> GetServerInfo()
          => new List<string> {                      
                      $"Server IP: {GetMachineIP(Dns.GetHostEntry(Dns.GetHostName()))}",
                      $"Machine: {Environment.MachineName}",
                      $"OS: {Environment.OSVersion}",
                      $"System Time: {DateTime.Now}",
                      $"User Domain: {Environment.UserDomainName}/{Environment.UserName}"};

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
