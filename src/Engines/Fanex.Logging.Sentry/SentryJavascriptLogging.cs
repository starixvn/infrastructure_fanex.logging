namespace Fanex.Logging.Sentry
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public static class SentryJavascriptLogging
    {
#pragma warning disable S3994 // URI Parameters should not be strings

        public static string Render(string sentryScriptUrl = "https://browser.sentry-cdn.com/4.2.3/bundle.min.js")
#pragma warning restore S3994 // URI Parameters should not be strings
        {
            var sentryEngine =
                LogManager.Instance.LogEngines.FirstOrDefault(logEngine => logEngine is SentryLogging)
                ?? throw new InvalidOperationException("Please init Fanex.Logging for Sentry before using.");
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
            var sentryOptions = (sentryEngine as SentryLogging).GetSentryOptions;
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

            return string.Join(
                string.Empty,
                $"<script src=\"{sentryScriptUrl}\" crossorigin=\"anonymous\"></script>",
                $"<script type=\"text/javascript\"> var option = {JsonConvert.SerializeObject(sentryOptions, jsonSerializerSettings)};",
                $"option.dsn=\"{sentryOptions.Dsn}\";",
                $"option.onerror=true;",
                $"option.onunhandledrejection=true;",
                $"Sentry.init(option);",
                "</script>");
        }
    }
}