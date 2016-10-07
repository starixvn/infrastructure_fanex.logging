namespace Fanex.Logging.Demo
{
    using System;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("First time testing");
                LogService.Error("Test logging1.", new Exception("Demo Exception"));
                LogService.Warn("Test warning1");
                LogService.Info("Test info1");

                Thread.Sleep(61 * 1000);

                Console.WriteLine("Second time testing");
                LogService.Error("Test logging.", new Exception("Demo Exception"));
                LogService.Warn("Test warning");
                LogService.Info("Test info");

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}