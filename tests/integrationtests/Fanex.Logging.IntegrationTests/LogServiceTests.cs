namespace Fanex.Logging.IntegrationTests
{
    using System;
    using System.IO;
    using System.Reflection;
    using EntLib;
    using EntLib.Configuration;
    using Xunit;

    [Trait("Integration Tests", "LogService")]
    [CollectionDefinition("Log Service Integration Tests")]
    public class LogServiceTests
    {
        private static bool cleanedOutputFiles;
        private static object syncRoot = new object();
        private static string currentDirectoryPath;

        public LogServiceTests()
        {
            LogManager
                .SetDefaultLogCategory("Test-Cagegory")
                .Use(new EntlibLogging(new EntLibConfig(string.Empty, @"~\Logs\")));

            CleanOutputFiles();
        }

        [Fact]
        public void Error_Always_WriteError()
        {
            // Act
            LogService.Error("Error Message 1");

            // Assert
            AssertLogContent(
                "Message: Error Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Error\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void Error_Always_WriteErrorWithExceptionInfo()
        {
            // Act
            LogService.Error("Error Message 2", new DllNotFoundException());

            // Assert
            AssertLogContent(
                "Message: Error Message 2\n"
                + "System.DllNotFoundException: Dll was not found.\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Error\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void Error_Always_WriteErrorWithCustomCategory()
        {
            // Act
            LogService.Error("Error Message 3", "Custom-Category", new DllNotFoundException());

            // Assert
            AssertLogContent(
                "Message: Error Message 3\n"
                + "System.DllNotFoundException: Dll was not found.\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Error\r\n\n"
                + "Title:Custom-Category\r\n\n");
        }

        [Fact]
        public void Warn_Always_WriteWarning()
        {
            // Act
            LogService.Warn("Warn Message 1");

            // Assert
            AssertLogContent(
                "Message: Warn Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Warning\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void Warn_Always_WriteWarningWithCustomCategory()
        {
            // Act
            LogService.Warn("Warn Message 2", "Custom-Category");

            // Assert
            AssertLogContent(
                "Message: Warn Message 2\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Warning\r\n\n"
                + "Title:Custom-Category");
        }

        [Fact]
        public void Info_Always_WriteInformation()
        {
            // Act
            LogService.Info("Info Message 1");

            // Assert
            AssertLogContent(
                "Message: Info Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Information\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void Info_Always_WriteInfoWithCustomCategory()
        {
            // Act
            LogService.Info("Info Message 2", "Custom-Category");

            // Assert
            AssertLogContent(
                "Message: Info Message 2\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Information\r\n\n"
                + "Title:Custom-Category");
        }

        [Fact]
        public void Critical_Always_WriteCriticalInfo()
        {
            // Act
            LogService.Critical("Critical Message 1");

            // Assert
            AssertLogContent(
                "Message: Critical Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Critical\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void Critical_Always_WriteCriticalInfoWithCustomCategory()
        {
            // Act
            LogService.Critical("Critical Message 2", "Custom-Category");

            // Assert
            AssertLogContent(
                "Message: Critical Message 2\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Critical\r\n\n"
                + "Title:Custom-Category");
        }

        [Fact]
        public void Debug_Always_WriteDebugInfo()
        {
            // Act
            LogService.Debug("Debug Message 1");

            // Assert
            AssertLogContent(
                "Message: Debug Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Verbose\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void Debug_Always_WriteDebugInfoWithCustomCategory()
        {
            // Act
            LogService.Debug("Debug Message 2", "Custom-Category");

            // Assert
            AssertLogContent(
                "Message: Debug Message 2\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Verbose\r\n\n"
                + "Title:Custom-Category");
        }

        [Fact]
        public void ErrorAsync_Always_WriteErrorAsync()
        {
            // Act
            LogService.ErrorAsync("ErrorAsync Message 1");

            // Assert
            AssertLogContent(
                "Message: ErrorAsync Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Error\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void ErrorAsync_Always_WriteErrorAsyncWithExceptionInfoAsync()
        {
            // Act
            LogService.ErrorAsync("ErrorAsync Message 2", new DllNotFoundException());

            // Assert
            AssertLogContent(
                "Message: ErrorAsync Message 2\n"
                + "System.DllNotFoundException: Dll was not found.\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Error\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void ErrorAsync_Always_WriteErrorAsyncWithCustomCategory()
        {
            // Act
            LogService.ErrorAsync("ErrorAsync Message 3", "Custom-Category", new DllNotFoundException());

            // Assert
            AssertLogContent(
                "Message: ErrorAsync Message 3\n"
                + "System.DllNotFoundException: Dll was not found.\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Error\r\n\n"
                + "Title:Custom-Category\r\n\n");
        }

        [Fact]
        public void WarnAsync_Always_WriteWarnAsyncing()
        {
            // Act
            LogService.WarnAsync("WarnAsync Message 1");

            // Assert
            AssertLogContent(
                "Message: WarnAsync Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Warning\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void WarnAsync_Always_WriteWarnAsyncingWithCustomCategory()
        {
            // Act
            LogService.WarnAsync("WarnAsync Message 2", "Custom-Category");

            // Assert
            AssertLogContent(
                "Message: WarnAsync Message 2\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Warning\r\n\n"
                + "Title:Custom-Category");
        }

        [Fact]
        public void InfoAsync_Always_WriteInfoAsyncrmation()
        {
            // Act
            LogService.InfoAsync("InfoAsync Message 1");

            // Assert
            AssertLogContent(
                "Message: InfoAsync Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Information\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void InfoAsync_Always_WriteInfoAsyncWithCustomCategory()
        {
            // Act
            LogService.InfoAsync("InfoAsync Message 2", "Custom-Category");

            // Assert
            AssertLogContent(
                "Message: InfoAsync Message 2\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Information\r\n\n"
                + "Title:Custom-Category");
        }

        [Fact]
        public void CriticalAsync_Always_WriteCriticalAsyncInfoAsync()
        {
            // Act
            LogService.CriticalAsync("CriticalAsync Message 1");

            // Assert
            AssertLogContent(
                "Message: CriticalAsync Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Critical\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void CriticalAsync_Always_WriteCriticalAsyncInfoAsyncWithCustomCategory()
        {
            // Act
            LogService.CriticalAsync("CriticalAsync Message 2", "Custom-Category");

            // Assert
            AssertLogContent(
                "Message: CriticalAsync Message 2\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Critical\r\n\n"
                + "Title:Custom-Category");
        }

        [Fact]
        public void DebugAsync_Always_WriteDebugAsyncInfoAsync()
        {
            // Act
            LogService.DebugAsync("DebugAsync Message 1");

            // Assert
            AssertLogContent(
                "Message: DebugAsync Message 1\r\n\n"
                + "Category: Test-Cagegory\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Verbose\r\n\n"
                + "Title:Test-Cagegory");
        }

        [Fact]
        public void DebugAsync_Always_WriteDebugAsyncInfoAsyncWithCustomCategory()
        {
            // Act
            LogService.DebugAsync("DebugAsync Message 2", "Custom-Category");

            // Assert
            AssertLogContent(
                "Message: DebugAsync Message 2\r\n\n"
                + "Category: Custom-Category\r\n\n"
                + "Priority: -1\r\n\n"
                + "EventId: 0\r\n\n"
                + "Severity: Verbose\r\n\n"
                + "Title:Custom-Category");
        }

        public void AssertLogContent(string shouldBeExistedContent)
        {
            var logContent = GetLogContent();
            Assert.True(logContent.Contains(shouldBeExistedContent));
        }

        private static string LogsDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(currentDirectoryPath))
                {
                    string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                    UriBuilder uri = new UriBuilder(codeBase);
                    string path = Uri.UnescapeDataString(uri.Path);
                    currentDirectoryPath = $"{Path.GetDirectoryName(path)}\\Logs\\";
                }

                return currentDirectoryPath;
            }
        }

        public static void CleanOutputFiles()
        {
            if (!cleanedOutputFiles)
            {
                lock (syncRoot)
                {
                    if (!cleanedOutputFiles)
                    {
                        cleanedOutputFiles = true;

                        DeleteAllFiles();
                    }
                }
            }
        }

        private static void DeleteAllFiles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(LogsDirectory);

            try
            {
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            catch
#pragma warning disable S108 // Nested blocks of code should not be left empty
            { }
#pragma warning restore S108 // Nested blocks of code should not be left empty
        }

        public static string GetLogContent()
        {
            var fileName = $"Logs.{DateTime.Now.ToString("yyyyMMdd")}.log";
            var fileCopiedName = $"Logs.{DateTime.Now.ToString("yyyyMMdd")}.ForTesting.log";

            var filePath = Path.Combine(LogsDirectory, fileName);
            var fileCopiedPath = Path.Combine(LogsDirectory, fileCopiedName);

            File.Copy(filePath, fileCopiedPath, true);

            return File.ReadAllText(fileCopiedPath);
        }
    }
}