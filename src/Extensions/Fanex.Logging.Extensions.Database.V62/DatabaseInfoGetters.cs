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
            if (exception is ObjectDbException)
            {
                return GetObjectDbExceptionInfo(exception as ObjectDbException);
            }

            return CommonInfoGetters.GetDefaulDbExceptionInfo(exception);
        }

        private static IDictionary<string, object> GetObjectDbExceptionInfo(ObjectDbException ex)
        {
            var connString = new SqlConnectionStringBuilder(ex.DbSetting.ConnectionString);

            return new Dictionary<string, object>
            {
                ["Server"] = connString.DataSource,
                [nameof(ex.DbSetting.DbName)] = ex.DbSetting.DbName,
                [nameof(connString.UserID)] = connString.UserID,
                [nameof(ex.DbSetting.SpName)] = ex.DbSetting.SpName,
                ["Line"] = CommonInfoGetters.GetLineNumber(ex)
            };
        }
    }
}