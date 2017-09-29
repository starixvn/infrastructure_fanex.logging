namespace Fanex.Logging.Extensions.Database.V59.UnitTests
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    using Xunit;

    [Trait("Category", "Extensions/DB_V59/DatabaseLoggingExtensions.cs")]
    public class DatabaseLoggingExtensionsTests
    {
        private readonly ILogEntry logEntry;

        public DatabaseLoggingExtensionsTests()
        {
            logEntry = new LogEntry("Message");

        }

        [Fact]
        public void AddSqlInfo_LogEntryIsNull_ReturnNull()
        {
            // Arrange
            ILogEntry nullLogEntry = null;

            // Act
            nullLogEntry = nullLogEntry.AddSqlInfo(new Exception());

            // Assert
            Assert.Null(nullLogEntry);
        }

        [Fact]
        public void AddSqlInfo_ExceptionIsNull_ReturnLogEntry()
        {
            // Act
            var actualLogEntry = logEntry.AddSqlInfo(new Exception());

            // Assert
            Assert.Same(logEntry, actualLogEntry);
        }

        [Fact]
        public void AddSqlInfo_ExceptionIsSqlException_ReturnLogEntryWithDbInfo()
        {
            // Arrange
            var sqlException = CreateSqlException("server 1", "proccedure 1");

            // Act
            logEntry.AddSqlInfo(sqlException);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "DATABASE INFO:"
                        + "\r\n\tSQL Server: server 1"
                        + "\r\n\tStored Procedure: proccedure 1"
                        + "\r\n\tLine Number: 100",
                        logEntry.ToString());
        }

        [Fact]
        public void AddSqlInfo_InnerExceptionIsSqlException_ReturnLogEntryWithDbInfo()
        {
            // Arrange
            var sqlException = CreateSqlException("server 1", "proccedure 1");
            var exception = new Exception("message", sqlException);

            // Act
            logEntry.AddSqlInfo(exception);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "DATABASE INFO:"
                        + "\r\n\tSQL Server: server 1"
                        + "\r\n\tStored Procedure: proccedure 1"
                        + "\r\n\tLine Number: 100",
                        logEntry.ToString());
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