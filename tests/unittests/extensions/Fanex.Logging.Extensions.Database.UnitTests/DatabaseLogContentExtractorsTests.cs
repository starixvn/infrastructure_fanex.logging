namespace Fanex.Logging.Extensions.Database.UnitTests;

using System;
using System.Collections.Generic;
using Data;
using Xunit;

[Trait("Unit Tests", "Extensions/DatabaseInfoGetters.cs")]
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
        // Arrange
        var objectDbException = new ObjectDbException(
            new Exception("Inner exception"),
            "Setting Key",
            "SPNAME",
            "DbName",
            "packet size=4096; Data Source=10.18.200.232;Initial Catalog=bodb_OER;integrated security=false; User Id=bodbAdminNet;Password=23232323;",
            600,
            new Dictionary<string,string> { { "Name", "Value" } });

        // Act
        var dbExceptionInfo = DatabaseInfoGetters.GetDbExceptionInfo(objectDbException);

        // Assert
        Assert.Equal(
            new Dictionary<string, object>
            {
                ["Server"] = "10.18.200.232",
                ["DbName"] = "DbName",
                ["UserId"] = "bodbAdminNet",
                ["SpName"] = "SPNAME",
                ["Parameters"] = "",
                ["Line"] = 0,
                ["CommandTimeout"] = 600
            },
            dbExceptionInfo);
    }

       
    [Fact]
    public void GetParameters_Always_ReturnCombinedParameterString()
    {
        // Arrange
        var parameters = new Dictionary<string,object> {
            { "name1", "value1" },
            { "name2", "value2"},
            { "name3", null},
            { "name4", 2}
        };

        // Act
        var actualParamsString = DatabaseInfoGetters.GetParameters(parameters);

        // Assert
        Assert.Equal("name1=value1;name2=value2;name3=;name4=2", actualParamsString);
    }

    [Fact]
    public void GetDbExceptionInfo_InnerExceptionIsObjectDbException_ReturnDbExceptionInfo()
    {
        // Arrange
        var objectDbException = new ObjectDbException(
            new Exception("Inner exception"),
            "Setting Key",
            "SPNAME",
            "DbName",
            "packet size=4096; Data Source=10.18.200.232;Initial Catalog=bodb_OER;integrated security=false; User Id=bodbAdminNet;Password=23232323;",
            600,
            new Dictionary<string,string> { { "Name", "Value" } });

        // Act
        var dbExceptionInfo = DatabaseInfoGetters.GetDbExceptionInfo(new Exception("Message", objectDbException));

        // Assert
        Assert.Equal(
            new Dictionary<string, object>
            {
                ["Server"] = "10.18.200.232",
                ["DbName"] = "DbName",
                ["UserId"] = "bodbAdminNet",
                ["SpName"] = "SPNAME",
                ["Parameters"] = "",
                ["Line"] = 0,
                ["CommandTimeout"] = 600
            },
            dbExceptionInfo);
    }
}