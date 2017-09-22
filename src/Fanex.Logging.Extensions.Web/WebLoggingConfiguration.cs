namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web;

    public class WebLoggingConfiguration
    {
        static string[] defaultRegexSensitiveParamPatterns = {
            LogUtility.QueryStringParamRegexPattern,
            LogUtility.BodyDataJsonParamRegexPattern
        };

        internal Func<Exception, string> GetDatabaseInfoFunc;
        internal IEnumerable<string> SensitiveParams;
        internal string[] RegexSensitiveParamPatterns;
        internal bool EnabledLogHttpHeader;
        internal bool EnabledLogHttpSession;
        internal List<Type> IgnoredExceptionTypes = new List<Type> { typeof(HttpException), typeof(ThreadAbortException)};

        public WebLoggingConfiguration UseGetDatabaseInfoFunc(Func<Exception, string> getDatabaseInfoFunc)
        {
            if (GetDatabaseInfoFunc == null
                && getDatabaseInfoFunc != null)
            {
                GetDatabaseInfoFunc = getDatabaseInfoFunc;
            }

            return this;
        }

        public WebLoggingConfiguration CensorSensitiveValueForTheseParams(
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

        public WebLoggingConfiguration LogHttpHeaderInfo()
        {
            EnabledLogHttpHeader = true;            

            return this;
        }

        public WebLoggingConfiguration LogHttpSessionInfo()
        {
            EnabledLogHttpSession = true;

            return this;
        }

        /// <summary>
        /// Ingore HttpException and ThreadAbortException
        /// </summary>
        /// <param name="exceptionTypes"></param>
        /// <returns>instance of WebLoggingConfiguration</returns>
        public WebLoggingConfiguration IgnoreExceptionTypes(params Type[] exceptionTypes)
        {
            IgnoredExceptionTypes.AddRange(exceptionTypes);

            return this;
        }

        /// <summary>
        /// Ingore HttpException and ThreadAbortException 
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <returns>instance of WebLoggingConfiguration</returns>
        public WebLoggingConfiguration IgnoreException<TException>()
        {
            IgnoredExceptionTypes.Add(typeof(TException));

            return this;
        }
    }
}