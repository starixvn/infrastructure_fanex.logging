namespace Fanex.Logging.Sentry
{
    using global::Sentry;

    public class SentryEngineOptions : SentryOptions
    {
        public Dsn FrontEndDns { get; set; }
    }
}