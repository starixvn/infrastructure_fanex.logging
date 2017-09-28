using Xunit;
namespace Fanex.Logging.UnitTests
{
    using System;
    using System.Net;
    using Xunit;

    [Trait("Category", "LogUtility.cs")]
    public class LogUtilityTests
    {
        [Fact]
        public void AddExceptionInfo_LogDataIsNull_ReturnNull()
        {
            // Arrange
            LogData logData = null;

            // Act
            logData.AddExceptionInfo(new Exception());

            // Assert
            Assert.Null(logData);
        }

        [Fact]
        public void AddExceptionInfo_ExceptionIsNull_ReturnLogData()
        {
            // Arrange
            var logData = new LogData();

            // Act
            var actualLogData = logData.AddExceptionInfo(null);

            // Assert
            Assert.Same(logData, actualLogData);
        }

        [Fact]
        public void AddExceptionInfo_Always_ReturnLogDataWithExceptionInfo()
        {
            // Arrange
            var logData = new LogData();
            var customException = new DllNotFoundException();

            // Act
            logData.AddExceptionInfo(customException);

            // Assert
            Assert.Equal(
                "EXCEPTION:"
                    + "\r\n\tSource: "
                    + "\r\n\tType: System.DllNotFoundException"
                    + "\r\n\tTarget Site: "
                    + "\r\n\tStack Trace: "
                    + "\r\n\tDetails: System.DllNotFoundException: Dll was not found.",
                logData.ToString());
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
            var machineIP = LogUtility.GetMachineIP(hostEntry);

            // Assert
            Assert.Equal("160.134.1.0", machineIP);
        }

        [Fact]
        public void GetMachineIP_NotFoundIP_ReturnEmpty()
        {
            // Arrange
            var hostEntry = new IPHostEntry
            {
                AddressList = new IPAddress[] { }
            };

            // Act
            var machineIP = LogUtility.GetMachineIP(hostEntry);

            // Assert
            Assert.Empty(machineIP);
        }

        [Fact]
        public void AddWebServerInfo_LogDataIsNull_ReturnNull()
        {
            // Arrange
            LogData logData = null;

            // Act
            logData.AddWebServerInfo();

            // Assert
            Assert.Null(logData);
        }

        [Fact]
        public void AddWebServerInfo_Always_ReturnLogDataWithServerInformation()
        {
            // Arrange
            var logData = new LogData();

            // Act
            logData.AddWebServerInfo();

            // Assert
            var logResult = logData.ToString();
            Assert.True(
                logResult.Contains("WEB SERVER INFO")
                && logResult.Contains("Server IP:")
                && logResult.Contains("Machine:")
                && logResult.Contains("OS:")
                && logResult.Contains("System Time:")
                && logResult.Contains("User Domain:"));
        }
    }
}