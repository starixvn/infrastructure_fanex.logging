using Xunit;

namespace Fanex.Logging.Extensions.Web.UnitTests
{
    using System;
    using System.Collections.Generic;

    [Trait("Category", "Extensions/InfoLogConfiguration.cs")]
    public class InfoLogConfigurationTests
    {
        private readonly LoggingConfiguration loggingConfiguration;

        public InfoLogConfigurationTests()
        {
            loggingConfiguration = new LoggingConfiguration();
        }

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
            Func<Exception, List<string>> getDBInfoFunc = (ex) => new List<string> { "result" };

            // Act
            loggingConfiguration.UseGetDbExceptionInfo(getDBInfoFunc);

            // Assert
            Assert.Equal(getDBInfoFunc, loggingConfiguration.GetDbExceptionInfo);
        }

        [Fact]
        public void HideSensitiveInfo_SensitiveParamsAreNull_NotSetSensitiveParams()
        {
            // Act
            loggingConfiguration.HideSensitiveInfo(null);

            // Assert
            Assert.True(loggingConfiguration.SensitiveParams == null);
        }

        [Fact]
        public void HideSensitiveInfo_Always_SetSensitiveParams()
        {
            // Arrange
            var sensitiveParams = new List<string>
            {
                "paramname1"
            };

            // Act
            loggingConfiguration.HideSensitiveInfo(sensitiveParams);

            // Assert
            Assert.Equal(sensitiveParams, loggingConfiguration.SensitiveParams);
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
}