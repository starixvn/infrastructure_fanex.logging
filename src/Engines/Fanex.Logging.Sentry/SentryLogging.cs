[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Sentry.UnitTests")]

namespace Fanex.Logging.Sentry;

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using global::Sentry;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class SentryLogging : ILogEngine
{
    private const int WriteLogTimeoutSeconds = 1;
    private const string SentryScriptDefault = "https://browser.sentry-cdn.com/4.3.2/bundle.min.js";
    private const string Es6PromiseScriptDefault = "https://cdnjs.cloudflare.com/ajax/libs/es6-promise/4.1.1/es6-promise.auto.min.js";
    private readonly SentryEngineOptions sentryOptions;
    private readonly string sentryScript;
    private readonly string es6PromiseScript;
    private readonly Action<SentryEngineOptions, string, string, SentryLevel> captureMessageAction;
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        Formatting = Formatting.Indented,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        MaxDepth = 1
    };

    public SentryLogging(
        SentryEngineOptions sentryOptions,
        string sentryScript = SentryScriptDefault,
        string es6PromiseScript = Es6PromiseScriptDefault)
        : this(sentryOptions, null, sentryScript, es6PromiseScript)
    {
    }

    internal SentryLogging(
        SentryEngineOptions sentryOptions,
        Action<SentryEngineOptions, string, string, SentryLevel> captureMessageAction,
        string sentryScript,
        string es6PromiseScript)
    {
        this.sentryOptions = sentryOptions ?? throw new ArgumentNullException(nameof(sentryOptions));
        this.captureMessageAction = captureMessageAction ?? CaptureMessage;
        this.sentryScript = sentryScript;
        this.es6PromiseScript = es6PromiseScript;
    }

    private static void DefaultConfigClient(HttpClient httpClient )
    {
        httpClient.Timeout = TimeSpan.FromSeconds(WriteLogTimeoutSeconds);
    }

    internal SentryEngineOptions GetSentryOptions
        => sentryOptions
           ?? throw new InvalidOperationException("Please init Fanex.Logging for Sentry before using.");

    public void Write(string message, TraceEventType severity, string category)
        => captureMessageAction(sentryOptions, message, category, GetLogLevel(severity));

    private static void CaptureMessage(
        SentryEngineOptions sentryOptions,
        string message,
        string category,
        SentryLevel sentryLevel)
    {
        if (sentryOptions.ConfigureClient == null)
        {
            sentryOptions.ConfigureClient = DefaultConfigClient;
        }
        else
        {
            var customConfigureClient = sentryOptions.ConfigureClient;

            sentryOptions.ConfigureClient = httpclient =>
            {
                DefaultConfigClient(httpclient);
                customConfigureClient(httpclient);
            };
        }

        using (SentrySdk.Init(sentryOptions))
        {
            SentrySdk.ConfigureScope(scope =>
            {
                scope.Environment = category;
                scope.User = sentryOptions.GetUserInfo?.Invoke();
            });
            SentrySdk.CaptureMessage(message, sentryLevel);
        }
    }

    private static SentryLevel GetLogLevel(TraceEventType severity)
    {
        return severity switch
        {
            TraceEventType.Information => SentryLevel.Info,
            TraceEventType.Warning => SentryLevel.Warning,
            TraceEventType.Error => SentryLevel.Error,
            TraceEventType.Critical => SentryLevel.Fatal,
            TraceEventType.Verbose => SentryLevel.Debug,
            _ => SentryLevel.Error
        };
    }

    public Task WriteAsync(string message, TraceEventType severity, string category)
    {
        Write(message, severity, category);

        return Task.FromResult(0);
    }

    public string RenderJavascriptLogger()
    {
        var backEndSentryOptions = GetSentryOptions;
        var frontEndSentryOptions = new JavascriptSentryOptions
        {
            AttachStacktrace = backEndSentryOptions.AttachStacktrace,
            Debug = backEndSentryOptions.Debug,
            DecompressionMethods = backEndSentryOptions.DecompressionMethods,
            DiagnosticLogger = backEndSentryOptions.DiagnosticLogger,
            DiagnosticsLevel = backEndSentryOptions.DiagnosticLevel,
            Environment = backEndSentryOptions.Environment,
            MaxBreadcrumbs = backEndSentryOptions.MaxBreadcrumbs,
            MaxQueueItems = backEndSentryOptions.MaxQueueItems,
            Release = backEndSentryOptions.Release,
            RequestBodyCompressionBuffered = backEndSentryOptions.RequestBodyCompressionBuffered,
            RequestBodyCompressionLevel = backEndSentryOptions.RequestBodyCompressionLevel,
            SampleRate = backEndSentryOptions.SampleRate,
            SendDefaultPii = backEndSentryOptions.SendDefaultPii,
            ShutdownTimeout = backEndSentryOptions.ShutdownTimeout,
            Dsn = backEndSentryOptions.FrontEndDsn ?? backEndSentryOptions.Dsn
        };

        return string.Join(
            string.Empty,
            string.IsNullOrWhiteSpace(es6PromiseScript)
                ? string.Empty
                : $"<script src=\"{es6PromiseScript}\" crossorigin=\"anonymous\"></script>",
            string.IsNullOrWhiteSpace(sentryScript)
                ? string.Empty
                : $"<script src=\"{sentryScript}\" crossorigin=\"anonymous\"></script>",
            "<script type=\"text/javascript\">",
            $"var option = {JsonConvert.SerializeObject(frontEndSentryOptions, JsonSerializerSettings)};",
            $"option.dsn=\"{frontEndSentryOptions.Dsn}\";",
            "option.onerror=true;",
            "option.onunhandledrejection=true;",
            "Sentry.init(option);",
            "</script>");
    }
}