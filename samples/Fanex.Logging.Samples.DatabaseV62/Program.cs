namespace Fanex.Logging.Samples.DatabaseV62
{
    using System;
    using System.Diagnostics;
    using Data;
    using Data.Configuration;
    using Extensions;
    using Extensions.Database;

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