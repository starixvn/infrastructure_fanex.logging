namespace Fanex.Logging.Sentry;

using System;
using global::Sentry;
using Newtonsoft.Json;

public class SentryEngineOptions : SentryOptions
{
    public string FrontEndDsn { get; set; }

    [JsonIgnore]
    public Func<SentryUser> GetUserInfo { get; set; }
}