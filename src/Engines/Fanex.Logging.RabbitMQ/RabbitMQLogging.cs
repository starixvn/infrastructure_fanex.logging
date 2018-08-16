namespace Fanex.Logging.RabbitMQ
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using EasyNetQ;
    using Fanex.Logging.RabbitMQ.Configurations;
    using Fanex.Logging.RabbitMQ.Helpers;

    public class RabbitMQLogging : ILogEngine
    {
        private const string RoutingKey = "RabbitMQTraceListener.LogEntryQueueRouteKey";
        private readonly IRabbitMQLoggingConfigs rabbitMQLoggingConfigs;
        private readonly IBus bus;

        public RabbitMQLogging(
            IRabbitMQLoggingConfigs rabbitMQLoggingConfigs,
            IBus bus = null)
        {
            this.rabbitMQLoggingConfigs = rabbitMQLoggingConfigs
                ?? throw new ArgumentNullException(nameof(rabbitMQLoggingConfigs));
            this.bus = bus ?? RabbitMQManager.CreateBus(rabbitMQLoggingConfigs.ConnectionString);
        }

        public void Write(string message, TraceEventType severity, string category)
        {
            var logEntry = new LogEntry
            {
                AppDomainName = "AppDomainName",
                ActivityId = Guid.NewGuid(),
                TimeStampString = DateTime.Now.ToString(),
                ExtendedProperties = new Dictionary<string, object>(),
                Win32ThreadId = Thread.CurrentThread.ManagedThreadId.ToString(),
                ProcessName = Process.GetCurrentProcess().ProcessName,
                ProcessId = Process.GetCurrentProcess().Id.ToString(),
                TimeStamp = DateTime.Now,
                CategoriesStrings = new List<string> { category },
                Categories = new List<string> { category },
                Title = category,
                Severity = severity,
                Message = message,
                MachineName = System.Net.Dns.GetHostName(),
                ManagedThreadName = Thread.CurrentThread.Name
            };

            bus.Publish(logEntry, RoutingKey);
        }
    }
}