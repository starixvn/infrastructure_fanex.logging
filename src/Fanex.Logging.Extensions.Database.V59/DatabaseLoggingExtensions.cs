namespace Fanex.Logging.Extensions.Database.V59
{
    using System;
    using System.Data.SqlClient;

    public static class DatabaseLoggingExtensions
    {
        public static ILogData AddSqlInfo(this ILogData logData, Exception ex)
        {
            if (logData == null || ex == null)
            {
                return logData;
            }

            if (ex is SqlException)
            {
                return logData.AddInfo(GetSqlExceptionInfo(ex as SqlException));
            }

            if (ex.GetBaseException() is SqlException)
            {
                return logData.AddInfo(GetSqlExceptionInfo(ex.GetBaseException() as SqlException));
            }

            return logData;
        }

        private static string GetSqlExceptionInfo(SqlException ex)
            => LogUtility.CombineInfo(
                "DATABASE INFO",
                $"SQL Server: {ex.Server}",
                $"Stored Procedure: {ex.Procedure}",
                $"Line Number: {ex.LineNumber}");
    }
}