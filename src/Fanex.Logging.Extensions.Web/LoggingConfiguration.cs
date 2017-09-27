namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web;

    public class LoggingConfiguration
    {
        static string[] defaultRegexSensitiveParamPatterns = {
            Constants.QueryStringParamRegexPattern,
            Constants.BodyDataJsonParamRegexPattern
        };

        internal Func<Exception, string> GetDbExceptionInfo;
        internal IEnumerable<string> SensitiveParams;
        internal string[] RegexSensitiveParamPatterns;
        internal bool EnabledLogHttpHeader;
        internal bool EnabledLogHttpSession;
        internal List<Type> IgnoredExceptionTypes = new List<Type> { typeof(HttpException), typeof(ThreadAbortException)};

        public LoggingConfiguration UseGetDbExceptionInfo(Func<Exception, string> getDbExceptionInfo)
        {
            if (GetDbExceptionInfo == null
                && getDbExceptionInfo != null)
            {
                GetDbExceptionInfo = getDbExceptionInfo;
            }

            return this;
        }

        public LoggingConfiguration HideSensitiveInfo(
            IEnumerable<string> sensitiveParams,
            params string[] regexSensitiveParamPatterns)
        {
            if (SensitiveParams == null
                && sensitiveParams != null)
            {
                SensitiveParams = sensitiveParams;
                RegexSensitiveParamPatterns = regexSensitiveParamPatterns ?? defaultRegexSensitiveParamPatterns;
            }

            return this;
        }

        public LoggingConfiguration GetHttpHeaderInfo()
        {
            EnabledLogHttpHeader = true;            

            return this;
        }

        public LoggingConfiguration GetHttpSessionInfo()
        {
            EnabledLogHttpSession = true;

            return this;
        }

        /// <summary>
        /// Ingore HttpException and ThreadAbortException
        /// </summary>
        /// <param name="exceptionTypes"></param>
        /// <returns>instance of WebLoggingConfiguration</returns>
        public LoggingConfiguration IgnoreExceptionTypes(params Type[] exceptionTypes)
        {
            IgnoredExceptionTypes.AddRange(exceptionTypes);

            return this;
        }

        /// <summary>
        /// Ingore HttpException and ThreadAbortException 
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <returns>instance of WebLoggingConfiguration</returns>
        public LoggingConfiguration IgnoreException<TException>()
        {
            IgnoredExceptionTypes.Add(typeof(TException));

            return this;
        }
    }
}