namespace Fanex.Logging.Extensions
{
    using System.Collections.Generic;

    internal static class Constants
    {
        public const string NotAvailableAbbr = "N/A";
        public const string SensitiveInfo = "*****";
        public const string NoInfomration = "No Information is available.";
        public const string ParamValue = "paramValue";
        private readonly static IDictionary<string, object> emptyDictionary = new Dictionary<string, object>();

        public static IDictionary<string, object> EmptyDictionary
        {
            get
            {
                return emptyDictionary;
            }
        }
    }
}