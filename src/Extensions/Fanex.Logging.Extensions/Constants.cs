namespace Fanex.Logging.Extensions
{
    using System.Collections.Generic;

    internal static class Constants
    {
        public const string NotAvailableAbbr = "N/A";
        public const string SensitiveInfo = "*****";
        public const string NoInfomration = "No Information is available.";
        private readonly static IDictionary<string, object> defaultDictionary = new Dictionary<string, object>();

        public static IDictionary<string, object> DefaultDictionary
        {
            get
            {
                return defaultDictionary;
            }
        }
    }
}