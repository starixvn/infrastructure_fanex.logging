namespace Fanex.Logging.CustomTraceListener
{
    using System.Diagnostics;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    public abstract class CustomFormattedTraceListenerBase : CustomTraceListener
    {
        /// <summary>
        /// Initalizes a new instance of <see cref="FormattedTraceListenerBase"/>.
        /// </summary>
        protected CustomFormattedTraceListenerBase()
        {
        }

        /// <summary>
        /// Initalizes a new instance of <see cref="FormattedTraceListenerBase"/> with a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="formatter">The <see cref="ILogFormatter"/> to use when tracing a <see cref="LogEntry"/>.</param>
        protected CustomFormattedTraceListenerBase(ILogFormatter formatter)
        {
            this.Formatter = formatter;
        }

        /// <summary>
        /// Specifies whether this TraceListener is threadsafe
        /// </summary>
        public override bool IsThreadSafe
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Overriding TraceData method for the base TraceListener class because it calls the
        /// private WriteHeader and WriteFooter methods which actually call the Write method again
        /// and this amounts to multiple log messages
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                string text1 = string.Empty;
                if (data != null)
                {
                    text1 = data.ToString();

                    this.WriteLine(text1);
                }
            }
        }
    }
}