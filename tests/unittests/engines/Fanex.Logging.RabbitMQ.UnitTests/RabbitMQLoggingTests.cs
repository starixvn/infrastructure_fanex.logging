namespace Fanex.Logging.RabbitMQ.UnitTests
{
    using System;
    using System.Diagnostics;
    using EasyNetQ;
    using EasyNetQ.FluentConfiguration;
    using NSubstitute;
    using Xunit;

    [Trait("Unit Tests", "RabbitMQ/RabbitMQLogging")]
    public class RabbitMQLoggingTests
    {
        private readonly IBus bus;
        private readonly RabbitMQLogging rabbitMQLogging;
        private const string rabbitMQConnectionString = "connection-string";

        public RabbitMQLoggingTests()
        {
            bus = Substitute.For<IBus>();
            rabbitMQLogging = new RabbitMQLogging(rabbitMQConnectionString, bus, 1200);
        }

        [Fact]
        public void Write_Always_ExecuteBusPublish()
        {
            var message = "log-message";
            var severity = TraceEventType.Error;
            var category = "log-category";
            bus.IsConnected.Returns(true);

            rabbitMQLogging.Write(message, severity, category);

            bus.Received().Publish(
                Arg.Is<LogEntry>(logEntry
                                    => logEntry.Message == message
                                        && logEntry.Severity == severity
                                        && logEntry.Title == category),
                Arg.Any<Action<IPublishConfiguration>>());
        }

        [Fact]
        public void WriteAsync_Always_ExecuteBusPublishAsync()
        {
            var message = "log-message";
            var severity = TraceEventType.Error;
            var category = "log-category";
            bus.IsConnected.Returns(true);

            rabbitMQLogging.WriteAsync(message, severity, category);

            bus.Received().PublishAsync(
                Arg.Is<LogEntry>(logEntry
                                    => logEntry.Message == message
                                        && logEntry.Severity == severity
                                        && logEntry.Title == category),
                Arg.Any<Action<IPublishConfiguration>>());
        }
    }
}