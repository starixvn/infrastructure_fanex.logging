namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;
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
        internal List<Type> IgnoredExceptionTypes = new List<Type>();

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

        public WebLoggingConfiguration LogSessionInfo()
        {
            EnabledLogHttpSession = true;

            return this;
        }

        public WebLoggingConfiguration AddIgnoreExceptionTypes(params Type[] exceptionTypes)
        {
            IgnoredExceptionTypes.AddRange(exceptionTypes);

            return this;
        }

        public WebLoggingConfiguration IgnoreException<TException>()
        {
            IgnoredExceptionTypes.Add(typeof(TException));

            return this;
        }
    }
}