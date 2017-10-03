namespace ConsoleLogging
{
    using System;
    using System.Threading;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //TestLogging();
                SimulateSqlException();
            }
            catch (Exception exception)
            {
                var logMessage = new LogMessage(exception.Message)
                                .AddSection("DATABASE", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                                .AddSection("WEB SERVER", CommonInfoGetters.GetServerInfo())
                                .AddSection("EXCEPTION", CommonInfoGetters.GetExceptionInfo(exception))
                                .ToString();

                LogService.Error(logMessage);
                LogService.Warn(logMessage);
                LogService.Debug(logMessage);
                LogService.Critical(logMessage);
                LogService.Info(logMessage);

                LogService.Info(logMessage, "Alpha-MBC-AF-LOCAL");

                Console.WriteLine(logMessage);
            }

            Console.ReadLine();
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

            Thread.Sleep(130 * 1000);

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