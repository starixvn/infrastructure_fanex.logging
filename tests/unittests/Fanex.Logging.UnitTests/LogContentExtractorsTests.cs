namespace Fanex.Logging.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Fanex.Logging;
    using Xunit;
    using static Fanex.Logging.LogContentExtractors;

    [Trait("Category", "LogContentExtractors.cs")]
    public class LogContentExtractorsTests
    {
        [Fact]
        public void GetExceptionInfo_ExceptionIsNull_ReturnEmptyContent()
        {
            // Act
            var exceptionInfo = GetExceptionInfo(null);

            // Assert
            Assert.Same(Constants.NoInformationContent, exceptionInfo);
        }

        [Fact]
        public void AddExceptionInfo_Always_ReturnLogEntryWithExceptionInfo()
        {
            // Arrange
            var logEntry = new LogEntry();
            var customException = new DllNotFoundException();

            // Act
            var exceptionInfo = GetExceptionInfo(customException);

            // Assert
            Assert.Equal(
                new List<string>
                {
                    "Source: ",
                    "Type: System.DllNotFoundException",
                    "Target Site: ",
                    "Stack Trace: ",
                    "Details: System.DllNotFoundException: Dll was not found."
                    },
                exceptionInfo);
        }

        [Fact]
        public void CombineInfo_Always_ReturnCombinedMessage()
        {
            // Arrange
            var infoGroup = "infoGroup";
            var infoItems = new string[] { "Item1", string.Empty, "Item2" };

            // Act
            var actualCombinedMessage = LogUtility.CombineInfo(infoGroup, infoItems);

            // Assert
            Assert.Equal(
                "infoGroup:"
                + "\r\n\tItem1"
                + "\r\n\t"
                + "\r\n\tItem2",
                actualCombinedMessage);
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
                serverInfo.Exists(info => info.Contains("Server IP:"))
                && serverInfo.Exists(info => info.Contains("Machine:"))
                && serverInfo.Exists(info => info.Contains("OS:"))
                && serverInfo.Exists(info => info.Contains("System Time:"))
                && serverInfo.Exists(info => info.Contains("User Domain:")));
        }
    }
}