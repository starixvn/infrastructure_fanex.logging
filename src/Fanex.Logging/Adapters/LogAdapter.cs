namespace Fanex.Logging.Adapters
{
    using MPEL = Microsoft.Practices.EnterpriseLibrary.Logging;

    public interface ILogAdapter
    {
        void Write(LogEntryAdapter logEntryAdapter);
    }

    public class LogAdapter : ILogAdapter
    {
        public void Write(LogEntryAdapter logEntryAdapter)
        {
            var logEntry = new MPEL.LogEntry
            {
                Message = logEntryAdapter.Message,
                Title = logEntryAdapter.Title,
                Severity = logEntryAdapter.Severity
            };

            logEntry.Categories.Add(logEntryAdapter.Category);

            MPEL.Logger.Write(logEntry);
        }
    }
}