namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;

    public class WebLoggingSession
    {
        private static string[] defualtRegexSensitiveParamPatterns = new string[]
        {
            LogUtility.QueryStringParamRegexPattern,
            LogUtility.BodyDataJsonParamRegexPattern
        };

        internal Func<Exception, string> GetDatabaseInfoFunc;
        internal IEnumerable<string> SensitiveParams;
        internal string[] RegexSensitiveParamPatterns;
        internal bool EnabledLogHttpHeader;
        internal bool EnabledLogHttpSession;

        public WebLoggingSession UseGetDatabaseInfoFunc(Func<Exception, string> getDatabaseInfoFunc)
        {
            if (GetDatabaseInfoFunc == null
                && getDatabaseInfoFunc != null)
            {
                GetDatabaseInfoFunc = getDatabaseInfoFunc;
            }

            return this;
        }

        public WebLoggingSession CensorSensitiveValueForTheseParams(
            IEnumerable<string> sensitiveParams,
            params string[] regexSensitiveParamPatterns)
        {
            if (SensitiveParams == null
                && sensitiveParams != null)
            {
                SensitiveParams = sensitiveParams;
                RegexSensitiveParamPatterns = regexSensitiveParamPatterns ?? defualtRegexSensitiveParamPatterns;
            }

            return this;
        }

        public WebLoggingSession EnableLogHttpHeader()
        {
            EnabledLogHttpHeader = true;

            return this;
        }

        public WebLoggingSession EnableLogHttpSession()
        {
            EnabledLogHttpSession = true;

            return this;
        }
    }
}