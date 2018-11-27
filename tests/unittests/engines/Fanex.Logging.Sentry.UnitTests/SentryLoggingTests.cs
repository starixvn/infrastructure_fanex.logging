namespace Fanex.Logging.Sentry.UnitTests
{
    using System;
    using System.Diagnostics;
    using global::Sentry;
    using global::Sentry.Protocol;
    using NSubstitute;
    using Xunit;

    [Trait("Unit Tests", "Sentry")]
    public class SentryLoggingTests
    {
        private readonly SentryLogging sentryLogging;
        private readonly Action<SentryOptions, string, string, SentryLevel> captureMessageAction;
        private readonly SentryEngineOptions sentryOptions = new SentryEngineOptions
        {
            Dsn = new Dsn("http://21a8f21f7c5a4cc1b9c79bb68fb0ebb9@sentry.nexdev.net/3")
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

        [Fact]
        public void RenderJavascriptLogger_Always_ReturnSentryJavascript()
        {
            var sentryJs = sentryLogging.RenderJavascriptLogger();

            var expectedSentryJs = 
                "<script src=\"es6-script\" crossorigin=\"anonymous\"></script><script src=\"sentry-script\" crossorigin=\"anonymous\"></script><script type=\"text/javascript\">var option = {"               
                  + "  \"frontEndDsn\": null,"
                  + "  \"sendDefaultPii\": false,"
                  + "  \"attachStacktrace\": false,"
                  + "  \"maxBreadcrumbs\": 100,"
                  + "  \"sampleRate\": null,"
                  + "  \"release\": null,"
                  + "  \"environment\": null,"
                  + "  \"dsn\": {"
                  + "    \"projectId\": \"3\","
                  + "    \"path\": \"\","
                  + "    \"secretKey\": null,"
                  + "    \"publicKey\": \"21a8f21f7c5a4cc1b9c79bb68fb0ebb9\","
                  + "    \"sentryUri\": \"http://sentry.nexdev.net:80/api/3/store/\""
                  + "  },"
                  + "  \"beforeSend\": null,"
                  + "  \"beforeBreadcrumb\": null,"
                  + "  \"maxQueueItems\": 30,"
                  + "  \"shutdownTimeout\": \"00:00:02\","
                  + "  \"decompressionMethods\": -1,"
                  + "  \"requestBodyCompressionLevel\": 0,"
                  + "  \"requestBodyCompressionBuffered\": true,"
                  + "  \"httpProxy\": null,"
                  + "  \"configureHandler\": null,"
                  + "  \"configureClient\": null,"
                  + "  \"debug\": false,"
                  + "  \"diagnosticsLevel\": 0,"
                  + "  \"diagnosticLogger\": null"
                  + "};option.dsn=\"http://21a8f21f7c5a4cc1b9c79bb68fb0ebb9@sentry.nexdev.net/3\";option.onerror=true;option.onunhandledrejection=true;Sentry.init(option);</script>";

            Assert.Equal(expectedSentryJs, sentryJs.Replace("\r\n", string.Empty));
        }

        [Fact]
        public void RenderJavascriptLogger_WithFrontEndDsn_ReturnSentryJavascriptWithFrontEndDns()
        {
            sentryLogging.GetSentryOptions.FrontEndDsn = new Dsn("http://112a8f21f7c5a4cc1b9c79bb68fb0ebb9@sentry.nexdev.net/3");

            var sentryJs = sentryLogging.RenderJavascriptLogger();

            var expectedSentryJs =
                "<script src=\"es6-script\" crossorigin=\"anonymous\"></script><script src=\"sentry-script\" crossorigin=\"anonymous\"></script><script type=\"text/javascript\">var option = {"
                  + "  \"frontEndDsn\": {"
                  + "    \"projectId\": \"3\","
                  + "    \"path\": \"\","
                  + "    \"secretKey\": null,"
                  + "    \"publicKey\": \"112a8f21f7c5a4cc1b9c79bb68fb0ebb9\","
                  + "    \"sentryUri\": \"http://sentry.nexdev.net:80/api/3/store/\""
                  + "  },"
                  + "  \"sendDefaultPii\": false,"
                  + "  \"attachStacktrace\": false,"
                  + "  \"maxBreadcrumbs\": 100,"
                  + "  \"sampleRate\": null,"
                  + "  \"release\": null,"
                  + "  \"environment\": null,"
                  + "  \"dsn\": {"
                  + "    \"projectId\": \"3\","
                  + "    \"path\": \"\","
                  + "    \"secretKey\": null,"
                  + "    \"publicKey\": \"21a8f21f7c5a4cc1b9c79bb68fb0ebb9\","
                  + "    \"sentryUri\": \"http://sentry.nexdev.net:80/api/3/store/\""
                  + "  },"
                  + "  \"beforeSend\": null,"
                  + "  \"beforeBreadcrumb\": null,"
                  + "  \"maxQueueItems\": 30,"
                  + "  \"shutdownTimeout\": \"00:00:02\","
                  + "  \"decompressionMethods\": -1,"
                  + "  \"requestBodyCompressionLevel\": 0,"
                  + "  \"requestBodyCompressionBuffered\": true,"
                  + "  \"httpProxy\": null,"
                  + "  \"configureHandler\": null,"
                  + "  \"configureClient\": null,"
                  + "  \"debug\": false,"
                  + "  \"diagnosticsLevel\": 0,"
                  + "  \"diagnosticLogger\": null"
                  + "};option.dsn=\"http://112a8f21f7c5a4cc1b9c79bb68fb0ebb9@sentry.nexdev.net/3\";option.onerror=true;option.onunhandledrejection=true;Sentry.init(option);</script>";

            Assert.Equal(expectedSentryJs, sentryJs.Replace("\r\n", string.Empty));
        }
    }
}