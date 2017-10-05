namespace Fanex.Logging.Extensions.Web.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "Extensions/ExceptionHandler.cs")]
    public class ExceptionHandlerTests
    {
        [Fact]
        public void UseConfig_InfoLogConfigurationIsNull_ThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExceptionHandler.UseConfig(null));
        }

        [Fact]
        public void UseConfig_Always_SetConfig()
        {
            // Arrange
            var config = new LoggingConfiguration();

            // Act
            ExceptionHandler.UseConfig(config);

            // Assert
            Assert.Equal(config, ExceptionHandler.config);
        }

        [Fact]
        public void UseGetDbExceptionInfo_GetDbExceptionInfoMethodIsNull_ThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExceptionHandler.UseGetDbExceptionInfo(null));
        }

        [Fact]
        public void UseGetDbExceptionInfo_Always_SetUseGetDbExceptionInfo()
        {
            // Arrange
            Func<Exception, IDictionary<string, object>> getDbExceptionInfo = (ex) => new Dictionary<string, object> { ["data"] = "value" };

            // Act
            ExceptionHandler.UseGetDbExceptionInfo(getDbExceptionInfo);

            // Assert
            Assert.Equal(getDbExceptionInfo, ExceptionHandler.config.GetDbExceptionInfo);
        }
    }
}