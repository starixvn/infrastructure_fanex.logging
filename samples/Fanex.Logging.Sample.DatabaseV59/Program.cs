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
                var errorInfo = exception
                                .Message
                                .AddSqlInfo(exception)
                                .AddWebServerInfo()
                                .AddExceptionInfo(exception)
                                .AddCustomInfo(
                                    "INFO GROUP",
                                    "Info 1",
                                    "Info 2");

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