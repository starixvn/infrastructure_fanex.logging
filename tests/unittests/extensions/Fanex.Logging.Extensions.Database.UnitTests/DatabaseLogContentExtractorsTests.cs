namespace Fanex.Logging.Extensions.Database.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using Data;
    using Data.CrossCutting.WrappingException;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Extensions/DatabaseInfoGetters.cs")]
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
                new Exception("Inner exception"),
                "Setting Key",
                dbSetting,
                dbParameterCollection,
                600);

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
                    ["Parameters"] = "",
                    ["Line"] = 0,
                    ["CommandTimeout"] = 600,
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

        [Fact]
        public void GetParameters_Always_ReturnCombinedParameterString()
        {
            // Arrange
            var parameters = new[] {
                new SqlParameter(){ ParameterName="name1", Value="value1" },
                new SqlParameter(){ ParameterName="name2", Value="value2" }
            };

            // Act
            string actualParamsString = DatabaseInfoGetters.GetParameters(parameters);

            // Assert
            Assert.Equal("name1=value1;name2=value2", actualParamsString);
        }
    }
}