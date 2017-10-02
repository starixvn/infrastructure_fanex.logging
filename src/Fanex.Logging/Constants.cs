namespace Fanex.Logging
{
    using System.Collections.Generic;

    internal static class Constants
    {
        public const string NewLine = "\r\n";
        public const string Tab = "\t";
        public const string Segment = "\r\n\t";
        public const string QueryStringParamRegexPattern = "(?:{0}=)(?<paramValue>[^&|^\\s]+)";
        public const string BodyDataJsonParamRegexPattern = "(\"{0}\":)(?<paramValue>[^,|^\r]+)";
        public const string NoInfomration = "No Information is available.";
        public const string NotAvailableAbbr = "N/A";
        private static readonly List<string> noInformationContent = new List<string> { NoInfomration };

        public static List<string> NoInformationContent
        {
            get
            {
                return noInformationContent;
            }
        }
    }
}