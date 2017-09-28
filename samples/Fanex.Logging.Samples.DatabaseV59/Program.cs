namespace Fanex.Logging.Samples.DatabaseV59
{
    using System;
    using Data;
    using Data.Configuration;
    using Extensions.Database.V59;

    public static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                SimulateSqlException();
            }
            catch (Exception exception)
            {
                Handle(exception);
            }

            Console.ReadLine();
        }

        public static void Handle(Exception exception)
        {
            var errorInfo = new LogData(exception.Message)
                               .AddSqlInfo(exception)
                               .AddWebServerInfo()
                               .AddExceptionInfo(exception)
                               .AddInfo(
                                   LogUtility.CombineInfo(
                                       "INFO GROUP1",
                                        "Info 2",
                                        "Info 3"))
                                       .ToString();

            LogService.Error(errorInfo);
            Console.WriteLine(errorInfo);
        }

        public static void SimulateSqlException()
        {
            DbSettingProviderManager.Current.Start();

            using (var db = new ObjectDb("Logging_UpdateMachine"))
            {
                db.ExecuteNonQuery(new
                {
                    MachineIP = "45454 1 1",
                    Timezone = "GMT8 2323 "
                });
            }
        }
    }
}