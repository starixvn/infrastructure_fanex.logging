namespace Fanex.Logging.Extensions.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Xunit;
    using static CommonInfoGetters;

    [Trait("Category", "Extensions/CommonInfoGetters.cs")]
    public class CommonInfoGettersTests
    {
        [Fact]
        public void GetExceptionInfo_ExceptionIsNull_ReturnEmptyContent()
        {
            // Act
            var exceptionInfo = GetExceptionInfo(null);

            // Assert
            Assert.Same(Constants.DefaultDictionary, exceptionInfo);
        }

        [Fact]
        public void GetExceptionInfo_Always_ReturnLogEntryWithExceptionInfo()
        {
            // Arrange
            var customException = new DllNotFoundException();

            // Act
            var exceptionInfo = GetExceptionInfo(customException);

            // Assert
            Assert.Equal(
                new Dictionary<string, object>
                {
                    ["Source"] = null,
                    ["Type"] = typeof(DllNotFoundException),
                    ["TargetSite"] = null,
                    ["Details"] = "System.DllNotFoundException: Dll was not found."
                },
                exceptionInfo);
        }

        [Fact]
        public void GetMachineIP_IPAddressFamilyIsInterNetwork_ReturnIP()
        {
            // Arrange
            var hostEntry = new IPHostEntry
            {
                AddressList = new IPAddress[] { new IPAddress(100000) }
            };

            // Act
            var machineIP = GetMachineIP(hostEntry);

            // Assert
            Assert.Equal("160.134.1.0", machineIP);
        }

        [Fact]
        public void GetMachineIP_NotFoundIP_ReturnNotAvailable()
        {
            // Arrange
            var hostEntry = new IPHostEntry
            {
                AddressList = new IPAddress[] { }
            };

            // Act
            var machineIP = GetMachineIP(hostEntry);

            // Assert
            Assert.Equal("N/A", machineIP);
        }

        [Fact]
        public void GetServerInfo_Always_ReturnLogEntryWithServerInformation()
        {
            // Act
            var serverInfo = GetServerInfo();

            // Assert
            Assert.True(
                serverInfo.Keys.Any(info => info.Contains("Server IP"))
                && serverInfo.Keys.Any(info => info.Contains("MachineName"))
                && serverInfo.Keys.Any(info => info.Contains("OSVersion"))
                && serverInfo.Keys.Any(info => info.Contains("System Time"))
                && serverInfo.Keys.Any(info => info.Contains("User Domain")));
        }
    }
}