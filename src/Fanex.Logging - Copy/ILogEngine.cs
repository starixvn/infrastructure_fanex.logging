namespace Fanex.Logging
{
    using System.Diagnostics;

    public interface ILogEngine
    {
        void Write(string message, TraceEventType severity, string category);
    }
}