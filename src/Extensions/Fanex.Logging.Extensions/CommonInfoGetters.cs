namespace Fanex.Logging.Extensions;

using System;
using System.Collections.Generic;
#if NET5_0_OR_GREATER
using Microsoft.Data.SqlClient;
#else
    using System.Data.SqlClient;
#endif
using System.IO;
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
                ["Details"] = ex.ToString()
            };

    public static IDictionary<string, object> GetServerInfo()
    {
        var hostname = Dns.GetHostName();

        var hostEntry = Dns.GetHostEntry(hostname);
        var getServerInfo = new Dictionary<string, object>
        {
            ["Server IP"] = GetMachineIp(hostEntry),
            [nameof(Environment.MachineName)] = Environment.MachineName,
            [nameof(Environment.OSVersion)] = Environment.OSVersion,
            ["System Time"] = DateTime.Now,
            ["User Domain"] = $"{Environment.UserDomainName}/{Environment.UserName}"
        };

        return getServerInfo;
    }


    public static Exception GetInnerException<TInnerExceptionType>(this Exception exception)
    {
        while (true)
        {
            if (exception is TInnerExceptionType || exception.InnerException == null)
            {
                return exception;
            }

            if (exception.InnerException is TInnerExceptionType)
            {
                return exception.InnerException;
            }

            exception = exception.InnerException;
        }
    }

    internal static string GetMachineIp(IPHostEntry hostEntry)
    {
        if (hostEntry.AddressList == null)
        {
            return Constants.NotAvailableAbbr;
        }

        foreach (var ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        return Constants.NotAvailableAbbr;
    }

    internal static IDictionary<string, object> GetDefaultDbExceptionInfo(Exception exception)
    {
        if (exception is SqlException sqlException)
        {
            return GetSqlExceptionInfo(sqlException);
        }

        return exception.GetBaseException() is SqlException ? GetSqlExceptionInfo(exception.GetBaseException() as SqlException) : Constants.EmptyDictionary;
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
        => (ex.InnerException as SqlException)?.LineNumber ?? 0;

    internal static string ConvertStreamToString(Stream requestStream)
    {
        if (requestStream == null)
        {
            return Constants.NotAvailableAbbr;
        }

        var bodyData = GetStreamData(requestStream);

        if (requestStream.CanSeek)
        {
            requestStream.Seek(0, SeekOrigin.Begin);
        }

        return !string.IsNullOrWhiteSpace(bodyData) ? Uri.UnescapeDataString(bodyData).Replace(@"\", string.Empty) : Constants.NotAvailableAbbr;
    }

    private static string GetStreamData(Stream requestStream)
    {
        var tempStream = new MemoryStream();

        requestStream.CopyTo(tempStream);

        tempStream.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(tempStream);
        var bodyData = streamReader.ReadToEnd();

        return bodyData;
    }
}