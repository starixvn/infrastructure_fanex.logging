namespace Fanex.Logging.Extensions.Database.V62
{
    using System;
    using System.Data.SqlClient;
    using Data;

    public static class DatabaseLoggingUtility
    {
        public static string BuildSqlInfo(Exception ex)
        {
            if (ex is ObjectDbException)
            {
                return GetObjectDbExceptionInfo(ex as ObjectDbException);
            }

            if (ex is SqlException)
            {
                return GetSqlExceptionInfo(ex as SqlException);
            }

            if (ex.GetBaseException() is SqlException)
            {
                return GetSqlExceptionInfo(ex.GetBaseException() as SqlException);
            }

            return "NO DATABASE INFO";
        }

        private static string GetSqlExceptionInfo(SqlException ex) 
            => LogUtility.CombineInfo(
                "DATABASE INFO",
                $"Stored Procedure: {ex.Procedure}",
                $"Line Number: {ex.LineNumber}",
                $"SQL Server: {ex.Server}");

        private static string GetObjectDbExceptionInfo(ObjectDbException ex)
        {
            var sqlConnectionString = new SqlConnectionStringBuilder(ex.DbSetting.ConnectionString);
            var storedProcedureName = ex.DbSetting.SpName;
            var databaseName = ex.DbSetting.DbName;
            var lineNumber = ex.InnerException is SqlException
                ? (ex.InnerException as SqlException).LineNumber
                : 0;

            return LogUtility.CombineInfo(
                "DATABASE INFO",
                $"Database Name: {databaseName}",
                $"Account: {sqlConnectionString.UserID}",
                $"Stored Procedure: {storedProcedureName}",
                $"Line Number: {lineNumber}",
                $"SQL Server: {sqlConnectionString.DataSource}");
        }
    }
}