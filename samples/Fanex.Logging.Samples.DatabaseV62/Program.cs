namespace Fanex.Logging.Samples.DatabaseV62
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Timers;
    using Data;
    using Data.Configuration;
    using Extensions;
    using Extensions.Database;
    using Fanex.Logging.RabbitMQ;
    using Fanex.Logging.Sentry.RavenClient;
    using SharpRaven;

    public static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                LogManager
                    .SetDefaultLogCategory("Fanex.Logging.Tests")
                    .Use(new List<ILogEngine>
                    {
                        new RabbitMQLogging("host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw=="),
                        new SentryLogging(new SentryEngineOptions
                        {
                            Dsn = new Dsn("http://cedc74b2ae244da599820b107361e389@sentry.nexdev.net/3"),
                            //GetUserInfo = () => new SharpRaven.Data.SentryUser("ricky.test.1")
                        }),
                        //new EntlibLogging(new EntLibConfig(
                        //        @"FormatName:DIRECT=tcp:10.18.200.103\private$\entlib",
                        //        @"~\Logs\"))
                    });
                //.Use(new RabbitMQLogging("host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw=="));

                for (int i = 0; i < 1000; i++)
                {
                    CalculateLoggingSpeed();
                }
                
                //SimulateSqlException();
                Console.WriteLine("-----------DONE-----------");
            }
            catch (Exception exception)
            {
                Handle(exception);
            }

            Console.ReadLine();
        }

        private static void CalculateLoggingSpeed()
        {
            var watch = Stopwatch.StartNew();

            LogService.Error("test message");

            watch.Stop();

            Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
        }

        public static void SimulateSqlException()
        {
            DbSettingProviderManager.Current.Start();

            using (var db = new ObjectDb("Logging_UpdateMachine"))
            {
                db.ExecuteNonQuery(new
                {
                    MachineIP = "45454 1 1 22",
                    Timezone = "GMT8 2323 "
                });
            }
        }

        public static void Handle(Exception exception)
        {
            var logMessage = new LogMessage(exception.Message)
                                .AddSection("DATABASE", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                                .AddSection("WEB SERVER", CommonInfoGetters.GetServerInfo())
                                .AddSection("EXCEPTION", CommonInfoGetters.GetExceptionInfo(exception))
                                .ToString();

            LogService.Error(logMessage);
            Console.WriteLine(logMessage);
        }
    }
}