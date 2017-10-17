namespace Fanex.Logging.Extensions.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public static class DatabaseInfoGetters
    {
        public static IDictionary<string, object> GetDbExceptionInfo(Exception exception)
        {
            var databaseException = exception.GetInnerException<DatabaseException>();

            return databaseException is DatabaseException
                ? GetDatabasExceptionInfo(databaseException as DatabaseException)
                : CommonInfoGetters.GetDefaultDbExceptionInfo(exception);
        }

        private static IDictionary<string, object> GetDatabasExceptionInfo(DatabaseException ex)
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
            => ex.InnerException is SqlException
                ? (ex.InnerException as SqlException).LineNumber
                : 0;
    }
}