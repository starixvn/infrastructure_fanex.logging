namespace Fanex.Logging.Extensions.Database.V62.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Reflection;
    using Data;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Extensions/DB_V62/DatabaseLogContentExtractors.cs")]
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
        public void GetDbExceptionInfo_ExceptionIsObjectDbException_ReturnDbExceptionInfo()
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
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(objectDBException);

            // Assert
            Assert.Equal(
                new List<string>
                {
                    "SQL Server: 10.18.200.232",
                    "Database: DbName",
                    "Account: bodbAdminNet",
                    "Stored Procedure: SPNAME",
                    "Line Number: 0"
                },
                dbExceptionInfo);
        }

        [Fact]
        public void GetDbExceptionInfo_ExceptionIsSqlException_ReturnDbExceptionInfo()
        {
            // Arrange
            var sqlException = V59.UnitTests.DatabaseLogContentExtractorsTests.CreateSqlException("server 2", "proccedure 2");

            // Act
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(sqlException);

            // Assert
            Assert.Equal(new List<string>
                        {
                            "SQL Server: server 2",
                            "Stored Procedure: proccedure 2",
                            "Line Number: 100",
                        },
                        dbExceptionInfo);
        }

        [Fact]
        public void GetDbExceptionInfo_InnerExceptionIsSqlException_ReturnDbExceptionInfo()
        {
            // Arrange
            var sqlException = V59.UnitTests.DatabaseLogContentExtractorsTests.CreateSqlException("server 2", "proccedure 2");
            var exception = new Exception("message", sqlException);

            // Act
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(sqlException);

            // Assert
            Assert.Equal(new List<string>
                        {
                            "SQL Server: server 2",
                            "Stored Procedure: proccedure 2",
                            "Line Number: 100",
                        },
                        dbExceptionInfo);
        }
    }
}