namespace Fanex.Logging.Extensions.Database.V62
{
    using System;
    using System.Data.SqlClient;
    using Data;

    public static class DatabaseLoggingExtensions
    {
        public static ILogEntry AddSqlInfo(this ILogEntry logEntry, Exception ex)
        {
            if (logEntry == null || ex == null)
            {
                return logEntry;
            }

            if (ex is ObjectDbException)
            {
                return logEntry.AddInfo(GetObjectDbExceptionInfo(ex as ObjectDbException));
            }

            if (ex is SqlException)
            {
                return logEntry.AddInfo(GetSqlExceptionInfo(ex as SqlException));
            }

            if (ex.GetBaseException() is SqlException)
            {
                return logEntry.AddInfo(GetSqlExceptionInfo(ex.GetBaseException() as SqlException));
            }

            return logEntry;
        }

        private static string GetSqlExceptionInfo(SqlException ex)
            => LogUtility.CombineInfo(
                "DATABASE INFO",
                $"SQL Server: {ex.Server}",
                $"Stored Procedure: {ex.Procedure}",
                $"Line Number: {ex.LineNumber}");

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
                $"SQL Server: {sqlConnectionString.DataSource}",
                $"Database:  {databaseName}",
                $"Account: {sqlConnectionString.UserID}",
                $"Stored Procedure: {storedProcedureName}",
                $"Line Number: {lineNumber}");
        }
    }
}