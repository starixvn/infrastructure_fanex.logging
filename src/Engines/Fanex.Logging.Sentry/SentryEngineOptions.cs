namespace Fanex.Logging.Sentry
{
    using System;
    using global::Sentry;
    using global::Sentry.Protocol;
    using Newtonsoft.Json;

    public class SentryEngineOptions : SentryOptions
    {
        public Dsn FrontEndDsn { get; set; }

        [JsonIgnore]
        public Func<User> GetUserInfo { get; set; }
    }
}