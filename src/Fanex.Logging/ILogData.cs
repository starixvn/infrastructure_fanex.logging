namespace Fanex.Logging
{
    using System.Collections.Generic;

    public interface ILogData
    {
        ILogData AddInfo(string information);

        ILogData HideSensitiveInfo(IEnumerable<string> sensitiveParams, params string[] regexSensitiveParams);
    }    
}