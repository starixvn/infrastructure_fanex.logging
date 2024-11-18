namespace Fanex.Logging.Extensions;

using System;
using System.Collections.Generic;
using System.Threading;

public class LoggingConfiguration
{
    internal Func<Exception, IDictionary<string, object>> GetDbExceptionInfo;
    internal List<string> Patterns;
    internal bool EnabledLogHttpHeader;
    internal bool EnabledLogHttpSession;
    internal List<Type> IgnoredExceptionTypes = [typeof(ThreadAbortException)];

    public LoggingConfiguration UseGetDbExceptionInfo(Func<Exception, IDictionary<string, object>> getDbExceptionInfo)
    {
        GetDbExceptionInfo = getDbExceptionInfo ?? throw new ArgumentNullException(nameof(getDbExceptionInfo));

        return this;
    }

    public LoggingConfiguration HideSensitiveInfo(List<string> patterns)
    {
        if (patterns != null)
        {
            Patterns = patterns;
        }

        return this;
    }

    public LoggingConfiguration GetHttpHeaderInfo()
    {
        EnabledLogHttpHeader = true;

        return this;
    }

    public LoggingConfiguration GetHttpSessionInfo()
    {
        EnabledLogHttpSession = true;

        return this;
    }

    /// <summary>
    /// Ignore HttpException and ThreadAbortException
    /// </summary>
    /// <param name="exceptionTypes"></param>
    /// <returns>instance of WebLoggingConfiguration</returns>
    public LoggingConfiguration IgnoreExceptionTypes(params Type[] exceptionTypes)
    {
        IgnoredExceptionTypes.AddRange(exceptionTypes);

        return this;
    }

    /// <summary>
    /// Ignore HttpException and ThreadAbortException
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <returns>instance of WebLoggingConfiguration</returns>
    public LoggingConfiguration IgnoreException<TException>()
    {
        IgnoredExceptionTypes.Add(typeof(TException));

        return this;
    }
}