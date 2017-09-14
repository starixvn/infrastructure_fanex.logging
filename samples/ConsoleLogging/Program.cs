namespace ConsoleLogging
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using Fanex.Logging.DatabaseExtensions;

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
            catch (Exception ex)
            {
                var customLogSection = LogUtility.CombineInfo(
                                            "INFO GROUP",
                                            "Info 1",
                                            "Info 2");

                var logMessage = LogUtility.BuildLogInfo(
                    ex.Message,
                    ex,
                    new List<Func<Exception, string>>
                    {
                        DatabaseLoggingUtility.BuildSqlInfo
                    },
                    customLogSection);

                Console.WriteLine(logMessage);
                LogService.Error(logMessage);
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
            DbSettingProviderManager.Current.Start();

            using (var db = new ObjectDb("Logging_UpdateMachine"))
            {
                db.ExecuteNonQuery();
            }
        }
    }
}