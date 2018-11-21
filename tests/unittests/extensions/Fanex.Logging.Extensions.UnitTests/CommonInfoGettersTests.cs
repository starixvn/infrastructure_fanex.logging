namespace Fanex.Logging.Extensions.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Xunit;
    using static CommonInfoGetters;

    [Trait("Unit Tests", "Extensions/CommonInfoGetters.cs")]
    public class CommonInfoGettersTests
    {
        [Fact]
        public void GetExceptionInfo_ExceptionIsNull_ReturnEmptyContent()
        {
            // Act
            var exceptionInfo = GetExceptionInfo(null);

            // Assert
            Assert.Same(Constants.EmptyDictionary, exceptionInfo);
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

        [Fact]
        public void ConvertStreamToString_RequestStreamIsNull_ReturnNAString()
        {
            // Act
            var bodyData = ConvertStreamToString(null);

            // Assert
            Assert.Equal("N/A", bodyData);
        }

        [Theory]
        [InlineData("{ 'demo': 1, 'demo2': '33333' }", "{ 'demo': 1, 'demo2': '33333' }")]
        [InlineData("{ \\\"demo\\\": 1, \\\"demo2\\\": '33333' }", "{ \"demo\": 1, \"demo2\": '33333' }")]
        public void ConvertStreamToString_RequestStreamContainsJsonData_ReturnEmpty(
            string requestJson,
            string expectedJson)
        {
            // Arrange
            using (Stream stream = GenerateStreamFromString(requestJson))
            {
                // Act
                var bodyData = ConvertStreamToString(stream);

                // Assert
                Assert.Equal(expectedJson, bodyData);
            }
        }

        internal static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}