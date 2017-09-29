using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Logging.Adapters
{
    internal interface ILoggingEngine
    {
        void Write(LogEntryAdapter logEntryAdapter);
    }
}