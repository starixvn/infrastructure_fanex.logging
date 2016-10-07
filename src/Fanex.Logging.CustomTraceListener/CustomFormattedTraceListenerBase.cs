namespace Fanex.Logging.CustomTraceListener
{
    using System.Diagnostics;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    public abstract class CustomFormattedTraceListenerBase : CustomTraceListener
    {
        protected CustomFormattedTraceListenerBase()
        {
        }

        protected CustomFormattedTraceListenerBase(ILogFormatter formatter)
        {
            Formatter = formatter;
        }

        public override bool IsThreadSafe
            => true;

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (Filter?.ShouldTrace(eventCache, source, eventType, id, null, null, data, null) == true)
            {
                var message = string.Empty;

                if (data != null)
                {
                    message = data.ToString();

                    WriteLine(message);
                }
            }
        }
    }
}