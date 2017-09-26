namespace Fanex.Logging.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Logging;
    using Xunit;

    public class LogEntryTests
    {
        [Fact]
        public void Constructor_CreateConstructorWithMessage_ReturnMessage()
        {
            // Arrange
            var message = "Message";

            // Act
            var actualMessage = new LogEntry(message).ToString();

            // Assert
            Assert.Equal(message, actualMessage);
        }
    }
}
