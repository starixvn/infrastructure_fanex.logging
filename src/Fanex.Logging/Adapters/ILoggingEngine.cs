namespace Fanex.Logging.Adapters
{
    using System.Diagnostics;

    internal interface ILoggingEngine
    {
        void Write(string message, TraceEventType severity, string category);
    }
}