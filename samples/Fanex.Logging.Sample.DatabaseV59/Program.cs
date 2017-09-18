namespace Fanex.Logging.Sample.DatabaseV59
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Data.Configuration;
    using Extensions.Database.V59;

    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
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

        private static void SimulateSqlException()
        {
            DbSettingProviderManager.Current.Start();

            using (var db = new ObjectDb("Logging_UpdateMachine"))
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