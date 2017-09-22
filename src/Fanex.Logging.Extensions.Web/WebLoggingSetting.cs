namespace Fanex.Logging.Extensions.Web
{
    using System;
    using System.Collections.Generic;

    public class WebLoggingSetting
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

        public WebLoggingSetting UseGetDatabaseInfoFunc(Func<Exception, string> getDatabaseInfoFunc)
        {
            if (GetDatabaseInfoFunc == null
                && getDatabaseInfoFunc != null)
            {
                GetDatabaseInfoFunc = getDatabaseInfoFunc;
            }

            return this;
        }

        public WebLoggingSetting CensorSensitiveValueForTheseParams(
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

        public WebLoggingSetting EnableLogHttpHeader()
        {
            EnabledLogHttpHeader = true;

            return this;
        }

        public WebLoggingSetting EnableLogHttpSession()
        {
            EnabledLogHttpSession = true;

            return this;
        }
    }
}