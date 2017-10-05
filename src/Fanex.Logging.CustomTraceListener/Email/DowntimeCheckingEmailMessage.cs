namespace Fanex.Logging.CustomTraceListener.Email
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
    using Sunplus.UMControllerLibrary;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    public class DowntimeCheckingEmailMessage : EmailMessage
    {
        public DowntimeCheckingEmailMessage(EmailTraceListenerData configurationData, LogEntry logEntry, ILogFormatter formatter)
            : base(configurationData, logEntry, formatter)
        {
        }

        public DowntimeCheckingEmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, LogEntry logEntry, ILogFormatter formatter)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, logEntry, formatter)
        {
        }

        public DowntimeCheckingEmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string message, ILogFormatter formatter)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, message, formatter)
        {
        }

        public DowntimeCheckingEmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, LogEntry logEntry, ILogFormatter formatter, EmailAuthenticationMode authenticationMode, string userName, string password, bool useSSL)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, logEntry, formatter, authenticationMode, userName, password, useSSL)
        {
        }

        public DowntimeCheckingEmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string message, ILogFormatter formatter, EmailAuthenticationMode authenticationMode, string userName, string password, bool useSSL)
            : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, message, formatter, authenticationMode, userName, password, useSSL)
        {
        }

        public void Send(string umAddress, bool isCheckingDowntimeEnabled = true)
        {
            if (this.IsDowntime(umAddress, isCheckingDowntimeEnabled))
            {
                return;
            }

            base.Send();
        }

        private bool IsDowntime(string umAddress, bool isCheckingDowntimeEnabled)
        {
            var isDowntime = false;

            if (isCheckingDowntimeEnabled)
            {
                string timeFrom = string.Empty;
                string timeTo = string.Empty;
                var downTimeController = new UMBusiness();

                // gets Agent Site UM Status.
                isDowntime = downTimeController.isUnderMaintainence(
                    Sites.AccountSite,
                    umAddress,
                    ref timeFrom,
                    ref timeTo);
            }

            return isDowntime;
        }
    }
}