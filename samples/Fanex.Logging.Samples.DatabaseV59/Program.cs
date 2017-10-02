namespace Fanex.Logging.Samples.DatabaseV59
{
    using System;
    using System.Diagnostics;
    using Data;
    using Data.Configuration;
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
            var errorInfo = new LogEntry(exception.Message)
                                .Section("DATABASE")
                                    .AddContent(DatabaseLogContentExtractors.GetDbExceptionInfo(exception))
                                .Section("WEB SERVER")
                                    .AddContent(LogContentExtractors.GetServerInfo())
                                .Section("EXCEPTION")
                                    .AddContent(LogContentExtractors.GetExceptionInfo(exception));

            LogService.Write(errorInfo, TraceEventType.Error);
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