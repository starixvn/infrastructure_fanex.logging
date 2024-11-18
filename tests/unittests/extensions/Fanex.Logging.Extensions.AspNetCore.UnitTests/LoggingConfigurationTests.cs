namespace Fanex.Logging.Extensions.AspNetCore.UnitTests;

using System;
using System.Collections.Generic;
using Xunit;

[Trait("Unit Tests", "Extensions/AspNetCore/LoggingConfiguration.cs")]
public class LoggingConfigurationTests
{
    private readonly LoggingConfiguration loggingConfiguration = new();

    [Fact]
    public void UseGetDbExceptionInfo_FuncIsNull_ThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => loggingConfiguration.UseGetDbExceptionInfo(null));
    }

    [Fact]
    public void UseGetDbExceptionInfo_Always_SetFunc()
    {
        // Arrange
        Func<Exception, IDictionary<string, object>> getDbInfoFunc = _ => new Dictionary<string, object> { ["data"] = "value" };

        // Act
        loggingConfiguration.UseGetDbExceptionInfo(getDbInfoFunc);

        // Assert
        Assert.Equal(getDbInfoFunc, loggingConfiguration.GetDbExceptionInfo);
    }

    [Fact]
    public void HideSensitiveInfo_SensitiveParamsAreNull_NotSetSensitiveParams()
    {
        // Act
        loggingConfiguration.HideSensitiveInfo(null);

        // Assert
        Assert.True(loggingConfiguration.Patterns == null);
    }

    [Fact]
    public void HideSensitiveInfo_Always_SetSensitiveParams()
    {
        // Arrange
        var paramPatterns = new List<string>
        {
            "paramname1"
        };

        // Act
        loggingConfiguration.HideSensitiveInfo(paramPatterns);

        // Assert
        Assert.Equal(paramPatterns, loggingConfiguration.Patterns);
    }

    [Fact]
    public void GetHttpHeaderInfo_Always_SetTrueForEnabledLogHttpHeader()
    {
        // Act
        loggingConfiguration.GetHttpHeaderInfo();

        // Assert
        Assert.True(loggingConfiguration.EnabledLogHttpHeader);
    }

    [Fact]
    public void GetHttpSessionInfo_Always_SetTrueForEnabledLogHttpHeader()
    {
        // Act
        loggingConfiguration.GetHttpSessionInfo();

        // Assert
        Assert.True(loggingConfiguration.EnabledLogHttpSession);
    }

    [Fact]
    public void IgnoreExceptionTypes_Always_AddExceptionType()
    {
        // Act
        loggingConfiguration.IgnoreExceptionTypes(typeof(NullReferenceException));

        // Assert
        Assert.True(loggingConfiguration.IgnoredExceptionTypes.Contains(typeof(NullReferenceException)));
    }

    [Fact]
    public void IgnoreException_Always_AddExceptionType()
    {
        // Act
        loggingConfiguration.IgnoreException<NullReferenceException>();

        // Assert
        Assert.True(loggingConfiguration.IgnoredExceptionTypes.Contains(typeof(NullReferenceException)));
    }
}