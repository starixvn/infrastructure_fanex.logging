namespace Fanex.Logging.RabbitMQ
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using EasyNetQ;
    using Fanex.Logging.RabbitMQ.Helpers;

    public class RabbitMQLogging : ILogEngine
    {
        private const string RoutingKey = "RabbitMQTraceListener.LogEntryQueueRouteKey";
        private readonly string connectionString;
        private readonly IBus bus;

        public RabbitMQLogging(
            string connectionString,
            IBus bus = null)
        {
            this.connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
            this.bus = bus ?? RabbitMQManager.CreateBus(connectionString);
        }

        public void Write(string message, TraceEventType severity, string category)
        {
            var currentThread = Thread.CurrentThread;
            var currentProcess = Process.GetCurrentProcess();
            var now = DateTime.Now.ToUniversalTime();
            var categoryList = new List<string> { category };
            var logEntry = new LogEntry
            {
                AppDomainName = "NotFound",
                ActivityId = Guid.NewGuid(),
                TimeStampString = now.ToString(),
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

            bus.Publish(logEntry, RoutingKey);
        }
    }
}