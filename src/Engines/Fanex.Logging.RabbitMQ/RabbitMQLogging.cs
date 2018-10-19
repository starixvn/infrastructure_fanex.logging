namespace Fanex.Logging.RabbitMQ
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using EasyNetQ;
    using EasyNetQ.FluentConfiguration;
    using Fanex.Logging.RabbitMQ.Helpers;

    public class RabbitMQLogging : ILogEngine
    {
        private const string RoutingKey = "RabbitMQTraceListener.LogEntryQueueRouteKey";
        private readonly IBus bus;
        private readonly int expirationInMiliseconds = 3600000;

        public RabbitMQLogging(
            string connectionString,
            IBus bus = null,
            int expirationInSeconds = 0)
        {
            this.bus = bus ?? RabbitMQManager.CreateBus(connectionString ?? throw new ArgumentNullException(nameof(connectionString)));
            if (expirationInSeconds > 0)
            {
                this.expirationInMiliseconds = expirationInSeconds * 1000;
            }
        }

        public void Write(string message, TraceEventType severity, string category)
        {
            var logEntry = BuildLogEntry(message, severity, category);

            bus.Publish(logEntry, SettingPublishConfiguration);
        }

        private static LogEntry BuildLogEntry(string message, TraceEventType severity, string category)
        {
            var currentThread = Thread.CurrentThread;
            var currentProcess = Process.GetCurrentProcess();
            var now = DateTime.Now.ToUniversalTime();
            var categoryList = new List<string> { category };

            return new LogEntry
            {
                AppDomainName = "N/A",
                ActivityId = Guid.NewGuid(),
#pragma warning disable S1449 // Culture should be specified for "string" operations
                TimeStampString = now.ToString(),
#pragma warning restore S1449 // Culture should be specified for "string" operations
                ExtendedProperties = new Dictionary<string, object>(),
                Win32ThreadId = currentThread.ManagedThreadId.ToString(),
                ProcessName = currentProcess.ProcessName,
                ProcessId = currentProcess.Id.ToString(),
                TimeStamp = now,
                CategoriesStrings = categoryList,
                Categories = categoryList,
                Title = category,
                Severity = severity,
                Message = message,
                MachineName = Dns.GetHostName(),
                ManagedThreadName = currentThread.Name
            };
        }

        public Task WriteAsync(string message, TraceEventType severity, string category)
        {
            var logEntry = BuildLogEntry(message, severity, category);

            return bus.PublishAsync(logEntry, SettingPublishConfiguration);
        }

        private void SettingPublishConfiguration(IPublishConfiguration publishConfiguration)
            => publishConfiguration.WithTopic(RoutingKey).WithExpires(expirationInMiliseconds);
    }
}