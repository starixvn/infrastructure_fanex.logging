namespace Fanex.Logging.CustomTraceListener.Email
{
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
    using Services;

    [ConfigurationElementType(typeof(CustomEmailTraceListenerData))]
    public class CustomEmailTraceListener : CustomFormattedTraceListenerBase
    {
        private static ICacheService _cacheManager = new CacheService();

        private string _toAddress = string.Empty;
        private string _fromAddress = string.Empty;
        private string _subjectLineStarter = string.Empty;
        private string _subjectLineEnder = string.Empty;
        private string _smtpServer = string.Empty;
        private int _smtpPort = 25;
        private EmailAuthenticationMode _authenticationMode = EmailAuthenticationMode.None;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private bool _useSSL = false;
        private string _umAddress = string.Empty;
        private bool _isCheckingDowntimeEnabled = true;
        private int _cacheDuplicatedEmailTimeOut = 120;

        public CustomEmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            ILogFormatter formatter,
            string umAddress,
            bool isCheckingDowntimeEnabled,
            int cacheDuplicatedEmailTimeOut)
            : this(
                  toAddress,
                  fromAddress,
                  subjectLineStarter,
                  subjectLineEnder,
                  smtpServer,
                  25,
                  formatter,
                  umAddress,
                  isCheckingDowntimeEnabled,
                  cacheDuplicatedEmailTimeOut)
        {
        }

        public CustomEmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            ILogFormatter formatter,
            string umAddress,
            bool isCheckingDowntimeEnabled,
            int cacheDuplicatedEmailTimeOut)
            : base(formatter)
        {
            _toAddress = toAddress;
            _fromAddress = fromAddress;
            _subjectLineStarter = subjectLineStarter;
            _subjectLineEnder = subjectLineEnder;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _umAddress = umAddress;
            _isCheckingDowntimeEnabled = isCheckingDowntimeEnabled;
            _cacheDuplicatedEmailTimeOut = cacheDuplicatedEmailTimeOut;
        }

        public CustomEmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            ILogFormatter formatter,
            EmailAuthenticationMode authenticationMode,
            string userName,
            string password,
            bool useSSL,
            string umAddress,
            bool isCheckingDowntimeEnabled,
            int cacheDuplicatedEmailTimeOut)
            : base(formatter)
        {
            _toAddress = toAddress;
            _fromAddress = fromAddress;
            _subjectLineStarter = subjectLineStarter;
            _subjectLineEnder = subjectLineEnder;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _authenticationMode = authenticationMode;
            _userName = userName;
            _password = password;
            _useSSL = useSSL;
            _umAddress = umAddress;
            _isCheckingDowntimeEnabled = isCheckingDowntimeEnabled;
            _cacheDuplicatedEmailTimeOut = cacheDuplicatedEmailTimeOut;
        }

        public CustomEmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            string umAddress,
            bool isCheckingDowntimeEnabled,
            int cacheDuplicatedEmailTimeOut)
            : this(
                  toAddress,
                  fromAddress,
                  subjectLineStarter,
                  subjectLineEnder,
                  smtpServer,
                  25,
                  umAddress,
                  isCheckingDowntimeEnabled,
                  cacheDuplicatedEmailTimeOut)
        {
        }

        public CustomEmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            string umAddress,
            bool isCheckingDowntimeEnabled,
            int cacheDuplicatedEmailTimeOut)
            : this(
                  toAddress,
                  fromAddress,
                  subjectLineStarter,
                  subjectLineEnder,
                  smtpServer,
                  smtpPort,
                  EmailAuthenticationMode.None,
                  null,
                  null,
                  false,
                  umAddress,
                  isCheckingDowntimeEnabled,
                  cacheDuplicatedEmailTimeOut)
        {
        }

        public CustomEmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            EmailAuthenticationMode authenticationMode,
            string userName,
            string password,
            bool useSSL,
            string umAddress,
            bool isCheckingDowntimeEnabled,
            int cacheDuplicatedEmailTimeOut)
        {
            _toAddress = toAddress;
            _fromAddress = fromAddress;
            _subjectLineStarter = subjectLineStarter;
            _subjectLineEnder = subjectLineEnder;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _authenticationMode = authenticationMode;
            _userName = userName;
            _password = password;
            _useSSL = useSSL;
            _umAddress = umAddress;
            _isCheckingDowntimeEnabled = isCheckingDowntimeEnabled;
            _cacheDuplicatedEmailTimeOut = cacheDuplicatedEmailTimeOut;
        }

        public override void Write(string message)
        {
            var cacheKey = GetMd5Hash(message);

            if (_cacheManager.Contains(cacheKey))
            {
                return;
            }

            _cacheManager.Set(cacheKey, new object(), _cacheDuplicatedEmailTimeOut);

            DowntimeCheckingEmailMessage mailMessage = new DowntimeCheckingEmailMessage(
                    _toAddress,
                    _fromAddress,
                    _subjectLineStarter,
                    _subjectLineEnder,
                    _smtpServer,
                    _smtpPort,
                    message,
                    Formatter,
                    _authenticationMode,
                    _userName,
                    _password,
                    _useSSL);

            mailMessage.Send(_umAddress, _isCheckingDowntimeEnabled);
        }

        public override void WriteLine(string message)
        {
            Write(message);
        }

        public override void TraceData(
            TraceEventCache eventCache,
            string source,
            TraceEventType eventType,
            int id,
            object data)
        {
            if ((Filter == null) || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                var logEntry = data as LogEntry;

                if (logEntry != null)
                {
                    var message = GetLogEntryCombinedMessage(logEntry);
                    var cacheKey = GetMd5Hash(message);

                    if (_cacheManager.Contains(cacheKey))
                    {
                        return;
                    }

                    _cacheManager.Set(cacheKey, new object(), _cacheDuplicatedEmailTimeOut);

                    var emailMessage = new DowntimeCheckingEmailMessage(
                        _toAddress,
                        _fromAddress,
                        _subjectLineStarter,
                        _subjectLineEnder,
                        _smtpServer,
                        _smtpPort,
                        logEntry,
                        Formatter,
                        _authenticationMode,
                        _userName,
                        _password,
                        _useSSL);

                    emailMessage.Send(_umAddress, _isCheckingDowntimeEnabled);
                }
                else if (data is string)
                {
                    Write(data);
                }
                else
                {
                    base.TraceData(eventCache, source, eventType, id, data);
                }
            }
        }

        private string GetLogEntryCombinedMessage(LogEntry logEntry)
        {
            var logCategory = "default";

            if (logEntry.Categories != null && logEntry.Categories.Any())
            {
                logCategory = logEntry.Categories.First();
            }

            var messageParts = new string[]
            {
                logEntry.Message,
                logEntry.MachineName,
                logEntry.Severity.ToString(),
                logCategory
            };
            var message = string.Join(string.Empty, messageParts);

            return message;
        }

        private string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                var stringBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return stringBuilder.ToString();
            }
        }

        protected override string[] GetSupportedAttributes()
        {
            return new string[]
            {
                "formatter",
                "toAddress",
                "fromAddress",
                "subjectLineStarter",
                "subjectLineEnder",
                "smtpServer",
                "smtpPort",
                "umAddress",
                "isCheckingDowntimeEnabled",
                "cacheDuplicatedEmailTimeOut"
            };
        }
    }
}