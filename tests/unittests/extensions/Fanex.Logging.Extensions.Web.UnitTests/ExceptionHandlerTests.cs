namespace Fanex.Logging.Extensions.Web.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Web.SessionState;
    using NSubstitute;
    using Xunit;

    [Trait("Unit Tests", "Extensions/AspNetFramework/ExceptionHandler.cs")]
    public class ExceptionHandlerTests
    {
        private readonly Func<string, Task> writeError;
        private readonly Func<string, Exception, Task> writeException;
        private readonly LoggingConfiguration config;

        public ExceptionHandlerTests()
        {
            writeError = Substitute.For<Func<string, Task>>();
            writeException = Substitute.For<Func<string, Exception, Task>>();
            config = new LoggingConfiguration();

            ExceptionHandler.WriteErrorAsync = writeError;
            ExceptionHandler.WriteExceptionAsync = writeException;
        }

        [Fact]
        public void UseConfig_InfoLogConfigurationIsNull_ThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExceptionHandler.UseConfig(null));
        }

        [Fact]
        public void UseConfig_Always_SetConfig()
        {
            // Act
            ExceptionHandler.UseConfig(config);

            // Assert
            Assert.Equal(config, ExceptionHandler.config);
        }

        [Fact]
        public void UseGetDbExceptionInfo_GetDbExceptionInfoMethodIsNull_ThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExceptionHandler.UseGetDbExceptionInfo(null));
        }

        [Fact]
        public void UseGetDbExceptionInfo_Always_SetUseGetDbExceptionInfo()
        {
            // Arrange
            Func<Exception, IDictionary<string, object>> getDbExceptionInfo = (ex) => new Dictionary<string, object> { ["data"] = "value" };

            // Act
            ExceptionHandler.UseGetDbExceptionInfo(getDbExceptionInfo);

            // Assert
            Assert.Equal(getDbExceptionInfo, ExceptionHandler.config.GetDbExceptionInfo);
        }

        [Fact]
        public void Handle_ExceptionIsInIgnoredTypes_DontExecuteWriteError()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var loggingConfig = new LoggingConfiguration();
            loggingConfig.IgnoreException<DllNotFoundException>();
            ExceptionHandler.UseConfig(loggingConfig);

            // Act
            ExceptionHandler.Handle(exception);
            ExceptionHandler.Handle(exception, null);
            ExceptionHandler.Handle(exception, null, TraceEventType.Error);
            ExceptionHandler.Handle(exception, null, null, TraceEventType.Error);

            // Assert
            writeError.DidNotReceiveWithAnyArgs().Invoke("string");
            writeException.DidNotReceiveWithAnyArgs().Invoke("string", new Exception());
        }

        [Fact]
        public void Handle_HttpCurrentIsNull_ExecuteWriteException()
        {
            // Arrange
            var exception = new DllNotFoundException();

            // Act
            ExceptionHandler.Handle(exception, null, null, TraceEventType.Error);

            // Assert
            writeException.Received().Invoke(exception.Message, exception);
        }

        [Fact]
        public void Handle_HttpCurrentIsNotNull_ExecuteWriteError()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var context = WebHelper.CreateFakeHttpContext("url", new SessionStateItemCollection());

            ExceptionHandler.UseConfig(new LoggingConfiguration());

            ExceptionHandler.GetRequestInfo = (request) => new Dictionary<string, object> { ["request"] = "request data" };
            ExceptionHandler.GetBrowserInfo = (browser) => new Dictionary<string, object> { ["browser"] = "browser data" };
            ExceptionHandler.GetServerInfo = () => new Dictionary<string, object> { ["server"] = "server data" };
            ExceptionHandler.GetExceptionInfo = (ex) => new Dictionary<string, object> { ["exception"] = "exception data" };
            ExceptionHandler.UseGetDbExceptionInfo((ex) => new Dictionary<string, object> { ["database"] = "database data" });

            // Act
            ExceptionHandler.Handle(
                exception,
                new Dictionary<string, object> { ["custom"] = "custom data" },
                context,
                TraceEventType.Error);

            // Assert
            writeError.Received().Invoke(
                "Dll was not found.\r\n\r\n"
                + "REQUEST INFO\r\n\trequest: request data\r\n\r\n"
                + "BROWSER INFO\r\n\tbrowser: browser data\r\n\r\n"
                + "SERVER INFO\r\n\tserver: server data\r\n\r\n"
                + "DATABASE INFO\r\n\tdatabase: database data\r\n\r\n"
                + "CUSTOM INFO\r\n\tcustom: custom data\r\n\r\n"
                + "EXCEPTION INFO\r\n\texception: exception data\r\n");
        }

        [Fact]
        public void Handle_EnabledLogHttpHeader_ExecuteWriteErrorWithHeaderInfo()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var context = WebHelper.CreateFakeHttpContext("url", new SessionStateItemCollection());

            ExceptionHandler.UseConfig(new LoggingConfiguration().GetHttpHeaderInfo());

            ExceptionHandler.GetRequestInfo = (request) => new Dictionary<string, object> { ["request"] = "request data" };
            ExceptionHandler.GetBrowserInfo = (browser) => new Dictionary<string, object> { ["browser"] = "browser data" };
            ExceptionHandler.GetServerInfo = () => new Dictionary<string, object> { ["server"] = "server data" };
            ExceptionHandler.GetExceptionInfo = (ex) => new Dictionary<string, object> { ["exception"] = "exception data" };
            ExceptionHandler.UseGetDbExceptionInfo((ex) => new Dictionary<string, object> { ["database"] = "database data" });
            ExceptionHandler.GetRequestHeader = (ex) => new Dictionary<string, object> { ["header"] = "header data" };

            // Act
            ExceptionHandler.Handle(
                exception,
                new Dictionary<string, object> { ["custom"] = "custom data" },
                context,
                TraceEventType.Error);

            // Assert
            writeError.Received().Invoke(
                "Dll was not found.\r\n\r\n"
                + "REQUEST INFO\r\n\trequest: request data\r\n\r\n"
                + "BROWSER INFO\r\n\tbrowser: browser data\r\n\r\n"
                + "SERVER INFO\r\n\tserver: server data\r\n\r\n"
                + "DATABASE INFO\r\n\tdatabase: database data\r\n\r\n"
                + "CUSTOM INFO\r\n\tcustom: custom data\r\n\r\n"
                + "EXCEPTION INFO\r\n\texception: exception data\r\n\r\n"
                + "REQUEST HEADERS\r\n\theader: header data\r\n");
        }

        [Fact]
        public void Handle_EnabledLogHttpSession_ExecuteWriteErrorWithSessionInfo()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var context = WebHelper.CreateFakeHttpContext("url", new SessionStateItemCollection());

            ExceptionHandler.UseConfig(new LoggingConfiguration().GetHttpSessionInfo());

            ExceptionHandler.GetRequestInfo = (request) => new Dictionary<string, object> { ["request"] = "request data" };
            ExceptionHandler.GetBrowserInfo = (browser) => new Dictionary<string, object> { ["browser"] = "browser data" };
            ExceptionHandler.GetServerInfo = () => new Dictionary<string, object> { ["server"] = "server data" };
            ExceptionHandler.GetExceptionInfo = (ex) => new Dictionary<string, object> { ["exception"] = "exception data" };
            ExceptionHandler.UseGetDbExceptionInfo((ex) => new Dictionary<string, object> { ["database"] = "database data" });
            ExceptionHandler.GetSessionInfo = (ex) => new Dictionary<string, object> { ["session"] = "session data" };

            // Act
            ExceptionHandler.Handle(
                exception,
                new Dictionary<string, object> { ["custom"] = "custom data" },
                context,
                TraceEventType.Error);

            // Assert
            writeError.Received().Invoke(
                "Dll was not found.\r\n\r\n"
                + "REQUEST INFO\r\n\trequest: request data\r\n\r\n"
                + "BROWSER INFO\r\n\tbrowser: browser data\r\n\r\n"
                + "SERVER INFO\r\n\tserver: server data\r\n\r\n"
                + "DATABASE INFO\r\n\tdatabase: database data\r\n\r\n"
                + "CUSTOM INFO\r\n\tcustom: custom data\r\n\r\n"
                + "EXCEPTION INFO\r\n\texception: exception data\r\n\r\n"
                + "SESSION INFO\r\n\tsession: session data\r\n");
        }

        [Fact]
        public void Handle_SetHiddenSensitiveInfo_ExecuteWriteErrorWithoutSensitiveInfo()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var sensitiveInfo = "password=asdadad";
            var context = WebHelper.CreateFakeHttpContext($"url", new SessionStateItemCollection());

            ExceptionHandler.UseConfig(
                new LoggingConfiguration()
                .HideSensitiveInfo(new List<string> { "(?:password=)(?<paramValue>[^&|^\\s]+)" }));

            ExceptionHandler.GetRequestInfo = (request) => new Dictionary<string, object> { ["request"] = $"request data {sensitiveInfo}" };
            ExceptionHandler.GetBrowserInfo = (browser) => new Dictionary<string, object> { ["browser"] = "browser data" };
            ExceptionHandler.GetServerInfo = () => new Dictionary<string, object> { ["server"] = "server data" };
            ExceptionHandler.GetExceptionInfo = (ex) => new Dictionary<string, object> { ["exception"] = "exception data" };
            ExceptionHandler.UseGetDbExceptionInfo((ex) => new Dictionary<string, object> { ["database"] = "database data" });

            // Act
            ExceptionHandler.Handle(
                exception,
                new Dictionary<string, object> { ["custom"] = "custom data 1" },
                context,
                TraceEventType.Error);

            // Assert
            writeError.Received().Invoke(
                "Dll was not found.\r\n\r\n"
                + "REQUEST INFO\r\n\trequest: request data password=*****\r\n\r\n"
                + "BROWSER INFO\r\n\tbrowser: browser data\r\n\r\n"
                + "SERVER INFO\r\n\tserver: server data\r\n\r\n"
                + "DATABASE INFO\r\n\tdatabase: database data\r\n\r\n"
                + "CUSTOM INFO\r\n\tcustom: custom data 1\r\n\r\n"
                + "EXCEPTION INFO\r\n\texception: exception data\r\n");
        }

        [Fact]
        public void Handle_TraceEventTypeIsVerbose_ExecuteWriteDebug()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var context = WebHelper.CreateFakeHttpContext("url", new SessionStateItemCollection());

            ExceptionHandler.UseConfig(new LoggingConfiguration());

            ExceptionHandler.GetRequestInfo = (request) => new Dictionary<string, object> { ["request"] = "request data" };
            ExceptionHandler.GetBrowserInfo = (browser) => new Dictionary<string, object> { ["browser"] = "browser data" };
            ExceptionHandler.GetServerInfo = () => new Dictionary<string, object> { ["server"] = "server data" };
            ExceptionHandler.GetExceptionInfo = (ex) => new Dictionary<string, object> { ["exception"] = "exception data" };
            ExceptionHandler.UseGetDbExceptionInfo((ex) => new Dictionary<string, object> { ["database"] = "database data" });

            var writeDebug = Substitute.For<Func<string, Task>>();
            ExceptionHandler.WriteDebugAsync = writeDebug;

            // Act
            ExceptionHandler.Handle(
                exception,
                new Dictionary<string, object> { ["custom"] = "custom data" },
                context,
                TraceEventType.Verbose);

            // Assert
            writeDebug.Received().Invoke(
                "Dll was not found.\r\n\r\n"
                + "REQUEST INFO\r\n\trequest: request data\r\n\r\n"
                + "BROWSER INFO\r\n\tbrowser: browser data\r\n\r\n"
                + "SERVER INFO\r\n\tserver: server data\r\n\r\n"
                + "DATABASE INFO\r\n\tdatabase: database data\r\n\r\n"
                + "CUSTOM INFO\r\n\tcustom: custom data\r\n\r\n"
                + "EXCEPTION INFO\r\n\texception: exception data\r\n");
        }

        [Fact]
        public void Handle_TraceEventTypeIsInformation_ExecuteWriteInfo()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var context = WebHelper.CreateFakeHttpContext("url", new SessionStateItemCollection());

            ExceptionHandler.UseConfig(new LoggingConfiguration());

            ExceptionHandler.GetRequestInfo = (request) => new Dictionary<string, object> { ["request"] = "request data" };
            ExceptionHandler.GetBrowserInfo = (browser) => new Dictionary<string, object> { ["browser"] = "browser data" };
            ExceptionHandler.GetServerInfo = () => new Dictionary<string, object> { ["server"] = "server data" };
            ExceptionHandler.GetExceptionInfo = (ex) => new Dictionary<string, object> { ["exception"] = "exception data" };
            ExceptionHandler.UseGetDbExceptionInfo((ex) => new Dictionary<string, object> { ["database"] = "database data" });

            var writeInfo = Substitute.For<Func<string, Task>>();
            ExceptionHandler.WriteInfoAsync = writeInfo;

            // Act
            ExceptionHandler.Handle(
                exception,
                new Dictionary<string, object> { ["custom"] = "custom data" },
                context,
                TraceEventType.Information);

            // Assert
            writeInfo.Received().Invoke(
                "Dll was not found.\r\n\r\n"
                + "REQUEST INFO\r\n\trequest: request data\r\n\r\n"
                + "BROWSER INFO\r\n\tbrowser: browser data\r\n\r\n"
                + "SERVER INFO\r\n\tserver: server data\r\n\r\n"
                + "DATABASE INFO\r\n\tdatabase: database data\r\n\r\n"
                + "CUSTOM INFO\r\n\tcustom: custom data\r\n\r\n"
                + "EXCEPTION INFO\r\n\texception: exception data\r\n");
        }

        [Fact]
        public void Handle_TraceEventTypeIsWarning_ExecuteWriteWarn()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var context = WebHelper.CreateFakeHttpContext("url", new SessionStateItemCollection());

            ExceptionHandler.UseConfig(new LoggingConfiguration());

            ExceptionHandler.GetRequestInfo = (request) => new Dictionary<string, object> { ["request"] = "request data" };
            ExceptionHandler.GetBrowserInfo = (browser) => new Dictionary<string, object> { ["browser"] = "browser data" };
            ExceptionHandler.GetServerInfo = () => new Dictionary<string, object> { ["server"] = "server data" };
            ExceptionHandler.GetExceptionInfo = (ex) => new Dictionary<string, object> { ["exception"] = "exception data" };
            ExceptionHandler.UseGetDbExceptionInfo((ex) => new Dictionary<string, object> { ["database"] = "database data" });

            var writeWarn = Substitute.For<Func<string, Task>>();
            ExceptionHandler.WriteWarnAsync = writeWarn;

            // Act
            ExceptionHandler.Handle(
                exception,
                new Dictionary<string, object> { ["custom"] = "custom data" },
                context,
                TraceEventType.Warning);

            // Assert
            writeWarn.Received().Invoke(
                "Dll was not found.\r\n\r\n"
                + "REQUEST INFO\r\n\trequest: request data\r\n\r\n"
                + "BROWSER INFO\r\n\tbrowser: browser data\r\n\r\n"
                + "SERVER INFO\r\n\tserver: server data\r\n\r\n"
                + "DATABASE INFO\r\n\tdatabase: database data\r\n\r\n"
                + "CUSTOM INFO\r\n\tcustom: custom data\r\n\r\n"
                + "EXCEPTION INFO\r\n\texception: exception data\r\n");
        }

        [Fact]
        public void Handle_TraceEventTypeIsCritical_ExecuteWriteCritical()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var context = WebHelper.CreateFakeHttpContext("url", new SessionStateItemCollection());

            ExceptionHandler.UseConfig(new LoggingConfiguration());

            ExceptionHandler.GetRequestInfo = (request) => new Dictionary<string, object> { ["request"] = "request data" };
            ExceptionHandler.GetBrowserInfo = (browser) => new Dictionary<string, object> { ["browser"] = "browser data" };
            ExceptionHandler.GetServerInfo = () => new Dictionary<string, object> { ["server"] = "server data" };
            ExceptionHandler.GetExceptionInfo = (ex) => new Dictionary<string, object> { ["exception"] = "exception data" };
            ExceptionHandler.UseGetDbExceptionInfo((ex) => new Dictionary<string, object> { ["database"] = "database data" });

            var writeCritical = Substitute.For<Func<string, Task>>();
            ExceptionHandler.WriteCriticalAsync = writeCritical;

            // Act
            ExceptionHandler.Handle(
                exception,
                new Dictionary<string, object> { ["custom"] = "custom data" },
                context,
                TraceEventType.Critical);

            // Assert
            writeCritical.Received().Invoke(
                "Dll was not found.\r\n\r\n"
                + "REQUEST INFO\r\n\trequest: request data\r\n\r\n"
                + "BROWSER INFO\r\n\tbrowser: browser data\r\n\r\n"
                + "SERVER INFO\r\n\tserver: server data\r\n\r\n"
                + "DATABASE INFO\r\n\tdatabase: database data\r\n\r\n"
                + "CUSTOM INFO\r\n\tcustom: custom data\r\n\r\n"
                + "EXCEPTION INFO\r\n\texception: exception data\r\n");
        }

        [Fact]
        public void Handle_ExceptionThrow_ExecuteWriteException()
        {
            // Arrange
            var exception = new DllNotFoundException();
            var context = WebHelper.CreateFakeHttpContext("url", new SessionStateItemCollection());

            ExceptionHandler.UseConfig(new LoggingConfiguration());

            ExceptionHandler.GetRequestInfo = (request) => new Dictionary<string, object> { ["request"] = "request data" };
            ExceptionHandler.GetBrowserInfo = (browser) => new Dictionary<string, object> { ["browser"] = "browser data" };
            ExceptionHandler.GetServerInfo = () => new Dictionary<string, object> { ["server"] = "server data" };
            ExceptionHandler.GetExceptionInfo = (ex) => new Dictionary<string, object> { ["exception"] = "exception data" };
            ExceptionHandler.UseGetDbExceptionInfo((ex) => new Dictionary<string, object> { ["database"] = "database data" });

            var simulatedException = new Exception("Simulated Exception");
            ExceptionHandler.WriteErrorAsync = (message) =>
            {
                throw simulatedException;
            };

            var writeException = Substitute.For<Func<string, Exception, Task>>();
            ExceptionHandler.WriteExceptionAsync = writeException;

            // Act
            ExceptionHandler.Handle(
                exception,
                new Dictionary<string, object> { ["custom"] = "custom data" },
                context,
                TraceEventType.Error);

            // Assert
            writeException.Received().Invoke(
                Arg.Is<string>(message => message.Contains("Consumer Exception:System.DllNotFoundException: Dll was not found.,")),
                exception);
        }
    }
}