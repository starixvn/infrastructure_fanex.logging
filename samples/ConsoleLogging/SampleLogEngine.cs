namespace Fanex.LoggingTest
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Logging;

    public class SampleLogEngine : ILogEngine
    {
        public void Write(string message, TraceEventType severity, string category)
        {
            Console.WriteLine($"Message:{message}, Severity:{severity}, Category:{category}");
        }

        public Task WriteAsync(string message, TraceEventType severity, string category)
        {
            throw new NotImplementedException();
        }
    }
}