namespace Fanex.Logging.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Net;
    using System.Net.Sockets;

    public static class CommonInfoGetters
    {
        public static IDictionary<string, object> GetExceptionInfo(Exception ex)
           => ex == null
            ? Constants.EmptyDictionary
            : new Dictionary<string, object>
            {
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

        internal static IDictionary<string, object> GetDefaulDbExceptionInfo(Exception exception)
        {
            if (exception is SqlException)
            {
                return GetSqlExceptionInfo(exception as SqlException);
            }

            if (exception.GetBaseException() is SqlException)
            {
                return GetSqlExceptionInfo(exception.GetBaseException() as SqlException);
            }

            return new Dictionary<string, object>();
        }

        private static IDictionary<string, object> GetSqlExceptionInfo(SqlException ex)
             => new Dictionary<string, object>
             {
                 [nameof(ex.Server)] = ex.Server,
                 [nameof(ex.Procedure)] = ex.Procedure,
                 [nameof(ex.LineNumber)] = ex.LineNumber,
                 [nameof(ex.ErrorCode)] = ex.ErrorCode,
                 [nameof(ex.Number)] = ex.Number
             };

        internal static int GetLineNumber(Exception ex)
        {
            return ex.InnerException is SqlException
                            ? (ex.InnerException as SqlException).LineNumber
                            : 0;
        }
    }
}