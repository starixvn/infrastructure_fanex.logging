namespace Fanex.Logging
{
    using System.Collections.Generic;

    public interface ILogEntry
    {
        ILogEntry AddInfo(string information);

        ILogEntry CensorSensitiveInfo(IEnumerable<string> sensitiveParams, params string[] regexSensitiveParams);
    }
}