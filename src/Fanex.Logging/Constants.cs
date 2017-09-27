namespace Fanex.Logging
{
    internal static class Constants
    {
        public const string NewLine = "\r\n";
        public const string Tab = "\t";
        public const string QueryStringParamRegexPattern = "(?:{0}=)(?<paramValue>[^&|^\\s]+)";
        public const string BodyDataJsonParamRegexPattern = "(\"{0}\":)(?<paramValue>[^,|^\r]+)";
    }
}
