namespace ConsoleApp;

using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;

public static class TestRaven
{
    public static void Action()
    {

        LogManager
            .SetDefaultLogCategory("Test")
            .Use(new SentryLogging(new SentryEngineOptions
            {
                Dsn = new Dsn("http://38a50130b98c4bd8a70b071774b3c26c@sentry.nexdev.net/119")
            }));

        Logger.Log.Info("Test " + Guid.NewGuid());
    }
}
