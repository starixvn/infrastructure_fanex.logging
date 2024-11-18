[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Database.UnitTests")]

namespace Fanex.Logging.Extensions.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using Data;

public static class DatabaseInfoGetters
{
    public static IDictionary<string, object> GetDbExceptionInfo(Exception exception)
    {
        var objectDbException = exception.GetInnerException<ObjectDbException>();

        return objectDbException is ObjectDbException dbException
            ? GetObjectDbExceptionInfo(dbException)
            : CommonInfoGetters.GetDefaultDbExceptionInfo(exception);
    }

    private static IDictionary<string, object> GetObjectDbExceptionInfo(ObjectDbException ex)
    {
        var connectionStringBuilder = new ConnectionStringBuilder(ex.ConnectionString);

        return new Dictionary<string, object>
        {
            ["Server"] = connectionStringBuilder.GetServer(),
            [nameof(ex.DbName)] = ex.DbName,
            ["UserId"] = connectionStringBuilder.GetUserId(),
            [nameof(ex.SpName)] = ex.SpName,
            ["Parameters"] = GetParameters(ex.Parameters),
            ["Line"] = CommonInfoGetters.GetLineNumber(ex),
            [nameof(ex.CommandTimeout)] = ex.CommandTimeout
        };
    }

    internal static string GetParameters(IDictionary<string, object> parameters)
        => parameters.Count > 0
            ? parameters
                .Select(parameter => $"{parameter.Key}={parameter.Value}")
                .Aggregate((firstPair, secondPair) => $"{firstPair};{secondPair}")
            : string.Empty;
}