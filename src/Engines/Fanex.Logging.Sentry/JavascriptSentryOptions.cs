namespace Fanex.Logging.Sentry
{
    using System;
    using System.IO.Compression;
    using System.Net;
    using global::Sentry;
    using global::Sentry.Extensibility;
    using global::Sentry.Protocol;

    public class JavascriptSentryOptions
    {
        public bool Debug { get; set; }

        public bool RequestBodyCompressionBuffered { get; set; }

        public CompressionLevel RequestBodyCompressionLevel { get; set; }

        public DecompressionMethods DecompressionMethods { get; set; }

        public TimeSpan ShutdownTimeout { get; set; }

        public SentryLevel DiagnosticsLevel { get; set; }

        public int MaxQueueItems { get; set; }

        public string Dsn { get; set; }

        public string Environment { get; set; }

        public string Release { get; set; }

        public float? SampleRate { get; set; }

        public int MaxBreadcrumbs { get; set; }

        public bool AttachStacktrace { get; set; }

        public bool SendDefaultPii { get; set; }

        public IDiagnosticLogger DiagnosticLogger { get; set; }
    }
}