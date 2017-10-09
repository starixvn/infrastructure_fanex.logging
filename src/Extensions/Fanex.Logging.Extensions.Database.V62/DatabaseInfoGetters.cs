namespace Fanex.Logging.Extensions.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Data;

    public static class DatabaseInfoGetters
    {
        public static IDictionary<string, object> GetDbExceptionInfo(Exception exception)
             => exception is ObjectDbException
                ? GetObjectDbExceptionInfo(exception as ObjectDbException)
                : CommonInfoGetters.GetDefaultDbExceptionInfo(exception);

        private static IDictionary<string, object> GetObjectDbExceptionInfo(ObjectDbException ex)
        {
            var connString = new SqlConnectionStringBuilder(ex.DbSetting.ConnectionString);

            return new Dictionary<string, object>
            {
                ["Server"] = connString.DataSource,
                ["DbName"] = ex.DbSetting.DbName,
                ["UserID"] = connString.UserID,
                ["SpName"] = ex.DbSetting.SpName,
                ["Line"] = CommonInfoGetters.GetLineNumber(ex)
            };
        }
    }
}