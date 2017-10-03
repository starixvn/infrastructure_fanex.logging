namespace Fanex.Logging.CustomTraceListener.Email
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Fanex.Logging.CustomTraceListener.Services;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    [ConfigurationElementType(typeof(CustomEmailTraceListenerData))]
    public class CustomEmailTraceListener : CustomFormattedTraceListenerBase
    {
        private static ICacheService _cacheManager = new CacheService();

        private readonly string _toAddress;
        private readonly string _fromAddress;
        private readonly string _subjectLineStarter;
        private readonly string _subjectLineEnder;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly EmailAuthenticationMode _authenticationMode = EmailAuthenticationMode.None;
        private readonly string _userName = string.Empty;
        private readonly string _password = string.Empty;
        private readonly bool _useSSL;
        private readonly string _umAddress;
        private readonly bool _isCheckingDowntimeEnabled;
        private readonly int _cacheDuplicatedEmailTimeOut;

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress,
        /// subjectlinestarter, subjectlinender, smtpserver, and a formatter
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the
        /// email message should be formatted</param>
        public CustomEmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, ILogFormatter formatter, string umAddress, bool isCheckingDowntimeEnabled, int cacheDuplicatedEmailTimeOut)
            : this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25, formatter, umAddress, isCheckingDowntimeEnabled, cacheDuplicatedEmailTimeOut)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress,
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport, and a formatter
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the
        /// email message should be formatted</param>
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
            this._toAddress = toAddress;
            this._fromAddress = fromAddress;
            this._subjectLineStarter = subjectLineStarter;
            this._subjectLineEnder = subjectLineEnder;
            this._smtpServer = smtpServer;
            this._smtpPort = smtpPort;
            this._umAddress = umAddress;
            this._isCheckingDowntimeEnabled = isCheckingDowntimeEnabled;
            this._cacheDuplicatedEmailTimeOut = cacheDuplicatedEmailTimeOut;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress,
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport, and a formatter
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the
        /// email message should be formatted</param>
        /// <param name="authenticationMode">Authentication style to use when connecting to SMTP server.</param>
        /// <param name="userName">User name to use for authentication if using username/password authentication.</param>
        /// <param name="password">Password to use for authentication if using username/password authentication.</param>
        /// <param name="useSSL">Use SSL to connect to mail server if true, regular socket if false.</param>
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
            this._toAddress = toAddress;
            this._fromAddress = fromAddress;
            this._subjectLineStarter = subjectLineStarter;
            this._subjectLineEnder = subjectLineEnder;
            this._smtpServer = smtpServer;
            this._smtpPort = smtpPort;
            this._authenticationMode = authenticationMode;
            this._userName = userName;
            this._password = password;
            this._useSSL = useSSL;
            this._umAddress = umAddress;
            this._isCheckingDowntimeEnabled = isCheckingDowntimeEnabled;
            this._cacheDuplicatedEmailTimeOut = cacheDuplicatedEmailTimeOut;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress,
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport, and a formatter
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        public CustomEmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            string umAddress,
            bool isCheckingDowntimeEnabled,
            int cacheDuplicatedEmailTimeOut)
            : this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25, umAddress, isCheckingDowntimeEnabled, cacheDuplicatedEmailTimeOut)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress,
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        public CustomEmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string umAddress, bool isCheckingDowntimeEnabled, int cacheDuplicatedEmailTimeOut)
            : this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, EmailAuthenticationMode.None, null, null, false, umAddress, isCheckingDowntimeEnabled, cacheDuplicatedEmailTimeOut)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress,
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport and authentication information.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="authenticationMode">Authentication style to use when connecting to SMTP server.</param>
        /// <param name="userName">User name to use for authentication if using username/password authentication.</param>
        /// <param name="password">Password to use for authentication if using username/password authentication.</param>
        /// <param name="useSSL">Use SSL to connect to mail server if true, regular socket if false.</param>
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
            this._toAddress = toAddress;
            this._fromAddress = fromAddress;
            this._subjectLineStarter = subjectLineStarter;
            this._subjectLineEnder = subjectLineEnder;
            this._smtpServer = smtpServer;
            this._smtpPort = smtpPort;
            this._authenticationMode = authenticationMode;
            this._userName = userName;
            this._password = password;
            this._useSSL = useSSL;
            this._umAddress = umAddress;
            this._isCheckingDowntimeEnabled = isCheckingDowntimeEnabled;
            this._cacheDuplicatedEmailTimeOut = cacheDuplicatedEmailTimeOut;
        }

        /// <summary>
        /// Sends an email message given a predefined string
        /// </summary>
        /// <param name="message">The string to write as the email message</param>
        public override void Write(string message)
        {
            var cacheKey = GetMd5Hash(message);
            if (_cacheManager.Contains(cacheKey))
            {
                return;
            }
            else
            {
                _cacheManager.Set(cacheKey, new object(), _cacheDuplicatedEmailTimeOut);
            }

            DowntimeCheckingEmailMessage mailMessage =
                new DowntimeCheckingEmailMessage(
                    _toAddress,
                    _fromAddress,
                    _subjectLineStarter,
                    _subjectLineEnder,
                    _smtpServer,
                    _smtpPort,
                    message,
                    this.Formatter,
                    _authenticationMode,
                    _userName,
                    _password,
                    _useSSL);

            mailMessage.Send(_umAddress, _isCheckingDowntimeEnabled);
        }

        /// <summary>
        /// Sends an email message given a predefined string
        /// </summary>
        /// <param name="message">The string to write as the email message</param>
        public override void WriteLine(string message)
        {
            Write(message);
        }

        /// <summary>
        /// Delivers the trace data as an email message.
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (data is LogEntry)
                {
                    LogEntry logEntry = data as LogEntry;
                    var message = GetLogEntryCombinedMessage(logEntry);

                    var cacheKey = GetMd5Hash(message);
                    if (_cacheManager.Contains(cacheKey))
                    {
                        return;
                    }
                    else
                    {
                        _cacheManager.Set(cacheKey, new object(), _cacheDuplicatedEmailTimeOut);
                    }

                    DowntimeCheckingEmailMessage emailMessage = new DowntimeCheckingEmailMessage(
                        _toAddress,
                        _fromAddress,
                        _subjectLineStarter,
                        _subjectLineEnder,
                        _smtpServer,
                        _smtpPort,
                        logEntry,
                        this.Formatter,
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

            var message = string.Join(string.Empty, new[] { logEntry.Message, logEntry.MachineName, logEntry.Severity.ToString(), logCategory });

            return message;
        }

        private static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// Declare the supported attributes for <see cref="EmailTraceListener"/>
        /// </summary>
        protected override string[] GetSupportedAttributes()
        {
            return new[] { "formatter", "toAddress", "fromAddress", "subjectLineStarter", "subjectLineEnder", "smtpServer", "smtpPort", "umAddress", "isCheckingDowntimeEnabled", "cacheDuplicatedEmailTimeOut" };
        }
    }
}