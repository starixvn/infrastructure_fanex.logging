namespace Fanex.Logging.Extensions.Database.V62.UnitTests
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Reflection;
    using Data;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Extensions/DB_V62/DatabaseLoggingExtensions.cs")]
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
        public void AddSqlInfo_ExceptionIsObjectDbException_ReturnLogEntryWithDbInfo()
        {
            // Arrnage
            var dbSetting = Substitute.For<IDbSetting>();
            dbSetting.ConnectionString.Returns("packet size=4096; Data Source=10.18.200.232;Initial Catalog=bodb_OER;integrated security=false; User Id=bodbAdminNet;Password=23232323;");
            dbSetting.SpName.Returns("SPNAME");
            dbSetting.DbName.Returns("DbName");

            var objectDBException = new ObjectDbException(
                "ErrorMessage",
                new Exception("Inner exception"),
                dbSetting);

            // Act
            logEntry.AddSqlInfo(objectDBException);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "DATABASE INFO:"
                        + "\r\n\tSQL Server: 10.18.200.232"
                        + "\r\n\tDatabase: DbName"
                        + "\r\n\tAccount: bodbAdminNet"
                        + "\r\n\tStored Procedure: SPNAME"
                        + "\r\n\tLine Number: 0",
                        logEntry.ToString());
        }

        [Fact]
        public void AddSqlInfo_ExceptionIsSqlException_ReturnLogEntryWithDbInfo()
        {
            // Arrange
            var sqlException = V59.UnitTests.DatabaseLoggingExtensionsTests.CreateSqlException("server 2", "proccedure 2");

            // Act
            logEntry.AddSqlInfo(sqlException);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "DATABASE INFO:"
                        + "\r\n\tSQL Server: server 2"
                        + "\r\n\tStored Procedure: proccedure 2"
                        + "\r\n\tLine Number: 100",
                        logEntry.ToString());
        }

        [Fact]
        public void AddSqlInfo_InnerExceptionIsSqlException_ReturnLogEntryWithDbInfo()
        {
            // Arrange
            var sqlException = V59.UnitTests.DatabaseLoggingExtensionsTests.CreateSqlException("server 2", "proccedure 2");
            var exception = new Exception("message", sqlException);

            // Act
            logEntry.AddSqlInfo(exception);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "DATABASE INFO:"
                        + "\r\n\tSQL Server: server 2"
                        + "\r\n\tStored Procedure: proccedure 2"
                        + "\r\n\tLine Number: 100",
                        logEntry.ToString());
        }
    }
}