[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Sentry.UnitTests")]

namespace Fanex.Logging.Sentry
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using global::Sentry;
    using global::Sentry.Protocol;

    public class SentryLogging : ILogEngine
    {
        private readonly SentryOptions sentryOptions;
        private readonly Action<SentryOptions, string, string, SentryLevel> captureMessageAction;

        public SentryLogging(SentryOptions sentryOptions)
            : this(sentryOptions, null)
        {
        }

        internal SentryLogging(
            SentryOptions sentryOptions,
            Action<SentryOptions, string, string, SentryLevel> captureMessageAction)
        {
            this.sentryOptions = sentryOptions ?? throw new ArgumentNullException(nameof(sentryOptions));
            this.captureMessageAction = captureMessageAction ?? CaptureMessage;
        }

        internal SentryOptions GetSentryOptions
            => sentryOptions
                ?? throw new InvalidOperationException("Please init Fanex.Logging for Sentry before using.");

        public void Write(string message, TraceEventType severity, string category)
            => captureMessageAction(sentryOptions, message, category, GetLogLevel(severity));

        private static void CaptureMessage(
            SentryOptions sentryOptions,
            string message,
            string category,
            SentryLevel sentryLevel)
        {
            using (SentrySdk.Init(sentryOptions))
            {
                SentrySdk.ConfigureScope(scope => scope.Environment = category);
                SentrySdk.CaptureMessage(message, sentryLevel);
            }
        }

        private static SentryLevel GetLogLevel(TraceEventType severity)
        {
            switch (severity)
            {
                case TraceEventType.Information:
                    return SentryLevel.Info;

                case TraceEventType.Warning:
                    return SentryLevel.Warning;

                case TraceEventType.Error:
                    return SentryLevel.Error;

                case TraceEventType.Critical:
                    return SentryLevel.Fatal;

                case TraceEventType.Verbose:
                    return SentryLevel.Debug;

                default:
                    return SentryLevel.Error;
            }
        }

        public Task WriteAsync(string message, TraceEventType severity, string category)
        {
            Write(message, severity, category);

            return Task.FromResult(0);
        }
    }
}