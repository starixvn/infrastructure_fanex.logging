namespace Fanex.Logging.Extensions.Database.UnitTests
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Reflection;
    using Data;
    using Data.CrossCutting.WrappingException;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Extensions/DatabaseLoggingExtensions.cs")]
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
            var dbParameter = Substitute.ForPartsOf<DbParameter>();
            dbParameter.ParameterName.Returns("Name");
            dbParameter.Value.Returns("Value");
            var dbParameterCollection = Substitute.ForPartsOf<DbParameterCollection>();
            dbParameterCollection["demo"] = dbParameter;

            var objectDBException = new ObjectDbException(
                new Exception("Inner exception"),
                "Setting Key",
                dbSetting,
                dbParameterCollection,
                600);

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
                        + "\r\n\tLine Number: 0"
                        + "\r\n\tSP Params: ",
                        logEntry.ToString());
        }

        [Fact]
        public void AddSqlInfo_ExceptionIsSqlException_ReturnLogEntryWithDbInfo()
        {
            // Arrange
            var sqlException = V59.UnitTests.DatabaseLoggingExtensionsTests.CreateSqlException("server", "proccedure");

            // Act
            logEntry.AddSqlInfo(sqlException);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "DATABASE INFO:"
                        + "\r\n\tSQL Server: server"
                        + "\r\n\tStored Procedure: proccedure"
                        + "\r\n\tLine Number: 100",
                        logEntry.ToString());
        }

        [Fact]
        public void AddSqlInfo_InnerExceptionIsSqlException_ReturnLogEntryWithDbInfo()
        {
            // Arrange
            var sqlException = V59.UnitTests.DatabaseLoggingExtensionsTests.CreateSqlException("server", "proccedure");
            var exception = new Exception("message", sqlException);

            // Act
            logEntry.AddSqlInfo(exception);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "DATABASE INFO:"
                        + "\r\n\tSQL Server: server"
                        + "\r\n\tStored Procedure: proccedure"
                        + "\r\n\tLine Number: 100",
                        logEntry.ToString());
        }
    }
}