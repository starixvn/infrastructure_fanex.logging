namespace Fanex.Logging.RabbitMQ.Helpers
{
    using EasyNetQ;

    public static class RabbitMQManager
    {
        private static volatile IBus bus;
        private static object syncRoot = new object();

        public static IBus CreateBus(string rabbitMQConnectionString)
        {
            if (bus == null)
            {
                lock (syncRoot)
                {
                    if (bus == null)
                    {
                        bus = RabbitHutch.CreateBus(EasyNetQConnectionStringCryptography.DecryptConnectionString(rabbitMQConnectionString));
                    }
                }
            }

            return bus;
        }
    }
}