namespace Fanex.Logging.Adapters
{
    using System.Diagnostics;

    public class LogEntryAdapter
    {
        public TraceEventType Severity { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }
    }
}