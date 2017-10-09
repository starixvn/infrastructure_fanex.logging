namespace Fanex.Logging.Extensions.Database.V62.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using Data;
    using NSubstitute;
    using Xunit;

    [Trait("Unit Tests", "Extensions/DB_V62/DatabaseInfoGetters.cs")]
    public class DatabaseLogContentExtractorsTests
    {
        [Fact]
        public void GetDbExceptionInfo_ExceptionIsNull_ReturnNoInfoContent()
        {
            // Act
            var dbExceptionInfo = DatabaseInfoGetters.GetDbExceptionInfo(new Exception());

            // Assert
            Assert.Equal(new Dictionary<string, object>(), dbExceptionInfo);
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
                "Message",
                new Exception("Inner exception"),
                dbSetting);

            // Act
            var dbExceptionInfo = DatabaseInfoGetters.GetDbExceptionInfo(objectDBException);

            // Assert
            Assert.Equal(
                new Dictionary<string, object>
                {
                    ["Server"] = "10.18.200.232",
                    ["DbName"] = "DbName",
                    ["UserID"] = "bodbAdminNet",
                    ["SpName"] = "SPNAME",
                    ["Line"] = 0,
                },
                dbExceptionInfo);
        }

        [Fact]
        public void GetDbExceptionInfo_ExceptionIsSqlException_ReturnDbExceptionInfo()
        {
            // Arrange
            var sqlException = V59.UnitTests.DatabaseLogContentExtractorsTests.CreateSqlException("server", "proccedure");

            // Act
            var dbExceptionInfo = DatabaseInfoGetters.GetDbExceptionInfo(sqlException);

            // Assert
            Assert.Equal(
                new Dictionary<string, object>
                {
                    ["Server"] = "server",
                    ["Procedure"] = "proccedure",
                    ["LineNumber"] = 100,
                    ["ErrorCode"] = -2146232060,
                    ["Number"] = 10
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
            var dbExceptionInfo = DatabaseInfoGetters.GetDbExceptionInfo(exception);

            // Assert
            Assert.Equal(
                new Dictionary<string, object>
                {
                    ["Server"] = "server",
                    ["Procedure"] = "proccedure",
                    ["LineNumber"] = 100,
                    ["ErrorCode"] = -2146232060,
                    ["Number"] = 10
                },
                dbExceptionInfo);
        }
    }
}