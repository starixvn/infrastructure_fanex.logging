namespace Fanex.Logging.Extensions.Database.V59.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Reflection;
    using Xunit;

    [Trait("Category", "Extensions/DB_V59/DatabaseLogContentExtractors.cs")]
    public class DatabaseLogContentExtractorsTests
    {
        private readonly ILogEntry logEntry;

        public DatabaseLogContentExtractorsTests()
        {
            logEntry = new LogEntry("Message");
        }

        [Fact]
        public void GetDbExceptionInfo_ExceptionIsNull_ReturnNoInfoContent()
        {
            // Act
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(new Exception());

            // Assert
            Assert.Same(Constants.NoInformationContent, dbExceptionInfo);
        }

        [Fact]
        public void GetDbExceptionInfo_ExceptionIsSqlException_ReturnDbExceptionInfo()
        {
            // Arrange
            var sqlException = CreateSqlException("server 1", "proccedure 1");

            // Act
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(sqlException);

            // Assert
            Assert.Equal(
                new List<string>
                {
                    "SQL Server: server 1",
                    "Stored Procedure: proccedure 1",
                    "Line Number: 100",
                },
                dbExceptionInfo);
        }

        [Fact]
        public void GetDbExceptionInfo_InnerExceptionIsSqlException_ReturnDbExceptionInfo()
        {
            // Arrange
            var sqlException = CreateSqlException("server 1", "proccedure 1");
            var exception = new Exception("message", sqlException);

            // Act
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(exception);

            // Assert
            Assert.Equal(
                new List<string>
                {
                    "SQL Server: server 1",
                    "Stored Procedure: proccedure 1",
                    "Line Number: 100",
                },
                dbExceptionInfo);
        }

        public static SqlException CreateSqlException(string server, string procedure)
        {
            var collectionConstructor = typeof(SqlErrorCollection)
              .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, //visibility
              null, //binder
              new Type[0],
              null);
            var addMethod = typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
            var errorCollection = (SqlErrorCollection)collectionConstructor.Invoke(null);
            var errorConstructor = typeof(SqlError).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,
              new[]
              { typeof (int), typeof (byte), typeof (byte), typeof (string), typeof(string), typeof (string), typeof (int), typeof (uint)
              }, null);
            var error =
              errorConstructor.Invoke(new object[] { 10, (byte)0, (byte)0, server, "errMsg 1", procedure, 100, (uint)0 });
            addMethod.Invoke(errorCollection, new[] { error });
            var constructor = typeof(SqlException)
              .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, //visibility
                null, //binder
                new[] { typeof(string), typeof(SqlErrorCollection), typeof(Exception), typeof(Guid) },
                null); //param modifiers

            return (SqlException)constructor.Invoke(new object[] { "Error message", errorCollection, null, Guid.NewGuid() });
        }
    }
}