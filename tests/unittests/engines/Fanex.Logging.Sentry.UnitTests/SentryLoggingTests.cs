namespace Fanex.Logging.Sentry.UnitTests;

using System;
using System.Diagnostics;
using global::Sentry;
using NSubstitute;
using Xunit;

[Trait("Unit Tests", "Sentry")]
public class SentryLoggingTests
{
    private readonly SentryLogging sentryLogging;
    private readonly Action<SentryOptions, string, string, SentryLevel> captureMessageAction;
    private readonly SentryEngineOptions sentryOptions = new()
    {
        Dsn = "http://21a8f21f7c5a4cc1b9c79bb68fb0ebb9@sentry.nexdev.net/3"
    };

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
    public void Write_Always_ExecuteCaptureMessageAction(TraceEventType actualSeverity, SentryLevel expectedSentryLevel)
    {
        const string message = "message";
        const string category = "category";

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
    public void WriteAsync_Always_ExecuteCaptureMessageAction(TraceEventType actualSeverity, SentryLevel expectedSentryLevel)
    {
        const string message = "message";
        const string category = "category";

        sentryLogging.WriteAsync(message, actualSeverity, category).GetAwaiter().GetResult();

        captureMessageAction.Received().Invoke(
            sentryOptions,
            message,
            category,
            expectedSentryLevel);
    }

    [Fact]
    public void RenderJavascriptLogger_Always_ReturnSentryJavascript()
    {
        var sentryJs = sentryLogging.RenderJavascriptLogger();

        const string expectedSentryJs = "<script src=\"es6-script\" crossorigin=\"anonymous\"></script>";

        Assert.Contains(expectedSentryJs, sentryJs.Replace("\r\n", string.Empty));
    }

    [Fact]
    public void RenderJavascriptLogger_WithFrontEndDsn_ReturnSentryJavascriptWithFrontEndDns()
    {
        sentryLogging.GetSentryOptions.FrontEndDsn = "http://112a8f21f7c5a4cc1b9c79bb68fb0ebb9@sentry.nexdev.net/3";

        var sentryJs = sentryLogging.RenderJavascriptLogger();

        const string expectedSentryJs = "<script src=\"es6-script\" crossorigin=\"anonymous\"></script>";

        Assert.Contains(expectedSentryJs, sentryJs.Replace("\r\n", string.Empty));
    }
}