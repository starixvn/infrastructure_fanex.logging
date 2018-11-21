namespace Fanex.Logging.EntLib.Configuration
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    internal static class LoggerConfiguration
    {
        private static volatile bool hasRunConfig;
        private static object syncRoot = new object();

        private static TraceOptions traceOptions =
                                TraceOptions.Callstack
                                | TraceOptions.DateTime
                                | TraceOptions.LogicalOperationStack
                                | TraceOptions.ProcessId
                                | TraceOptions.ThreadId
                                | TraceOptions.Timestamp;

        public static void Run(EntLibConfig config)
        {
            if (hasRunConfig)
            {
                return;
            }

            lock (syncRoot)
            {
                if (!hasRunConfig)
                {
                    var builder = new ConfigurationSourceBuilder();
                    var configSource = new DictionaryConfigurationSource();
                    var categoryConfiguration = builder
                        .ConfigureLogging()
                        .SpecialSources.UnprocessedCategory
                        .SpecialSources.LoggingErrorsAndWarningsCategory
                        .SpecialSources.AllEventsCategory
                        as ILoggingConfigurationCategoryContd;

                    ConfigureMsmq(categoryConfiguration, config);
                    ConfigureRollingFile(categoryConfiguration, config);

                    builder.UpdateConfigurationWithReplace(configSource);

                    EnterpriseLibraryContainer.Current = EnterpriseLibraryContainer.CreateDefaultContainer(configSource);
                    hasRunConfig = true;
                }
            }
        }

        internal static void ConfigureMsmq(ILoggingConfigurationCategoryContd categoryConfiguration, EntLibConfig config)
        {
            if (!string.IsNullOrWhiteSpace(config.MsmqPath))
            {
                categoryConfiguration
                    .SendTo.Msmq("MsmqListener").UseQueue(config.MsmqPath)
                    .WithTraceOptions(traceOptions)
                    .FormatWith(new FormatterBuilder().BinaryFormatterNamed("BinaryFormatter"));
            }
        }

        internal static void ConfigureRollingFile(ILoggingConfigurationCategoryContd categoryConfiguration, EntLibConfig config)
        {
            if (!string.IsNullOrWhiteSpace(config.FilePath))
            {
                string filePath = ResolveFilePath(config.FilePath);

                categoryConfiguration
                    .SendTo.RollingFile("RollingFlatFileListener").ToFile(filePath)
                    .RollEvery(RollInterval.Day).UseTimeStampPattern("yyyyMMdd").WhenRollFileExists(RollFileExistsBehavior.Increment).CleanUpArchivedFilesWhenMoreThan(90)
                    .WithTraceOptions(traceOptions)
                    .FormatWith(new FormatterBuilder().TextFormatterNamed("TextFormatter"));
            }
        }

        internal static string ResolveFilePath(string filePath)
        {
            string directoryPath = filePath;

            if (!Path.IsPathRooted(filePath))
            {
                string rootPath = AppDomain.CurrentDomain.BaseDirectory;

                directoryPath = directoryPath.TrimStart('~').TrimStart('\\').TrimStart('/');
                directoryPath = Path.Combine(rootPath, directoryPath);
            }

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var startWritingDate = DateTime.Now;

            directoryPath = Path.Combine(
                directoryPath,
                $"Logs.{startWritingDate.ToString("yyyyMMdd")}.log");

            return directoryPath;
        }
    }
}