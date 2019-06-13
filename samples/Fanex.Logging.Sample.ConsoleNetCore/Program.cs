using System;
using System.Collections.Generic;
using Fanex.Logging.Sentry;
using Sentry;
using Sentry.Protocol;

namespace Fanex.Logging.Sample.ConsoleNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager
                   .SetDefaultLogCategory("Fanex-Logging-Test-Web-new")
                   .Use(new List<ILogEngine>
                   {
                       //new RabbitMQLogging("host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw=="),
                       new SentryLogging(new SentryEngineOptions
                       {
                           Dsn = new Dsn("https://dd81659b4bdb4857b023ef046b212844@sentry.nexdev.net/5"),
                           Release = "Release-2018-10-25",
                           GetUserInfo = () => new User
                           {
                               Username = "ricky.nguyen2"
                           }
                       })
                   });

            LogService.Error("test message 1");
        }
    }
}
