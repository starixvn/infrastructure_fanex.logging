namespace Fanex.Logging.Extensions.Database.V59
{
    using System;
    using System.Data.SqlClient;

    public static class DatabaseLoggingUtility
    {
        public static string AddSqlInfo(this string message, Exception ex)
        {
            if (string.IsNullOrWhiteSpace(message) || ex == null)
            {
                return message;
            }

            var sqlInfo = string.Empty;

            if (ex is SqlException)
            {
                sqlInfo = GetSqlExceptionInfo(ex as SqlException);
            }

            if (ex.GetBaseException() is SqlException)
            {
                sqlInfo = GetSqlExceptionInfo(ex.GetBaseException() as SqlException);
            }

            return message.AddLogSection(sqlInfo);
        }

        private static string GetSqlExceptionInfo(SqlException ex) 
            => LogUtility.CombineInfo(
                "DATABASE INFO",
                $"Stored Procedure: {ex.Procedure}",
                $"Line Number: {ex.LineNumber}",
                $"SQL Server: {ex.Server}");
    }
}
