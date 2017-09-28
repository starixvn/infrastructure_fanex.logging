namespace Fanex.Logging.Extensions.Database.UnitTests
{
    using System;
    using System.Data.Common;
    using System.Data.SqlClient;
    using Data;
    using Data.CrossCutting.WrappingException;
    using NSubstitute;
    using Xunit;

    [Trait("Category", "Extensions/DatabaseLoggingExtensions.cs")]
    public class DatabaseLoggingExtensionsTests
    {
        private readonly ILogData logData;

        public DatabaseLoggingExtensionsTests()
        {
            logData = new LogData("Message");
        }

        [Fact]
        public void AddSqlInfo_LogDataIsNull_ReturnNull()
        {
            // Arrange
            ILogData nullLogData = null;

            // Act
            nullLogData = nullLogData.AddSqlInfo(new Exception());

            // Assert
            Assert.Null(nullLogData);
        }

        [Fact]
        public void AddSqlInfo_ExceptionIsNull_ReturnLogData()
        {
            // Act
            var actualLogData = logData.AddSqlInfo(new Exception());

            // Assert
            Assert.Same(logData, actualLogData);
        }

        [Fact]
        public void AddSqlInfo_ExceptionIsObjectDbException_ReturnLogDataWithDbInfo()
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
            logData.AddSqlInfo(objectDBException);

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
                        logData.ToString());
        }

        [Fact]
        public void AddSqlInfo_ExceptionIsSqlException_ReturnLogDataWithDbInfo()
        {
            // Arrange
            var sqlException = CreateSqlException();

            // Act
            logData.AddSqlInfo(sqlException);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "DATABASE INFO:"
                        + "\r\n\tSQL Server: ."
                        + "\r\n\tStored Procedure: "
                        + "\r\n\tLine Number: 65536",
                        logData.ToString());
        }

        [Fact]
        public void AddSqlInfo_InnerExceptionIsSqlException_ReturnLogDataWithDbInfo()
        {
            // Arrange
            var sqlException = CreateSqlException();
            var exception = new Exception("message", sqlException);

            // Act
            logData.AddSqlInfo(sqlException);

            // Assert
            Assert.Equal("Message"
                        + "\r\n---------------------------------------------------------------------------------------------------------------\r\n"
                        + "DATABASE INFO:"
                        + "\r\n\tSQL Server: ."
                        + "\r\n\tStored Procedure: "
                        + "\r\n\tLine Number: 65536",
                        logData.ToString());
        }

        public SqlException CreateSqlException()
        {
            SqlException exception = null;

            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source=.;Database=GUARANTEED_TO_FAIL;Connection Timeout=1");
                conn.Open();
            }
            catch (SqlException ex)
            {
                exception = ex;
            }

            return exception;
        }
    }
}