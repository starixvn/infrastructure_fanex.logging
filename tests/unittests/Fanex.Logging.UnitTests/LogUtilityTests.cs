namespace Fanex.Logging.UnitTests
{
    using System;
    using System.Net;
    using Xunit;
    using Fanex.Logging;

    [Trait("Category", "LogUtility.cs")]
    public class LogUtilityTests
    {
        [Fact]
        public void AddExceptionInfo_LogEntryIsNull_ReturnNull()
        {
            // Arrange
            LogEntry logEntry = null;

            // Act
            logEntry.AddExceptionInfo(new Exception());

            // Assert
            Assert.Null(logEntry);
        }

        [Fact]
        public void AddExceptionInfo_ExceptionIsNull_ReturnLogEntry()
        {
            // Arrange
            var logEntry = new LogEntry();

            // Act
            var actualLogEntry = logEntry.AddExceptionInfo(null);

            // Assert
            Assert.Same(logEntry, actualLogEntry);
        }

        [Fact]
        public void AddExceptionInfo_Always_ReturnLogEntryWithExceptionInfo()
        {
            // Arrange
            var logEntry = new LogEntry();
            var customException = new DllNotFoundException();

            // Act
            logEntry.AddExceptionInfo(customException);

            // Assert
            Assert.Equal(
                "EXCEPTION:"
                    + "\r\n\tSource: "
                    + "\r\n\tType: System.DllNotFoundException"
                    + "\r\n\tTarget Site: "
                    + "\r\n\tStack Trace: "
                    + "\r\n\tDetails: System.DllNotFoundException: Dll was not found.",
                logEntry.ToString());
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
        public void AddWebServerInfo_LogEntryIsNull_ReturnNull()
        {
            // Arrange
            LogEntry logEntry = null;

            // Act
            logEntry.AddServerInfo();

            // Assert
            Assert.Null(logEntry);
        }

        [Fact]
        public void AddWebServerInfo_Always_ReturnLogEntryWithServerInformation()
        {
            // Arrange
            var logEntry = new LogEntry();

            // Act
            logEntry.AddServerInfo();

            // Assert
            var logResult = logEntry.ToString();
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