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

#if NET40

    using ContainerModel = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

#endif

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
        private const string PasswordProperty = "password";
        private const string UMAddressProperty = "umAddress";
        private const string IsCheckingDowntimeEnabledProperty = "isCheckingDowntimeEnabled";
        private const string CacheDuplicatedEmailTimeOutProperty = "cacheDuplicatedEmailTimeOut";

        public CustomEmailTraceListenerData()
            : base(typeof(CustomEmailTraceListener))
        {
            ListenerDataType = typeof(CustomEmailTraceListenerData);
        }

        public CustomEmailTraceListenerData(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            string formatterName)
            : this(
                  toAddress,
                  fromAddress,
                  subjectLineStarter,
                  subjectLineEnder,
                  smtpServer,
                  25,
                  formatterName)
        {
        }

        public CustomEmailTraceListenerData(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            string formatterName)
            : this(
                  "unnamed",
                  toAddress,
                  fromAddress,
                  subjectLineStarter,
                  subjectLineEnder,
                  smtpServer,
                  smtpPort,
                  formatterName)
        {
        }

        public CustomEmailTraceListenerData(
            string name,
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            string formatterName)
            : this(
                  name,
                  toAddress,
                  fromAddress,
                  subjectLineStarter,
                  subjectLineEnder,
                  smtpServer,
                  smtpPort,
                  formatterName,
                  TraceOptions.None)
        {
        }

        public CustomEmailTraceListenerData(
            string name,
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            string formatterName,
            TraceOptions traceOutputOptions)
            : base(name, typeof(CustomEmailTraceListener), traceOutputOptions)
        {
            ToAddress = toAddress;
            FromAddress = fromAddress;
            SubjectLineStarter = subjectLineStarter;
            SubjectLineEnder = subjectLineEnder;
            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
            Formatter = formatterName;
        }

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
            SourceLevels filter)
            : base(name, typeof(CustomEmailTraceListener), traceOutputOptions, filter)
        {
            ToAddress = toAddress;
            FromAddress = fromAddress;
            SubjectLineStarter = subjectLineStarter;
            SubjectLineEnder = subjectLineEnder;
            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
            Formatter = formatterName;
        }

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
            ToAddress = toAddress;
            FromAddress = fromAddress;
            SubjectLineStarter = subjectLineStarter;
            SubjectLineEnder = subjectLineEnder;
            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
            Formatter = formatterName;
            AuthenticationMode = authenticationMode;
            UserName = userName;
            Password = password;
            UseSSL = useSSL;
        }

        [ConfigurationProperty(ToAddressProperty, IsRequired = true)]
        [Description("The 'To' address to use for the email message created for each log entry.")]
        [DisplayName("To Address")]
        [DesigntimeDefaultAttribute("to@example.com")]
        public string ToAddress
        {
            get { return base[ToAddressProperty] as string; }
            set { base[ToAddressProperty] = value; }
        }

        [ConfigurationProperty(FromAddressProperty, IsRequired = true)]
        [Description("The 'From' address to use for the email message created for each log entry.")]
        [DisplayName("From Address")]
        [DesigntimeDefaultAttribute("from@example.com")]
        public string FromAddress
        {
            get { return base[FromAddressProperty] as string; }
            set { base[FromAddressProperty] = value; }
        }

        [ConfigurationProperty(SubjectLineStarterProperty)]
        [Description("The prefix to add at the start of the Subject for this email message.")]
        [DisplayName("Subject Line Prefix")]
        public string SubjectLineStarter
        {
            get { return base[SubjectLineStarterProperty] as string; }
            set { base[SubjectLineStarterProperty] = value; }
        }

        [ConfigurationProperty(SubjectLineEnderProperty)]
        [Description("The suffix to add to the end of the Subject for this email message.")]
        [DisplayName("Subject Line Suffix")]
        public string SubjectLineEnder
        {
            get { return base[SubjectLineEnderProperty] as string; }
            set { base[SubjectLineEnderProperty] = value; }
        }

        [ConfigurationProperty(SmtpServerProperty, DefaultValue = "127.0.0.1")]
        [Description("The name or IP address of the server that will send the email message. The default if not specified is the local host (127.0.0.1).")]
        [DisplayName("Smtp Server")]
        public string SmtpServer
        {
            get { return base[SmtpServerProperty] as string; }
            set { base[SmtpServerProperty] = value; }
        }

        [ConfigurationProperty(SmtpPortProperty, DefaultValue = 25)]
        [Description("The port to use to send the email message to the server that will despatch it. The default if not specified is the SMTP port 25.")]
        [DisplayName("Smtp Port")]
        public int SmtpPort
        {
            get { return (int)base[SmtpPortProperty]; }
            set { base[SmtpPortProperty] = value; }
        }

        [ConfigurationProperty(FormatterNameProperty, IsRequired = false)]
        [Reference(typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>), typeof(FormatterData))]
        [Description("The name of the Log Message Formatter to use for formatting the log message before sending it to the logging target.")]
        [DisplayName("Formatter Name")]
        public string Formatter
        {
            get { return base[FormatterNameProperty] as string; }
            set { base[FormatterNameProperty] = value; }
        }

        [ConfigurationProperty(AuthenticationModeProperty, IsRequired = false, DefaultValue = EmailAuthenticationMode.None)]
        [Description(@"A value from the EmailAuthenticationMode enumeration that specifies how the listener will authenticate the user.
                       Valid values are None, WindowsCredentials, and UserNameAndPassword.")]
        [DisplayName("Authentication  Mode ")]
        public EmailAuthenticationMode AuthenticationMode
        {
            get { return (EmailAuthenticationMode)base[AuthenticationModeProperty]; }
            set { base[AuthenticationModeProperty] = value; }
        }

        [ConfigurationProperty(UseSSLProperty, IsRequired = false, DefaultValue = false)]
        [Description("A boolean value that specifies if the listener will authenticate and send messages using Secure Sockets Layer (SSL) encrypted communication.")]
        [DisplayName("Use SSL")]
        public bool UseSSL
        {
            get { return (bool)base[UseSSLProperty]; }
            set { base[UseSSLProperty] = value; }
        }

        [ConfigurationProperty(UserNameProperty, IsRequired = false)]
        [Description("The user name of the user for authentication when sending email messages to the mail server that will relay them.  ")]
        [DisplayName("Authentication  User Name")]
        public string UserName
        {
            get { return base[UserNameProperty] as string; }
            set { base[UserNameProperty] = value; }
        }

        [ConfigurationProperty(PasswordProperty, IsRequired = false)]
        [Description("The password to use when authenticating the specified user name.")]
        [DisplayName("Authentication Password")]
        public string Password
        {
            get { return base[PasswordProperty] as string; }
            set { base[PasswordProperty] = value; }
        }

        [ConfigurationProperty(UMAddressProperty, IsRequired = true, DefaultValue = "tcp://10.40.40.150:8990/UMControllerInterface")]
        [Description("UM url for checking downtime.")]
        [DisplayName("UM Address")]
        public string UMAddress
        {
            get { return base[UMAddressProperty] as string; }
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

#if NET40

        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () =>
                    new CustomEmailTraceListener(
                        ToAddress,
                        FromAddress,
                        SubjectLineStarter,
                        SubjectLineEnder,
                        SmtpServer,
                        SmtpPort,
                        ContainerModel.Container.ResolvedIfNotNull<ILogFormatter>(Formatter),
                        AuthenticationMode,
                        UserName,
                        Password,
                        UseSSL,
                        UMAddress,
                        IsCheckingDowntimeEnabled,
                        CacheDuplicatedEmailTimeOut);
        }

#else

        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            var formatter = BuildFormatterSafe(settings, Formatter);

            return new CustomEmailTraceListener(
                        ToAddress,
                        FromAddress,
                        SubjectLineStarter,
                        SubjectLineEnder,
                        SmtpServer,
                        SmtpPort,
                        formatter,
                        AuthenticationMode,
                        UserName,
                        Password,
                        UseSSL,
                        UMAddress,
                        IsCheckingDowntimeEnabled,
                        CacheDuplicatedEmailTimeOut);
        }

#endif
    }
}