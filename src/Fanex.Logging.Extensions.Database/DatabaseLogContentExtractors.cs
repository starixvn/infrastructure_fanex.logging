namespace Fanex.Logging.Extensions.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using Data.CrossCutting.WrappingException;

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

        private static List<string> GetObjectDbExceptionInfo(ObjectDbException ex)
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

            return new List<string>
            {
                $"SQL Server: {sqlConnectionString.DataSource}",
                $"Database: {ex.DbSetting.DbName}",
                $"Account: {sqlConnectionString.UserID}",
                $"Stored Procedure: {ex.DbSetting.SpName}",
                $"Line Number: {lineNumber}",
                $"SP Params: {passedParameters}"
            };
        }
    }
}