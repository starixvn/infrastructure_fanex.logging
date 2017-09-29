namespace Fanex.Logging.Samples.DatabaseV62
{
    using System;
    using Data;
    using Data.Configuration;
    using Extensions.Database.V62;

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
            //var errorInfo = new LogEntry(exception.Message)
            //                   .AddSqlInfo(exception)
            //                   .AddServerInfo()
            //                   .AddExceptionInfo(exception)
            //                   .AddInfo(
            //                       LogUtility.CombineInfo(
            //                           "INFO GROUP",
            //                            "Info 1",
            //                            "Info 2"))
            //                           .ToString();

            //LogService.Error(errorInfo);
            //Console.WriteLine(errorInfo);
        }
    }
}