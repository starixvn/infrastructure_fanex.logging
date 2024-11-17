namespace Fanex.Logging;

using System;
using System.Threading.Tasks;

public partial interface ILogger
{
    Task DebugAsync(string message);

    Task DebugAsync(string message, string category);

    Task DebugAsync(string format, params object[] args);

    Task DebugAsync(string format, string category, params object[] args);

    Task InfoAsync(string message);

    Task InfoAsync(string message, string category);

    Task InfoAsync(string format, params object[] args);

    Task InfoAsync(string format, string category, params object[] args);

    Task WarnAsync(string message);

    Task WarnAsync(string message, string category);

    Task WarnAsync(string format, params object[] args);

    Task WarnAsync(string format, string category, params object[] args);

    Task ErrorAsync(string message);

    Task ErrorAsync(string message, string category);

    Task ErrorAsync(string message, Exception exception);

    Task ErrorAsync(string message, string category, Exception exception);

    Task CriticalAsync(string message);

    Task CriticalAsync(string message, string category);

    Task CriticalAsync(string format, params object[] args);

    Task CriticalAsync(string format, string category, params object[] args);
}