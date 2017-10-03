namespace Fanex.Logging.Extensions.Database.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Data;
    using Data.CrossCutting.WrappingException;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Extensions/DatabaseLogContentExtractors.cs")]
    public class DatabaseLogContentExtractorsTests
    {
        [Fact]
        public void GetDbExceptionInfo_ExceptionIsNull_ReturnNoInfoContent()
        {
            // Act
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(new Exception());

            // Assert
            Assert.Equal(new List<string> { "No Information is available." }, dbExceptionInfo);
        }

        [Fact]
        public void GetDbExceptionInfo_ExceptionIsObjectDbException_ReturnDbExceptionInfo()
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
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(objectDBException);

            // Assert
            Assert.Equal(
                new List<string>
                {
                    "SQL Server: 10.18.200.232",
                    "Database: DbName",
                    "Account: bodbAdminNet",
                    "Stored Procedure: SPNAME",
                    "Line Number: 0",
                    "SP Params: "
                },
                dbExceptionInfo);
        }

        [Fact]
        public void GetDbExceptionInfo_ExceptionIsSqlException_ReturnDbExceptionInfo()
        {
            // Arrange
            var sqlException = V59.UnitTests.DatabaseLogContentExtractorsTests.CreateSqlException("server", "proccedure");

            // Act
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(sqlException);

            // Assert
            Assert.Equal(new List<string>
                        {
                            "SQL Server: server",
                            "Stored Procedure: proccedure",
                            "Line Number: 100",
                        },
                        dbExceptionInfo);
        }

        [Fact]
        public void GetDbExceptionInfo_InnerExceptionIsSqlException_ReturnDbExceptionInfo()
        {
            // Arrange
            var sqlException = V59.UnitTests.DatabaseLogContentExtractorsTests.CreateSqlException("server", "proccedure");
            var exception = new Exception("message", sqlException);

            // Act
            var dbExceptionInfo = DatabaseLogContentExtractors.GetDbExceptionInfo(exception);

            // Assert
            Assert.Equal(new List<string>
                        {
                            "SQL Server: server",
                            "Stored Procedure: proccedure",
                            "Line Number: 100",
                        },
                        dbExceptionInfo);
        }
    }
}