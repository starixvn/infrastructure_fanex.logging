namespace Fanex.Logging.SampleApp40
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Sentry.RavenClient;
    using SharpRaven;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                LogManager
                    .SetDefaultLogCategory("Fanex.Logging.Tests")
                    .Use(new List<ILogEngine>
                    {
                        //new RabbitMQLogging("host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw=="),
                        new SentryLogging(new SentryEngineOptions
                        {
                            Dsn = new Dsn(ConfigurationManager.AppSettings["sentryurl"]),
                            //GetUserInfo = () => new SharpRaven.Data.SentryUser("ricky.test.1")
                        }),
                        //new EntlibLogging(new EntLibConfig(
                        //        @"FormatName:DIRECT=tcp:10.18.200.103\private$\entlib",
                        //        @"~\Logs\"))
                    });
                //.Use(new RabbitMQLogging("host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw=="));
                //LogService.Error("test message");
                for (int i = 0; i < 20; i++)
                {
                    if(i % 3 == 0)
                    {
                        Thread.Sleep(2000);
                    }

                    Task.Factory.StartNew(WriteLog, i);
                }
            }
            catch (Exception exception)
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var logMessage = new LogMessage(exception.Message)
                                //.AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                                .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                                .AddSection("Exception", CommonInfoGetters.GetExceptionInfo(exception))
                                .ToString();

                LogService.Error(logMessage);
                LogService.Warn(logMessage);
                LogService.Debug(logMessage);
                LogService.Critical(logMessage);
                LogService.Info(logMessage);

                LogService.Info(logMessage, "Fanex-Logging-Test1");

                LogService.Error(logMessage);
                LogService.Warn(logMessage);
                LogService.Debug(logMessage);
                LogService.Critical(logMessage);
                LogService.Info(logMessage);

                LogService.Error(logMessage);
                LogService.Warn(logMessage);
                LogService.Debug(logMessage);
                LogService.Critical(logMessage);
                LogService.Info(logMessage);

                LogService.Info(logMessage, "Fanex-Logging-Test1");

                //var logMessageAsyncMessage = new LogMessage($"Async {exception.Message}")
                //                .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                //                .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                //                .AddSection("Exception", CommonInfoGetters.GetExceptionInfo(exception))
                //                .ToString();

                //LogService.ErrorAsync(logMessageAsyncMessage);
                //LogService.WarnAsync(logMessageAsyncMessage);
                //LogService.DebugAsync(logMessageAsyncMessage);
                //LogService.CriticalAsync(logMessageAsyncMessage);
                //LogService.InfoAsync(logMessageAsyncMessage);

                //LogService.InfoAsync(logMessageAsyncMessage, "Fanex-Logging-Test1");

                //Console.WriteLine(logMessageAsyncMessage);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static void WriteLog(object index)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            LogService.Error("test message");
            //SimulateSqlException();
            stopwatch.Stop();

            Console.WriteLine($"Write log {(int)index} completed at {stopwatch.ElapsedMilliseconds}");
        }

        private static void TestLogging()
        {
            for (int i = 0; i < 2; i++)
            {
                LogService.Error("Test logging.", new Exception("Demo Exception"));
                LogService.Warn("Test warning");
                LogService.Info("Test info");
            }

            LogService.Error("Test logging1.", new Exception("Demo Exception"));
            LogService.Warn("Test warning1");
            LogService.Info("Test info1");

            //Thread.Sleep(130 * 1000);

            Console.WriteLine("Second time");

            LogService.Error("Test logging2", new Exception("Demo Exception"));
            LogService.Warn("Test warning2");
            LogService.Info("Test info2");
        }

        private static void SimulateSqlException()
        {
            // Fanex.Data version 1
            //DbSettingProviderManager.Current.Start();

            // Fanex.Data version 2
            DbSettingProviderManager
                .StartNewSession()
                .Use(new OldConnectionStringProvider())
                .UseDefaultDbSettingProvider(
                    resourcePath: "App_Data/")
                .Use((settingKey, commandTimeout) => new WrappingExceptionObjectDb(settingKey, commandTimeout))
                .Run();

            using (var db = ObjectDbFactory.CreateInstance("Logging_UpdateMachine"))
            {
                db.ExecuteNonQuery(new
                {
                    MachineIP = "45454",
                    Timezone = "GMT8"
                });
            }
        }
    }
}
