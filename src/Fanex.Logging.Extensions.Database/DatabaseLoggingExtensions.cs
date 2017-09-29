namespace Fanex.Logging.Extensions.Database
{
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using Data.CrossCutting.WrappingException;

    public static class DatabaseLoggingExtensions
    {
        public static ILogEntry AddSqlInfo(this ILogEntry logEntry, Exception ex)
        {
            if (logEntry == null || ex == null)
            {
                return logEntry;
            }

            var sqlInfo = GetSqlExceptionInfo(ex);

            return string.IsNullOrWhiteSpace(sqlInfo)
                        ? logEntry
                        : logEntry.AddInfo(sqlInfo);
        }

        public static string GetSqlExceptionInfo(Exception exception)
        {
            if (exception is ObjectDbException)
            {
                return GetObjectDbExceptionInfo(exception as ObjectDbException);
            }

            if (exception is SqlException)
            {
                return GetSqlExceptionInfo(exception as SqlException);
            }

            if (exception.GetBaseException() is SqlException)
            {
                return GetSqlExceptionInfo(exception.GetBaseException() as SqlException);
            }

            return string.Empty;
        }

        private static string GetSqlExceptionInfo(SqlException ex) =>
            LogUtility.CombineInfo(
                "DATABASE INFO",
                $"SQL Server: {ex.Server}",
                $"Stored Procedure: {ex.Procedure}",
                $"Line Number: {ex.LineNumber}");

        private static string GetObjectDbExceptionInfo(ObjectDbException ex)
        {
            var sqlConnectionString = new SqlConnectionStringBuilder(ex.DbSetting.ConnectionString);
            var lineNumber = ex.InnerException is SqlException
                ? (ex.InnerException as SqlException).LineNumber
                : 0;

            // Passed Parameters for fanex.data 2+
            var passedParameters = ex.Parameters.Count > 0
                ? ex.Parameters.Cast<DbParameter>()
                        .Select(parameter => $"{parameter.ParameterName}={parameter.Value}")
                        .Aggregate((firstPair, secondPair) => $"{firstPair};{secondPair}")
                : string.Empty;

            return LogUtility.CombineInfo(
                "DATABASE INFO",
                $"SQL Server: {sqlConnectionString.DataSource}",
                $"Database: {ex.DbSetting.DbName}",
                $"Account: {sqlConnectionString.UserID}",
                $"Stored Procedure: {ex.DbSetting.SpName}",
                $"Line Number: {lineNumber}",
                $"SP Params: {passedParameters}");
        }
    }
}