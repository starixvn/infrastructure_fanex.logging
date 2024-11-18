namespace ConsoleApp;

using System.Net;
using Fanex.Logging;
using Fanex.Logging.Sentry;

public static class TestSentry
{
    public static HttpMessageHandler CreateHttpMessageHandler()
    {
        var handler = new HttpClientHandler
        {
            CookieContainer = new CookieContainer(),
            UseCookies = true
        };

        return handler;
    }
    public static void Action()
    {

        LogManager
            .SetDefaultLogCategory("Test")
            .Use(new SentryLogging(new SentryEngineOptions
            {
                Dsn = "http://38a50130b98c4bd8a70b071774b3c26c@sentry.nexdev.net/119",
                Debug = true,
                AutoSessionTracking = true,
                IsGlobalModeEnabled = false,
                TracesSampleRate = 0.1,
                CreateHttpMessageHandler = CreateHttpMessageHandler
            }));

        Logger.Log.Info("Test" + Guid.NewGuid());
    }
}