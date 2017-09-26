namespace Fanex.Logging
{
    using System.Collections.Generic;

    public interface ILogEntry
    {
        ILogEntry AddInfo(string information);

        ILogEntry HideSensitiveInfo(IEnumerable<string> sensitiveParams, params string[] regexSensitiveParams);
    }
}