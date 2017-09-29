namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;

    public interface ILogEntry
    {
        ILogEntry AddInfo(string information);

        ILogEntry AddContent(Func<string> contentExtractor);

        ILogEntry HideSensitiveInfo(IEnumerable<string> sensitiveParams, params string[] regexSensitiveParams);
    }    
}