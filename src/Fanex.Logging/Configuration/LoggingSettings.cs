namespace Fanex.Logging.Configuration
{
    using System;
    using System.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

    internal class LoggingSettings : ConfigurationSection
    {
        private const string QueuePathPropertyName = "msmqPath";
        private const string RollingFilePathPropertyName = "filePath";
        private const string DefaultCategoryPropertyName = "defaultCategory";
        private const string SectionName = "Fanex.Logging";

        [ConfigurationProperty(QueuePathPropertyName)]
        public string MsmqPath
        {
            get { return base[QueuePathPropertyName] as string; }
            set { base[QueuePathPropertyName] = value; }
        }

        [ConfigurationProperty(RollingFilePathPropertyName)]
        public string FilePath
        {
            get { return base[RollingFilePathPropertyName] as string; }
            set { base[RollingFilePathPropertyName] = value; }
        }

        [ConfigurationProperty(DefaultCategoryPropertyName, IsRequired = true)]
        public string DefaultCategory
        {
            get { return base[DefaultCategoryPropertyName] as string; }
            set { base[DefaultCategoryPropertyName] = value; }
        }

        public static LoggingSettings GetSettings(IConfigurationSource configurationSource)
        {
            if (configurationSource == null)
            {
                throw new ArgumentNullException(nameof(configurationSource));
            }

            return configurationSource.GetSection(SectionName) as LoggingSettings;
        }
    }
}