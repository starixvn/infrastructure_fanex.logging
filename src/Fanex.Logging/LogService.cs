namespace Fanex.Logging;

using System;
using System.Threading.Tasks;

public static class LogService
{
    [ThreadStatic]
    private static ILogger loggerInstance;

    public static ILogger LoggerInstance
    {
        get => loggerInstance ?? Logger.Log;

        set => loggerInstance = value;
    }

    #region Debug

    public static void Debug(string message)
        => LoggerInstance.Debug(message);

    public static void Debug(string message, string category)
        => LoggerInstance.Debug(message, category);

    public static void Debug(string format, params object[] args)
        => LoggerInstance.Debug(format, args);

    public static void Debug(string format, string category, params object[] args)
        => LoggerInstance.Debug(format, category, args);

    #endregion Debug

    #region Info

    public static void Info(string message)
        => LoggerInstance.Info(message);

    public static void Info(string message, string category)
        => LoggerInstance.Info(message, category);

    public static void Info(string format, params object[] args)
        => LoggerInstance.Info(format, args);

    public static void Info(string format, string category, params object[] args)
        => LoggerInstance.Info(format, category, args);

    #endregion Info

    #region Warn

    public static void Warn(string message)
        => LoggerInstance.Warn(message);

    public static void Warn(string message, string category)
        => LoggerInstance.Warn(message, category);

    public static void Warn(string format, params object[] args)
        => LoggerInstance.Warn(format, args);

    public static void Warn(string format, string category, params object[] args)
        => LoggerInstance.Warn(format, category, args);

    #endregion Warn

    #region Error

    public static void Error(string message)
        => LoggerInstance.Error(message);

    public static void Error(string message, string category)
        => LoggerInstance.Error(message, category);

    public static void Error(string message, Exception exception)
        => LoggerInstance.Error(message, exception);

    public static void Error(string message, string category, Exception exception)
        => LoggerInstance.Error(message, category, exception);

    #endregion Error

    #region Critical

    public static void Critical(string message)
        => LoggerInstance.Critical(message);

    public static void Critical(string message, string category)
        => LoggerInstance.Critical(message, category);

    public static void Critical(string format, params object[] args)
        => LoggerInstance.Critical(format, args);

    public static void Critical(string format, string category, params object[] args)
        => LoggerInstance.Critical(format, category, args);

    #endregion Critical

    #region DebugAsync

    public static Task DebugAsync(string message)
        => LoggerInstance.DebugAsync(message);

    public static Task DebugAsync(string message, string category)
        => LoggerInstance.DebugAsync(message, category);

    public static Task DebugAsync(string format, params object[] args)
        => LoggerInstance.DebugAsync(format, args);

    public static Task DebugAsync(string format, string category, params object[] args)
        => LoggerInstance.DebugAsync(format, category, args);

    #endregion DebugAsync

    #region Info

    public static Task InfoAsync(string message)
        => LoggerInstance.InfoAsync(message);

    public static Task InfoAsync(string message, string category)
        => LoggerInstance.InfoAsync(message, category);

    public static Task InfoAsync(string format, params object[] args)
        => LoggerInstance.InfoAsync(format, args);

    public static Task InfoAsync(string format, string category, params object[] args)
        => LoggerInstance.InfoAsync(format, category, args);

    #endregion Info

    #region Warn

    public static Task WarnAsync(string message)
        => LoggerInstance.WarnAsync(message);

    public static Task WarnAsync(string message, string category)
        => LoggerInstance.WarnAsync(message, category);

    public static Task WarnAsync(string format, params object[] args)
        => LoggerInstance.WarnAsync(format, args);

    public static Task WarnAsync(string format, string category, params object[] args)
        => LoggerInstance.WarnAsync(format, category, args);

    #endregion Warn

    #region Error

    public static Task ErrorAsync(string message)
        => LoggerInstance.ErrorAsync(message);

    public static Task ErrorAsync(string message, string category)
        => LoggerInstance.ErrorAsync(message, category);

    public static Task ErrorAsync(string message, Exception exception)
        => LoggerInstance.ErrorAsync(message, exception);

    public static Task ErrorAsync(string message, string category, Exception exception)
        => LoggerInstance.ErrorAsync(message, category, exception);

    #endregion Error

    #region Critical

    public static Task CriticalAsync(string message)
        => LoggerInstance.CriticalAsync(message);

    public static Task CriticalAsync(string message, string category)
        => LoggerInstance.CriticalAsync(message, category);

    public static Task CriticalAsync(string format, params object[] args)
        => LoggerInstance.CriticalAsync(format, args);

    public static Task CriticalAsync(string format, string category, params object[] args)
        => LoggerInstance.CriticalAsync(format, category, args);

    #endregion Critical
}