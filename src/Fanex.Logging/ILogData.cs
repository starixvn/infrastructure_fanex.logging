namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;

    public interface ILogData
    {
        ILogData AddInfo(string information);

        ILogData AddContent(Func<string> contentExtractor);

        ILogData HideSensitiveInfo(IEnumerable<string> sensitiveParams, params string[] regexSensitiveParams);
    }
}