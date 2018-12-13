using System;

namespace Fanex.Logging.SimpleSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                LogService.Info($"Test Message at {DateTime.Now}");

                Console.WriteLine("Done");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}