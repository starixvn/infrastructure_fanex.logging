namespace Fanex.Logging
{
    using System;
    using System.Collections.Generic;

    public interface ILogEntry
    {
        ILogEntry AddContent(Func<string> contentExtractor);

        ILogEntry AddContent(List<string> contents);

        ILogEntry Section(string sectionName);

        ILogEntry HideSensitiveInfo(IEnumerable<string> sensitiveParams, params string[] regexSensitiveParams);
    }    
}