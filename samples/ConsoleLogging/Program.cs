namespace ConsoleLogging
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging;
    using Fanex.Logging.Extensions.Database;

    /// <summary>
    /// The testing application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                //TestLogging();
                SimulateSqlException();
            }
            catch (Exception exception)
            {
                var errorInfo = LogUtility.BuildLogInfo(
                    exception,
                    ex => ex.Message,
                    DatabaseLoggingUtility.BuildSqlInfo,
                    ex => LogUtility.CombineInfo(
                                            "INFO GROUP",
                                            "Info 1",
                                            "Info 2") // Custom Info
                                            );

                LogService.Error(errorInfo);
                Console.WriteLine(errorInfo);
            }

            Console.ReadLine();
        }

        private static void TestLogging()
        {
            ////for (int i = 0; i < 2; i++)
            ////{
            ////    LogService.Error("Test logging.", new Exception("Demo Exception"));
            ////    LogService.Warn("Test warning");
            ////    LogService.Info("Test info");
            ////}

            ////LogService.Error("Test logging1.", new Exception("Demo Exception"));
            ////LogService.Warn("Test warning1");
            ////LogService.Info("Test info1");

            ////Thread.Sleep(130 * 1000);

            ////Console.WriteLine("Second time");

            ////LogService.Error("Test logging.", new Exception("Demo Exception"));
            ////LogService.Warn("Test warning");
            ////LogService.Info("Test info");
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