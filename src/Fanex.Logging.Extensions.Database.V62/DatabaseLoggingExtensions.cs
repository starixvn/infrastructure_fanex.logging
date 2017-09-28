namespace Fanex.Logging.Extensions.Database.V62
{
    using System;
    using System.Data.SqlClient;
    using Data;

    public static class DatabaseLoggingExtensions
    {
        public static ILogData AddSqlInfo(this ILogData logData, Exception exception)
        {
            if (logData == null || exception == null)
            {
                return logData;
            }

            if (exception is ObjectDbException)
            {
                return logData.AddInfo(GetObjectDbExceptionInfo(exception as ObjectDbException));
            }

            if (exception is SqlException)
            {
                return logData.AddInfo(GetSqlExceptionInfo(exception as SqlException));
            }

            if (exception.GetBaseException() is SqlException)
            {
                return logData.AddInfo(GetSqlExceptionInfo(exception.GetBaseException() as SqlException));
            }

            return logData;
        }

        private static string GetSqlExceptionInfo(SqlException exception)
            => LogUtility.CombineInfo(
                "DATABASE INFO",
                $"SQL Server: {exception.Server}",
                $"Stored Procedure: {exception.Procedure}",
                $"Line Number: {exception.LineNumber}");

        private static string GetObjectDbExceptionInfo(ObjectDbException exception)
        {
            var sqlConnectionString = new SqlConnectionStringBuilder(exception.DbSetting.ConnectionString);
            var storedProcedureName = exception.DbSetting.SpName;
            var databaseName = exception.DbSetting.DbName;
            var lineNumber = exception.InnerException is SqlException
                ? (exception.InnerException as SqlException).LineNumber
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