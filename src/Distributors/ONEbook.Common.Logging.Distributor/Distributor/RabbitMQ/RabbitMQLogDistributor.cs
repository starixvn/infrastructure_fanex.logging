namespace ONEbook.Common.Logging.Distributor
{
    using System;
    using System.Linq;
    using EasyNetQ;
    using Fanex.Logging;
    using entlibLog = Microsoft.Practices.EnterpriseLibrary.Logging;

    public class RabbitMQLogDistributor
    {
        private const string RoutingKey = "RabbitMQTraceListener.LogEntryQueueRouteKey";
        private static volatile IBus _rabbitMQBus;
        private string _rabbitMQConnectionString;

        //private const RabbitMQ
        public RabbitMQLogDistributor(string rabbitMQConnectionString)
        {
            _rabbitMQConnectionString = EasyNetQConnectionStringCryptography.DecryptConnectionString(rabbitMQConnectionString);
            this.Start();
        }

        public void Start()
        {
            try
            {
                _rabbitMQBus = RabbitHutch.CreateBus(_rabbitMQConnectionString);
                this.SubscribeLogEntry();

                entlibLog.Logger.Write("Fanex.Logging.Distributor.Service has been started.", "Fanex-Logging-Test");
            }
            catch(Exception ex)
            {
                entlibLog.Logger.Write(ex.ToString(), "Fanex-Logging-Test");
            }
        }

        public void Stop()
        {
            _rabbitMQBus.Dispose();
        }

        public void SubscribeLogEntry()
        {
            _rabbitMQBus.Subscribe<LogEntry>(RoutingKey, logEntry =>
            {
                if (logEntry != null)
                {
                    var msLogEntry = new entlibLog.LogEntry
                    {
                        ActivityId = logEntry.ActivityId,
                        AppDomainName = logEntry.AppDomainName,
                        Categories = logEntry.Categories,
                        EventId = logEntry.EventId,
                        ExtendedProperties = logEntry.ExtendedProperties,
                        MachineName = logEntry.MachineName,
                        Message = logEntry.Message,
                        Priority = logEntry.Priority,
                        ProcessId = logEntry.ProcessId,
                        ProcessName = logEntry.ProcessName,
                        ManagedThreadName = logEntry.ManagedThreadName,
                        RelatedActivityId = logEntry.RelatedActivityId,
                        Severity = logEntry.Severity,
                        TimeStamp = logEntry.TimeStamp,
                        Title = logEntry.Title,
                        Win32ThreadId = logEntry.Win32ThreadId
                    };

                    if (msLogEntry.Categories != null
                        && msLogEntry.Categories.Any())
                    {
                        entlibLog.Logger.Write(msLogEntry);
                    }
                    else
                    {
                        entlibLog.Logger.Write(msLogEntry.Message);
                    }
                }
            }, config => config.WithTopic(RoutingKey));
        }
    }
}