namespace Fanex.Logging.Extensions.Database.V59
{
    using System;
    using System.Data.SqlClient;

    public static class DatabaseLoggingUtility
    {
        public static ILogEntry AddSqlInfo(this ILogEntry logEntry, Exception ex)
        {
            if (logEntry == null || ex == null)
            {
                return logEntry;
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
    }
}
