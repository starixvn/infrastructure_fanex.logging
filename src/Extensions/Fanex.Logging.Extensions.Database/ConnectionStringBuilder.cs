namespace Fanex.Logging.Extensions.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ConnectionStringBuilder
    {
        private const int splitIndex = 2;
        internal const string DefaultServerKeyword = "data source";
        internal const string DefaultUserIdKeyword = "user id";

        internal static readonly string[] ServerAliases = { DefaultServerKeyword, "server", "host", "datasource", "address", "addr", "network address" };
        internal static readonly string[] UserIdAliases = { DefaultUserIdKeyword, "uid", "username", "user name", "user", "userid" };

        private readonly IDictionary<string, string> connectionStringKeyValuePairs;

        public ConnectionStringBuilder(string connectionString)
        {
            connectionStringKeyValuePairs = BuildConnectionStringKeyValues(connectionString
                ?? throw new ArgumentNullException(nameof(connectionString)));
        }

        private static IDictionary<string, string> BuildConnectionStringKeyValues(string connectionString)
            => connectionString.Split(';')
                            .Where(kvp => kvp.Contains('='))
                            .Select(kvp => kvp.Split(new[] { '=' }, splitIndex))
                            .ToDictionary(kvp => kvp[0].Trim(),
                                            kvp => kvp[1].Trim(),
                                            StringComparer.InvariantCultureIgnoreCase);

        public string GetServer()
            => GetValue(ServerAliases);

        public string GetUserId()
            => GetValue(UserIdAliases);

        private string GetValue(string[] keywordAliases)
        {
            foreach (var keyword in keywordAliases)
            {
                if (connectionStringKeyValuePairs.ContainsKey(keyword))
                {
                    return connectionStringKeyValuePairs[keyword];
                }
            }

            return "N/A";
        }
    }
}