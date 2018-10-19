namespace Fanex.Logging
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    public interface ILogEngine
    {
        void Write(string message, TraceEventType severity, string category);

        Task WriteAsync(string message, TraceEventType severity, string category);
    }
}