namespace Fanex.Logging.Configuration
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    /// <summary>
    /// The logger configuration class.
    /// </summary>
    internal static class LoggerConfiguration
    {
        private static volatile LoggingSettings _loggingSetting;
        private static volatile bool _hasRunConfig = false;
        private static object syncRoot = new object();

        private static TraceOptions traceOptions =
                                TraceOptions.Callstack
                                | TraceOptions.DateTime
                                | TraceOptions.LogicalOperationStack
                                | TraceOptions.ProcessId
                                | TraceOptions.ThreadId
                                | TraceOptions.Timestamp;

        static LoggerConfiguration()
        {
            if (_loggingSetting == null)
            {
                _loggingSetting = LoggingSettings.GetSettings(ConfigurationSourceFactory.Create());
            }
        }

        public static LoggingSettings Setting
        {
            get { return _loggingSetting; }
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public static void Run()
        {
            if (_hasRunConfig)
            {
                return;
            }
            lock (syncRoot)
            {
                if (!_hasRunConfig)
                {
                    var builder = new ConfigurationSourceBuilder();
                    var configSource = new DictionaryConfigurationSource();
                    var categoryConfiguration = builder
                        .ConfigureLogging()
                        .SpecialSources.UnprocessedCategory
                        .SpecialSources.LoggingErrorsAndWarningsCategory
                        .SpecialSources.AllEventsCategory
                        as ILoggingConfigurationCategoryContd;

                    ConfigureMsmq(categoryConfiguration);
                    ConfigureRollingFile(categoryConfiguration);

                    builder.UpdateConfigurationWithReplace(configSource);

                    EnterpriseLibraryContainer.Current = EnterpriseLibraryContainer.CreateDefaultContainer(configSource);
                    _hasRunConfig = true;
                }
            }
        }

        private static void ConfigureMsmq(ILoggingConfigurationCategoryContd categoryConfiguration)
        {
            if (!string.IsNullOrWhiteSpace(Setting.MsmqPath))
            {
                categoryConfiguration
                    .SendTo.Msmq("MsmqListener").UseQueue(Setting.MsmqPath)
                    .WithTraceOptions(traceOptions)
                    .FormatWith(new FormatterBuilder().BinaryFormatterNamed("BinaryFormatter"));
            }
        }

        private static void ConfigureRollingFile(ILoggingConfigurationCategoryContd categoryConfiguration)
        {
            if (!string.IsNullOrWhiteSpace(Setting.FilePath))
            {
                string filePath = ResolveFilePath(Setting.FilePath);

                categoryConfiguration
                    .SendTo.RollingFile("RollingFlatFileListener").ToFile(filePath)
                    .RollEvery(RollInterval.Day).UseTimeStampPattern("yyyyMMdd").WhenRollFileExists(RollFileExistsBehavior.Increment)
                    .WithTraceOptions(traceOptions)
                    .FormatWith(new FormatterBuilder().TextFormatterNamed("TextFormatter"));
            }
        }

        private static string ResolveFilePath(string filePath)
        {
            string finalFilePath = filePath;

            if (!Path.IsPathRooted(filePath))
            {
                string rootPath = AppDomain.CurrentDomain.BaseDirectory;

                finalFilePath = finalFilePath.TrimStart('~').TrimStart('\\').TrimStart('/');
                finalFilePath = Path.Combine(rootPath, finalFilePath);
            }

            if (Directory.Exists(finalFilePath))
            {
                var startWritingDate = DateTime.Now;

                finalFilePath = Path.Combine(
                    finalFilePath,
                    $"Logs.{startWritingDate.ToString("yyyyMMdd")}.log");
            }

            return finalFilePath;
        }
    }
}