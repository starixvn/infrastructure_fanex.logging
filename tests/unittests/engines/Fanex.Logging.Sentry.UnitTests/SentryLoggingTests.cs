namespace Fanex.Logging.Sentry.UnitTests
{
    using System;
    using System.Diagnostics;
    using global::Sentry;
    using global::Sentry.Protocol;
    using NSubstitute;
    using Xunit;

    public class SentryLoggingTests
    {
        private readonly SentryLogging sentryLogging;
        private readonly Action<SentryOptions, string, string, SentryLevel> captureMessageAction;
        private SentryEngineOptions sentryOptions = new SentryEngineOptions();

        public SentryLoggingTests()
        {
            captureMessageAction = Substitute.For<Action<SentryOptions, string, string, SentryLevel>>();
            sentryLogging = new SentryLogging(sentryOptions, captureMessageAction, "sentry-script", "es6-script");
        }

        [Theory]
        [InlineData(TraceEventType.Information, SentryLevel.Info)]
        [InlineData(TraceEventType.Warning, SentryLevel.Warning)]
        [InlineData(TraceEventType.Error, SentryLevel.Error)]
        [InlineData(TraceEventType.Critical, SentryLevel.Fatal)]
        [InlineData(TraceEventType.Verbose, SentryLevel.Debug)]
        [InlineData(TraceEventType.Suspend, SentryLevel.Error)]
        public void Write_Always_ExecutureCaptureMessageAction(TraceEventType actualSeverity, SentryLevel expectedSentryLevel)
        {
            var message = "message";
            var category = "category";

            sentryLogging.Write(message, actualSeverity, category);

            captureMessageAction.Received().Invoke(
                sentryOptions,
                message,
                category,
                expectedSentryLevel);
        }

        [Theory]
        [InlineData(TraceEventType.Information, SentryLevel.Info)]
        [InlineData(TraceEventType.Warning, SentryLevel.Warning)]
        [InlineData(TraceEventType.Error, SentryLevel.Error)]
        [InlineData(TraceEventType.Critical, SentryLevel.Fatal)]
        [InlineData(TraceEventType.Verbose, SentryLevel.Debug)]
        [InlineData(TraceEventType.Suspend, SentryLevel.Error)]
        public void WriteAsync_Always_ExecutureCaptureMessageAction(TraceEventType actualSeverity, SentryLevel expectedSentryLevel)
        {
            var message = "message";
            var category = "category";

            sentryLogging.WriteAsync(message, actualSeverity, category).GetAwaiter().GetResult();

            captureMessageAction.Received().Invoke(
                sentryOptions,
                message,
                category,
                expectedSentryLevel);
        }
    }
}