namespace Fanex.Logging.CustomTraceListener.Email
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
    using ContainerModel = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

    [Description("Custom Fanex Email Trace Listener")]
    [DisplayName("Custom Fanex Email Trace Listener")]
    public class CustomEmailTraceListenerData : TraceListenerData
    {
        private const string ToAddressProperty = "toAddress";
        private const string FromAddressProperty = "fromAddress";
        private const string SubjectLineStarterProperty = "subjectLineStarter";
        private const string SubjectLineEnderProperty = "subjectLineEnder";
        private const string SmtpServerProperty = "smtpServer";
        private const string SmtpPortProperty = "smtpPort";
        private const string FormatterNameProperty = "formatter";
        private const string AuthenticationModeProperty = "authenticationMode";
        private const string UseSSLProperty = "useSSL";
        private const string UserNameProperty = "userName";
#pragma warning disable S2068 // Credentials should not be hard-coded
        private const string PasswordProperty = "password";
#pragma warning restore S2068 // Credentials should not be hard-coded
        private const string UMAddressProperty = "umAddress";
        private const string IsCheckingDowntimeEnabledProperty = "isCheckingDowntimeEnabled";
        private const string CacheDuplicatedEmailTimeOutProperty = "cacheDuplicatedEmailTimeOut";

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/>.
        /// </summary>
        public CustomEmailTraceListenerData()
            : base(typeof(CustomEmailTraceListener))
        {
            ListenerDataType = typeof(CustomEmailTraceListenerData);
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress,
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
        /// Default value for the SMTP port is 25
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        /// email message should be formatted</param>
        public CustomEmailTraceListenerData(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, string formatterName)
            : this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25, formatterName)
        {
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress,
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        /// email message should be formatted</param>
        public CustomEmailTraceListenerData(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName)
            : this("unnamed", toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName)
        {
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress,
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
        /// </summary>
        /// <param name="name">The name of this listener</param>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        /// email message should be formatted</param>
        public CustomEmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName)
            : this(name, toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName, TraceOptions.None)
        {
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress,
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name and trace options.
        /// </summary>
        /// <param name="name">The name of this listener</param>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        /// email message should be formatted</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        public CustomEmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName, TraceOptions traceOutputOptions)
            : base(name, typeof(CustomEmailTraceListener), traceOutputOptions)
        {
            this.ToAddress = toAddress;
            this.FromAddress = fromAddress;
            this.SubjectLineStarter = subjectLineStarter;
            this.SubjectLineEnder = subjectLineEnder;
            this.SmtpServer = smtpServer;
            this.SmtpPort = smtpPort;
            this.Formatter = formatterName;
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress,
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name and trace options.
        /// </summary>
        /// <param name="name">The name of this listener</param>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        /// email message should be formatted</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        public CustomEmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, typeof(CustomEmailTraceListener), traceOutputOptions, filter)
        {
            this.ToAddress = toAddress;
            this.FromAddress = fromAddress;
            this.SubjectLineStarter = subjectLineStarter;
            this.SubjectLineEnder = subjectLineEnder;
            this.SmtpServer = smtpServer;
            this.SmtpPort = smtpPort;
            this.Formatter = formatterName;
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress,
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name, trace options
        /// and authentication information.
        /// </summary>
        /// <param name="name">The name of this listener</param>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        /// email message should be formatted</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="authenticationMode">Authenticate mode to use.</param>
        /// <param name="userName">User name to pass to the server if using <see cref="EmailAuthenticationMode.UserNameAndPassword"/>.</param>
        /// <param name="password">Password to pass to the server if using <see cref="EmailAuthenticationMode.UserNameAndPassword"/>.</param>
        /// <param name="useSSL">Connect to the server using SSL?</param>
        public CustomEmailTraceListenerData(
            string name,
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            string formatterName,
            TraceOptions traceOutputOptions,
            SourceLevels filter,
            EmailAuthenticationMode authenticationMode,
            string userName,
            string password,
            bool useSSL)
            : base(
            name,
            typeof(CustomEmailTraceListener),
            traceOutputOptions,
            filter)
        {
            this.ToAddress = toAddress;
            this.FromAddress = fromAddress;
            this.SubjectLineStarter = subjectLineStarter;
            this.SubjectLineEnder = subjectLineEnder;
            this.SmtpServer = smtpServer;
            this.SmtpPort = smtpPort;
            this.Formatter = formatterName;
            this.AuthenticationMode = authenticationMode;
            this.UserName = userName;
            this.Password = password;
            this.UseSSL = useSSL;
        }

        /// <summary>
        /// Gets and sets the ToAddress.  One or more email semicolon separated addresses.
        /// </summary>
        [ConfigurationProperty(ToAddressProperty, IsRequired = true)]
        [Description("The 'To' address to use for the email message created for each log entry.")]
        [DisplayName("To Address")]
        [DesigntimeDefaultAttribute("to@example.com")]
        public string ToAddress
        {
            get { return (string)base[ToAddressProperty]; }
            set { base[ToAddressProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the FromAddress. Email address that messages will be sent from.
        /// </summary>
        [ConfigurationProperty(FromAddressProperty, IsRequired = true)]
        [Description("The 'From' address to use for the email message created for each log entry.")]
        [DisplayName("From Address")]
        [DesigntimeDefaultAttribute("from@example.com")]
        public string FromAddress
        {
            get { return (string)base[FromAddressProperty]; }
            set { base[FromAddressProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the Subject prefix.
        /// </summary>
        [ConfigurationProperty(SubjectLineStarterProperty)]
        [Description("The prefix to add at the start of the Subject for this email message.")]
        [DisplayName("Subject Line Prefix")]
        public string SubjectLineStarter
        {
            get { return (string)base[SubjectLineStarterProperty]; }
            set { base[SubjectLineStarterProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the Subject suffix.
        /// </summary>
        [ConfigurationProperty(SubjectLineEnderProperty)]
        [Description("The suffix to add to the end of the Subject for this email message.")]
        [DisplayName("Subject Line Suffix")]
        public string SubjectLineEnder
        {
            get { return (string)base[SubjectLineEnderProperty]; }
            set { base[SubjectLineEnderProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the SMTP server to use to send messages.
        /// </summary>
        [ConfigurationProperty(SmtpServerProperty, DefaultValue = "127.0.0.1")]
        [Description("The name or IP address of the server that will send the email message. The default if not specified is the local host (127.0.0.1).")]
        [DisplayName("Smtp Server")]
        public string SmtpServer
        {
            get { return (string)base[SmtpServerProperty]; }
            set { base[SmtpServerProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the SMTP port.
        /// </summary>
        [ConfigurationProperty(SmtpPortProperty, DefaultValue = 25)]
        [Description("The port to use to send the email message to the server that will despatch it. The default if not specified is the SMTP port 25.")]
        [DisplayName("Smtp Port")]
        public int SmtpPort
        {
            get { return (int)base[SmtpPortProperty]; }
            set { base[SmtpPortProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the formatter name.
        /// </summary>
        [ConfigurationProperty(FormatterNameProperty, IsRequired = false)]
        [Reference(typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>), typeof(FormatterData))]
        [Description("The name of the Log Message Formatter to use for formatting the log message before sending it to the logging target.")]
        [DisplayName("Formatter Name")]
        public string Formatter
        {
            get { return (string)base[FormatterNameProperty]; }
            set { base[FormatterNameProperty] = value; }
        }

        /// <summary>
        /// How do you authenticate against the email server?
        /// </summary>
        [ConfigurationProperty(AuthenticationModeProperty, IsRequired = false, DefaultValue = EmailAuthenticationMode.None)]
        [Description("A value from the EmailAuthenticationMode enumeration that specifies how the listener will authenticate the user. Valid values are None, WindowsCredentials, and UserNameAndPassword.")]
        [DisplayName("Authentication  Mode ")]
        public EmailAuthenticationMode AuthenticationMode
        {
            get { return (EmailAuthenticationMode)base[AuthenticationModeProperty]; }
            set { base[AuthenticationModeProperty] = value; }
        }

        /// <summary>
        /// Use SSL to connect to the email server?
        /// </summary>
        [ConfigurationProperty(UseSSLProperty, IsRequired = false, DefaultValue = false)]
        [Description("A boolean value that specifies if the listener will authenticate and send messages using Secure Sockets Layer (SSL) encrypted communication.")]
        [DisplayName("Use SSL")]
        public bool UseSSL
        {
            get { return (bool)base[UseSSLProperty]; }
            set { base[UseSSLProperty] = value; }
        }

        /// <summary>
        /// User name when authenticating with user name and password.
        /// </summary>
        [ConfigurationProperty(UserNameProperty, IsRequired = false)]
        [Description("The user name of the user for authentication when sending email messages to the mail server that will relay them.  ")]
        [DisplayName("Authentication  User Name")]
        public string UserName
        {
            get { return (string)base[UserNameProperty]; }
            set { base[UserNameProperty] = value; }
        }

        /// <summary>
        /// Password when authenticating with user name and password.
        /// </summary>
        [ConfigurationProperty(PasswordProperty, IsRequired = false)]
        [Description("The password to use when authenticating the specified user name.")]
        [DisplayName("Authentication Password")]
        public string Password
        {
            get { return (string)base[PasswordProperty]; }
            set { base[PasswordProperty] = value; }
        }

        [ConfigurationProperty(UMAddressProperty, IsRequired = true, DefaultValue = "tcp://10.40.40.150:8990/UMControllerInterface")]
        [Description("UM url for checking downtime.")]
        [DisplayName("UM Address")]
        public string UMAddress
        {
            get { return (string)base[UMAddressProperty]; }
            set { base[UMAddressProperty] = value; }
        }

        [ConfigurationProperty(IsCheckingDowntimeEnabledProperty, IsRequired = true, DefaultValue = true)]
        [Description("Enable checking downtime or not.")]
        [DisplayName("Enabled Checking Downtime")]
        public bool IsCheckingDowntimeEnabled
        {
            get { return (bool)base[IsCheckingDowntimeEnabledProperty]; }
            set { base[IsCheckingDowntimeEnabledProperty] = value; }
        }

        [ConfigurationProperty(CacheDuplicatedEmailTimeOutProperty, IsRequired = false, DefaultValue = 120)]
        [Description("Cache duplicated email to not send email in seconds")]
        [DisplayName("Cache Duplicated Email TimeOut In Seconds")]
        public int CacheDuplicatedEmailTimeOut
        {
            get { return (int)base[CacheDuplicatedEmailTimeOutProperty]; }
            set { base[CacheDuplicatedEmailTimeOutProperty] = value; }
        }

        /// <summary>
        /// Returns a lambda expression that represents the creation of the trace listener described by this
        /// configuration object.
        /// </summary>
        /// <returns>A lambda expression to create a trace listener.</returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () =>
                    new CustomEmailTraceListener(
                        this.ToAddress,
                        this.FromAddress,
                        this.SubjectLineStarter,
                        this.SubjectLineEnder,
                        this.SmtpServer,
                        this.SmtpPort,
                        ContainerModel.Container.ResolvedIfNotNull<ILogFormatter>(this.Formatter),
                        this.AuthenticationMode,
                        this.UserName,
                        this.Password,
                        this.UseSSL,
                        this.UMAddress,
                        this.IsCheckingDowntimeEnabled,
                        this.CacheDuplicatedEmailTimeOut);
        }
    }
}