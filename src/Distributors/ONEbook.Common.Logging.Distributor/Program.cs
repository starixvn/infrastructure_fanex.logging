namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    using System;
    using System.Reflection;
    using System.ServiceProcess;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var servicesToRun = new ServiceBase[]
            {
                new DistributorService()
            };

            if (Environment.UserInteractive)
            {
                //// run console
                Type type = typeof(ServiceBase);
                BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                MethodInfo method = type.GetMethod("OnStart", flags);
                foreach (ServiceBase service in servicesToRun)
                {
                    method.Invoke(service, new object[] { args });
                }

                Console.WriteLine("Fanex.Logging distributor service has been started.");
                Console.WriteLine("Press any key to exit!");
                Console.ReadLine();

                foreach (ServiceBase service in servicesToRun)
                {
                    service.Stop();
                }
            }
            else
            {
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}