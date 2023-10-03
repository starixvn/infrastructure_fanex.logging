namespace Fanex.Logging
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class Logger
    {
        #region Debug functions

        public Task DebugAsync(string message, string category)
            => WriteLogAsync(message, category, TraceEventType.Verbose);

        public Task DebugAsync(string message)
            => DebugAsync(message, defaultLogCategory);

        public Task DebugAsync(string format, params object[] args)
            => DebugAsync(string.Format(format, args));

        public Task DebugAsync(string format, string category, params object[] args)
            => DebugAsync(string.Format(format, args), category);

        #endregion Debug functions

        #region Info functions

        public Task InfoAsync(string message, string category)
            => WriteLogAsync(message, category, TraceEventType.Information);

        public Task InfoAsync(string message)
            => InfoAsync(message, defaultLogCategory);

        public Task InfoAsync(string format, params object[] args)
            => InfoAsync(string.Format(format, args));

        public Task InfoAsync(string format, string category, params object[] args)
            => InfoAsync(string.Format(format, args), category);

        #endregion Info functions

        #region Warning functions

        public Task WarnAsync(string message, string category)
            => WriteLogAsync(message, category, TraceEventType.Warning);

        public Task WarnAsync(string message)
            => WarnAsync(message, defaultLogCategory);

        public Task WarnAsync(string format, params object[] args)
            => WarnAsync(string.Format(format, args));

        public Task WarnAsync(string format, string category, params object[] args)
            => WarnAsync(string.Format(format, args), category);

        #endregion Warning functions

        #region Error functions

        public Task ErrorAsync(string message, string category)
            => WriteLogAsync(message, category, TraceEventType.Error);

        public Task ErrorAsync(string message)
            => ErrorAsync(message, defaultLogCategory);

        public Task ErrorAsync(string message, Exception exception)
            => ErrorAsync(message, defaultLogCategory, exception);

        public Task ErrorAsync(string message, string category, Exception exception)
            => ErrorAsync(string.Format("{0}\n{1}", message, exception), category);

        #endregion Error functions

        #region Critical

        public Task CriticalAsync(string message, string category)
            => WriteLogAsync(message, category, TraceEventType.Critical);

        public Task CriticalAsync(string message)
            => CriticalAsync(message, defaultLogCategory);

        public Task CriticalAsync(string format, params object[] args)
            => CriticalAsync(string.Format(format, args));

        public Task CriticalAsync(string format, string category, params object[] args)
            => CriticalAsync(string.Format(format, args), category);

        #endregion Critical

        public Task WriteLogAsync(string message, string category, TraceEventType traceEventType)
        {
            var writeLogTasks = logEngines.Select(
                            logEngine => logEngine.WriteAsync(
                                        message,
                                        GetSeverity(traceEventType),
                                        string.IsNullOrEmpty(category)
                                            ? defaultLogCategory
                                            : category));

#if NETSTANDARD2_0 || NET461 || NET5_0_OR_GREATER
            return Task.WhenAll(writeLogTasks);
#else
#pragma warning disable S4462 // Calls to "async" methods should not be blocking
            Task.WaitAll(writeLogTasks.ToArray());
#pragma warning restore S4462 // Calls to "async" methods should not be blocking

            return FromResult(0);
#endif


        }

#if NET40
        public static Task<T> FromResult<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }
#endif
    }
}