[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Web")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Database")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Database.V62")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Database.V59")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.AspNetCore")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.UnitTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Web.UnitTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Database.UnitTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Database.V62.UnitTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.Database.V59.UnitTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Fanex.Logging.Extensions.AspNetCore.UnitTests")]

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