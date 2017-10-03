namespace Fanex.Logging.Extensions.Database.V62
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Data;

    public static class DatabaseInfoGetters
    {
        public static IDictionary<string, object> GetDbExceptionInfo(Exception exception)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();

            if (exception is ObjectDbException)
            {
                data = GetObjectDbExceptionInfo(exception as ObjectDbException);
            }

            if (exception is SqlException)
            {
                data = GetSqlExceptionInfo(exception as SqlException);
            }

            if (exception.GetBaseException() is SqlException)
            {
                data = GetSqlExceptionInfo(exception.GetBaseException() as SqlException);
            }

            return data;
        }

        public static IDictionary<string, object> GetObjectDbExceptionInfo(ObjectDbException ex)
        {
            var connString = new SqlConnectionStringBuilder(ex.DbSetting.ConnectionString);

            return new Dictionary<string, object>
            {
                ["Server"] = connString.DataSource,
                [nameof(ex.DbSetting.DbName)] = ex.DbSetting.DbName,
                [nameof(connString.UserID)] = connString.UserID,
                [nameof(ex.DbSetting.SpName)] = ex.DbSetting.SpName,
                ["Line"] = GetLineNumber(ex)
            };
        }

        public static IDictionary<string, object> GetSqlExceptionInfo(SqlException ex)
             => new Dictionary<string, object>
             {
                 [nameof(ex.Server)] = ex.Server,
                 [nameof(ex.Procedure)] = ex.Procedure,
                 [nameof(ex.LineNumber)] = ex.LineNumber,
                 [nameof(ex.ErrorCode)] = ex.ErrorCode,
                 [nameof(ex.Number)] = ex.Number
             };

        private static int GetLineNumber(ObjectDbException ex)
        {
            return ex.InnerException is SqlException
                            ? (ex.InnerException as SqlException).LineNumber
                            : 0;
        }
    }
}