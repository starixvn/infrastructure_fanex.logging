namespace Fanex.Logging.Extensions.Database.V59
{
    using System;
    using System.Data.SqlClient;

    public static class DatabaseLoggingUtility
    {
        public static string BuildSqlInfo(Exception ex)
        {
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
    }
}
