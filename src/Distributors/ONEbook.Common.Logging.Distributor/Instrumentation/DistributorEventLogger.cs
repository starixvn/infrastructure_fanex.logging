using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation
{
    public class DistributorEventLogger
    {
        internal static string Header = ExceptionFormatter.Header;
        private static readonly string DefaultEventSource = Resources.DistributorEventLoggerDefaultApplicationName;
        private string eventSource = null;
        private readonly NameValueCollection additionalInfo = new NameValueCollection();

        /// <summary>
        /// The Event logger used to back up configured logging sinks in the event of problems.
        /// Used to write diagnostic messages to the event log.
        /// </summary>
        public DistributorEventLogger()
            : this(DefaultEventSource)
        {
        }

        public DistributorEventLogger(string eventSource)
        {
            if (string.IsNullOrEmpty(eventSource)) throw new ArgumentNullException("eventSource");

            this.EventSource = eventSource;
        }

        public string EventSource
        {
            get { return (eventSource ?? DefaultEventSource); }
            set { eventSource = value; }
        }

        public void AddMessage(string message, string value)
        {
            additionalInfo.Add(message, value);
        }

        public void LogServiceStarted()
        {
            LogServiceLifecycleEvent(string.Format(CultureInfo.CurrentCulture, Resources.ServiceStartComplete, this.EventSource), true);
        }

        public void LogServiceResumed()
        {
            LogServiceLifecycleEvent(string.Format(CultureInfo.CurrentCulture, Resources.ServiceResumeComplete, this.EventSource), true);
        }

        public void LogServiceStopped()
        {
            LogServiceLifecycleEvent(string.Format(CultureInfo.CurrentCulture, Resources.ServiceStopComplete, this.EventSource), false);
        }

        public void LogServicePaused()
        {
            LogServiceLifecycleEvent(string.Format(CultureInfo.CurrentCulture, Resources.ServicePausedSuccess, this.EventSource), false);
        }

        public void LogServiceFailure(string message, Exception exception, TraceEventType eventType)
        {
            this.AddMessage(DistributorEventLogger.Header, message);
            this.WriteToLog(exception, eventType);
        }

        private string GetMessage(Exception exception)
        {
            ExceptionFormatter exFormatter = new ExceptionFormatter(additionalInfo, this.EventSource);
            return exFormatter.GetMessage(exception);
        }

        private EventLogEntryType GetEventLogEntryType(TraceEventType severity)
        {
            if ((severity == TraceEventType.Error) || (severity == TraceEventType.Critical))
            {
                return EventLogEntryType.Error;
            }
            if (severity == TraceEventType.Warning)
            {
                return EventLogEntryType.Warning;
            }
            return EventLogEntryType.Information;
        }

        private void WriteToLog(Exception exception, TraceEventType severity)
        {
            string finalMessage = String.Empty;

            finalMessage = GetMessage(exception);
            additionalInfo.Clear();

            EventLog.WriteEntry(this.EventSource, finalMessage, GetEventLogEntryType(severity));
        }

        private void LogServiceLifecycleEvent(string message, bool started)
        {
            this.AddMessage(DistributorEventLogger.Header, message);
            this.WriteToLog(null, TraceEventType.Information);
        }
    }
}
