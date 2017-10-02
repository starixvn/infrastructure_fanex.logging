namespace Fanex.Logging.Extensions.Database.V59
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public static class DatabaseLogContentExtractors
    {
        public static List<string> GetDbExceptionInfo(Exception ex)
        {
            if (ex is SqlException)
            {
                return GetSqlExceptionInfo(ex as SqlException);
            }

            if (ex.GetBaseException() is SqlException)
            {
                return GetSqlExceptionInfo(ex.GetBaseException() as SqlException);
            }

            return Constants.NoInformationContent;
        }

        private static List<string> GetSqlExceptionInfo(SqlException ex)
             => new List<string>
             {
                $"SQL Server: {ex.Server}",
                $"Stored Procedure: {ex.Procedure}",
                $"Line Number: {ex.LineNumber}"
             };
    }
}