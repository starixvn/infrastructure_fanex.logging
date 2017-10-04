namespace Fanex.Logging.Extensions.Database.V59
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public static class DatabaseInfoGetters
    {
        public static IDictionary<string, object> GetDbExceptionInfo(Exception exception)
        {
            if (exception is DatabaseException)
            {
                return GetDatabasexceptionInfo(exception as DatabaseException);
            }

            return Fanex.Logging.Extensions.CommonInfoGetters.GetDefaulDbExceptionInfo(exception);
        }

        private static IDictionary<string, object> GetDatabasexceptionInfo(DatabaseException ex)
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

        private static int GetLineNumber(DatabaseException ex)
        {
            return ex.InnerException is SqlException
                            ? (ex.InnerException as SqlException).LineNumber
                            : 0;
        }
    }
}