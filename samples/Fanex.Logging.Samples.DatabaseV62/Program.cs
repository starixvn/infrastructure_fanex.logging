namespace Fanex.Logging.Samples.DatabaseV62
{
    using System;
    using System.Diagnostics;
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
    }
}