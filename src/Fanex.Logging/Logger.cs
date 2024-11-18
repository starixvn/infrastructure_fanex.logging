namespace Fanex.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;

public sealed partial class Logger(
    IEnumerable<ILogEngine> logEngines,
    string defaultLogCategory)
    : ILogger
{
    private static Logger logger;
    private static readonly object SyncRoot = new();

    private readonly string defaultLogCategory = defaultLogCategory ?? LogManager.Instance.DefaultLogCategory;
    internal readonly IEnumerable<ILogEngine> LogEngines = logEngines ?? LogManager.Instance.LogEngines;

    public Logger()
        : this(null, null)
    {
    }

    public static ILogger Log
    {
        get
        {
            if (logger != null)
            {
                return logger;
            }

            lock (SyncRoot)
            {
                logger ??= new Logger();
            }

            return logger;
        }
    }

    #region Debug functions

    public void Debug(string message, string category)
        => WriteLog(message, category, TraceEventType.Verbose);

    public void Debug(string message)
        => Debug(message, defaultLogCategory);

    public void Debug(string format, params object[] args)
        => Debug(string.Format(format, args));

    public void Debug(string format, string category, params object[] args)
        => Debug(string.Format(format, args), category);

    #endregion Debug functions

    #region Info functions

    public void Info(string message, string category)
        => WriteLog(message, category, TraceEventType.Information);

    public void Info(string message)
        => Info(message, defaultLogCategory);

    public void Info(string format, params object[] args)
        => Info(string.Format(format, args));

    public void Info(string format, string category, params object[] args)
        => Info(string.Format(format, args), category);

    #endregion Info functions

    #region Warning functions

    public void Warn(string message, string category)
        => WriteLog(message, category, TraceEventType.Warning);

    public void Warn(string message)
        => Warn(message, defaultLogCategory);

    public void Warn(string format, params object[] args)
        => Warn(string.Format(format, args));

    public void Warn(string format, string category, params object[] args)
        => Warn(string.Format(format, args), category);

    #endregion Warning functions

    #region Error functions

    public void Error(string message, string category)
        => WriteLog(message, category, TraceEventType.Error);

    public void Error(string message)
        => Error(message, defaultLogCategory);

    public void Error(string message, Exception exception)
        => Error(message, defaultLogCategory, exception);

    public void Error(string message, string category, Exception exception)
        => Error($"{message}\n{exception}", category);

    #endregion Error functions

    #region Critical

    public void Critical(string message, string category)
        => WriteLog(message, category, TraceEventType.Critical);

    public void Critical(string message)
        => Critical(message, defaultLogCategory);

    public void Critical(string format, params object[] args)
        => Critical(string.Format(format, args));

    public void Critical(string format, string category, params object[] args)
        => Critical(string.Format(format, args), category);

    #endregion Critical

    #region WriteLog functions

    public void WriteLog(string message, string category, TraceEventType traceEventType)
    {
        foreach (var logEngine in LogEngines)
        {
            logEngine.Write(
                message,
                GetSeverity(traceEventType),
                string.IsNullOrEmpty(category)
                    ? defaultLogCategory
                    : category);
        }
    }

    private static TraceEventType GetSeverity(TraceEventType traceEventType)
    {
        return traceEventType switch
        {
            TraceEventType.Verbose or TraceEventType.Warning or TraceEventType.Critical or TraceEventType.Error =>
                traceEventType,
            _ => TraceEventType.Information
        };
    }

    #endregion WriteLog functions
}