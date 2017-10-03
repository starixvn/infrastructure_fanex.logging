namespace Fanex.Logging.Extensions
{
    using System.Collections.Generic;

    internal static class Constants
    {
        public const string NewLine = "\r\n";
        public const string Tab = "\t";
        public const string Segment = "\r\n\t";
        public const string NotAvailableAbbr = "N/A";
        public const string SensitiveInfo = "*****";
        public const string QueryStringParamRegexPattern = "(?:{0}=)(?<paramValue>[^&|^\\s]+)";
        public const string NoInfomration = "No Information is available.";
        public const string BodyDataJsonParamRegexPattern = "(\"{0}\":)(?<paramValue>[^,|^\r]+)";
        public readonly static IDictionary<string, object> DefaultDictionary = new Dictionary<string, object>();
    }
}