namespace Fanex.Logging.Samples.DatabaseV59
{
    using System;
    using System.Diagnostics;
    using Data;
    using Data.Configuration;
    using Extensions;
    using Extensions.Database.V59;
    using Logging.Extensions.Database.V59;

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
            var logMessage = new LogMessage(exception.Message)
                                .AddSection("DATABASE", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                                .AddSection("WEB SERVER", CommonInfoGetters.GetServerInfo())
                                .AddSection("EXCEPTION", CommonInfoGetters.GetExceptionInfo(exception))
                                .ToString();

            LogService.Error(logMessage);
            Console.WriteLine(logMessage);
        }

        public static void SimulateSqlException()
        {
            DbSettingProviderManager.Current.Start();

            using (var db = new ObjectDb("Logging_UpdateMachine"))
            {
                try
                {
                    db.ExecuteNonQuery(new
                    {
                        MachineIP = "45454 1 1",
                        Timezone = "GMT8 2323 "
                    });
                }
                catch (Exception ex)
                {
                    throw new DatabaseException(ex.Message, ex, db.DbSetting);
                }
            }
        }
    }
}