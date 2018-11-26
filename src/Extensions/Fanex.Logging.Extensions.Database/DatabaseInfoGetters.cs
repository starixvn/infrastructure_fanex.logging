[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Database.UnitTests")]

namespace Fanex.Logging.Extensions.Database
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using Fanex.Data;

    public static class DatabaseInfoGetters
    {
        public static IDictionary<string, object> GetDbExceptionInfo(Exception exception)
        {
            var objectDBException = exception.GetInnerException<ObjectDbException>();

            return objectDBException is ObjectDbException
                ? GetObjectDbExceptionInfo(objectDBException as ObjectDbException)
                : CommonInfoGetters.GetDefaultDbExceptionInfo(exception);
        }

        private static IDictionary<string, object> GetObjectDbExceptionInfo(ObjectDbException ex)
        {
            var connString = new SqlConnectionStringBuilder(ex.ConnectionString);

            return new Dictionary<string, object>
            {
                ["Server"] = connString.DataSource,
                [nameof(ex.DbName)] = ex.DbName,
                [nameof(connString.UserID)] = connString.UserID,
                [nameof(ex.SpName)] = ex.SpName,
                ["Parameters"] = GetParameters(ex.Parameters),
                ["Line"] = CommonInfoGetters.GetLineNumber(ex),
                [nameof(ex.CommandTimeout)] = ex.CommandTimeout
            };
        }

        internal static string GetParameters(ICollection parameters)
            => parameters.Count > 0
                ? parameters.Cast<KeyValuePair<string, object>>()
                        .Select(parameter => $"{parameter.Key}={parameter.Value}")
                        .Aggregate((firstPair, secondPair) => $"{firstPair};{secondPair}")
                : string.Empty;
    }
}