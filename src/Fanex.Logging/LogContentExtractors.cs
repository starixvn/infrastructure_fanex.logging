namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    public static class LogContentExtractors
    {
        public static List<string> GetExceptionInfo(Exception exception)
         => exception == null 
            ? Constants.NoInformationContent
            : new List<string> {
                $"Source: {exception.Source}",
                     $"Type: {exception.GetType()}",
                     $"Target Site: {exception.TargetSite}",
                     $"Stack Trace: {exception.StackTrace}",
                     $"Details: {exception}" };

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

            return Constants.NotAvailableAbbr;
        }
    }
}
