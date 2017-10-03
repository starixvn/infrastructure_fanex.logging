namespace Fanex.Logging.Extensions.Database.V62
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Data;

    public static class DatabaseLogContentExtractors
    {
        private static readonly List<string> noInformationContent = new List<string> { "No Information is available." };

        public static List<string> GetDbExceptionInfo(Exception exception)
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

            return noInformationContent;
        }

        private static List<string> GetSqlExceptionInfo(SqlException ex)
              => new List<string>
              {
                $"SQL Server: {ex.Server}",
                $"Stored Procedure: {ex.Procedure}",
                $"Line Number: {ex.LineNumber}"
              };

        private static List<string> GetObjectDbExceptionInfo(ObjectDbException exception)
        {
            var sqlConnectionString = new SqlConnectionStringBuilder(exception.DbSetting.ConnectionString);
            var storedProcedureName = exception.DbSetting.SpName;
            var databaseName = exception.DbSetting.DbName;
            var lineNumber = exception.InnerException is SqlException
                ? (exception.InnerException as SqlException).LineNumber
                : 0;

            return new List<string>
            {
                $"SQL Server: {sqlConnectionString.DataSource}",
                $"Database: {databaseName}",
                $"Account: {sqlConnectionString.UserID}",
                $"Stored Procedure: {storedProcedureName}",
                $"Line Number: {lineNumber}"
            };
        }
    }
}