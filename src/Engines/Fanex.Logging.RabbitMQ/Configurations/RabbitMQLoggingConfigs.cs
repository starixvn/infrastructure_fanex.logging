namespace Fanex.Logging.RabbitMQ.Configurations
{
    public interface IRabbitMQLoggingConfigs
    {
        string ConnectionString { get; }

        string DefaultCagetory { get; }
    }

    public class RabbitMQLoggingConfigs : IRabbitMQLoggingConfigs
    {
        public RabbitMQLoggingConfigs(
            string connectionString, 
            string defaultCagetory)
        {
            ConnectionString = connectionString;
            DefaultCagetory = defaultCagetory;
        }

        public string ConnectionString { get; }

        public string DefaultCagetory { get; }
    }
}