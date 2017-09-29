
namespace Fanex.Logging
{
    using System;
    using System.Net;

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
                      $"Server IP: {LogUtility.GetMachineIP(Dns.GetHostEntry(Dns.GetHostName()))}",
                      $"Machine: {Environment.MachineName}",
                      $"OS: {Environment.OSVersion}",
                      $"System Time: {DateTime.Now}",
                      $"User Domain: {Environment.UserDomainName}/{Environment.UserName}");
    }
}
