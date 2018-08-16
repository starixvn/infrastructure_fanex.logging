namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class LogEntry
    {
        public string AppDomainName { get; set; }

        public string ActivityIdString { get; set; }

        public string ErrorMessages { get; set; }

        public Guid? RelatedActivityId { get; set; }

        public Guid ActivityId { get; set; }

        public string TimeStampString { get; set; }

        public IDictionary<string, object> ExtendedProperties { get; set; }

        public string Win32ThreadId { get; set; }

        public string ManagedThreadName { get; set; }

        public string ProcessName { get; set; }

        public string ProcessId { get; set; }

        public DateTime TimeStamp { get; set; }

        public List<string> CategoriesStrings { get; set; }

        public string Title { get; set; }

        public string LoggedSeverity { get; set; }

        public TraceEventType Severity { get; set; }

        public int EventId { get; set; }

        public int Priority { get; set; }

        public List<string> Categories { get; set; }

        public string Message { get; set; }

        public string MachineName { get; set; }
    }
}