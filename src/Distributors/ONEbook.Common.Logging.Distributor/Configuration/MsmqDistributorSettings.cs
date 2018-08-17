using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration
{
	public class MsmqDistributorSettings : SerializableConfigurationSection
	{
		private const string msmqPathProperty = "msmqPath";
		private const string queueTimerIntervalProperty = "queueTimerInterval";
		private const string serviceNameProperty = "serviceName";

		public static MsmqDistributorSettings GetSettings(IConfigurationSource configurationSource)
		{
            if(configurationSource == null) throw new ArgumentNullException("configurationSource");
			return configurationSource.GetSection(SectionName) as MsmqDistributorSettings;
		}

		[ConfigurationProperty(msmqPathProperty, IsRequired= true)]
		public string MsmqPath
		{
			get { return (string)base[msmqPathProperty]; }
			set { base[msmqPathProperty] = value; }
		}

		[ConfigurationProperty(queueTimerIntervalProperty, IsRequired= false)]
		public int QueueTimerInterval
		{
			get { return (int)base[queueTimerIntervalProperty]; }
			set { base[queueTimerIntervalProperty] = value; }
		}

		/// <summary>
		/// The name to use in the service.
		/// </summary>
		[ConfigurationProperty(serviceNameProperty, IsRequired= true)]
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
			get { return "msmqDistributorSettings"; }
		}
	}
}
