
namespace Microsoft.Practices.EnterpriseLibrary.Logging.RabbitMQDistributor.Configuration
{
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class RabbitMQDistributorSettings : SerializableConfigurationSection
    {
        private const string rabbitMQConnectionStringProperty = "rabbitMQConnectionString";
        private const string serviceNameProperty = "serviceName";

        /// <summary>
        /// Retrieves the instance of <see cref="RabbitMQDistributorSettings"/> from the <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> to get the section from.</param>
        /// <returns>The section, if exists in the configuration source.</returns>
        public static RabbitMQDistributorSettings GetSettings(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");
            return configurationSource.GetSection(SectionName) as RabbitMQDistributorSettings;
        }

        /// <summary>
        /// The path to the msmq to use.
        /// </summary>
        [ConfigurationProperty(rabbitMQConnectionStringProperty, IsRequired = true)]
        public string RabbitMQConnectionString
        {
            get { return (string)base[rabbitMQConnectionStringProperty]; }
            set { base[rabbitMQConnectionStringProperty] = value; }
        }

        /// <summary>
        /// The name to use in the service.
        /// </summary>
        [ConfigurationProperty(serviceNameProperty, IsRequired = true)]
        public string ServiceName
        {
            get { return (string)base[serviceNameProperty]; }
            set { base[serviceNameProperty] = value; }
        }

        /// <summary>
        /// The name of the configuration section.
        /// </summary>
        public static string SectionName
        {
            get { return "rabbitMQDistributorSettings"; }
        }
    }
}
