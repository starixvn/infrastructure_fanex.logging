namespace Fanex.Logging.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    public static class CommonInfoGetters
    {
        public static IDictionary<string, object> GetExceptionInfo(Exception ex)
           => ex == null
            ? Constants.DefaultDictionary
            : new Dictionary<string, object>
            {
                [nameof(ex.Message)] = ex.Message,
                [nameof(ex.Source)] = ex.Source,
                ["Type"] = ex.GetType(),
                [nameof(ex.TargetSite)] = ex.TargetSite,
                ["Details"] = ex.ToString(),
            };

        public static IDictionary<string, object> GetServerInfo()
           => new Dictionary<string, object>
           {
               ["Server IP"] = GetMachineIP(Dns.GetHostEntry(Dns.GetHostName())),
               [nameof(Environment.MachineName)] = Environment.MachineName,
               [nameof(Environment.OSVersion)] = Environment.OSVersion,
               ["System Time"] = DateTime.Now,
               ["User Domain"] = $"{Environment.UserDomainName}/{Environment.UserName}"
           };

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