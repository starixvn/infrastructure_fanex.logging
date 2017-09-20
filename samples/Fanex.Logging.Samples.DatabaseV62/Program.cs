namespace Fanex.Logging.Samples.DatabaseV62
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Data.Configuration;
    using Extensions.Database.V62;

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
                var errorInfo = new LogEntry(exception.Message)
                               .AddSqlInfo(exception)
                               .AddWebServerInfo()
                               .AddExceptionInfo(exception)
                               .AddInfo(
                                   LogUtility.CombineInfo(
                                       "INFO GROUP",
                                        "Info 1",
                                        "Info 2"))
                                       .ToString();

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
