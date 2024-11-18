namespace Fanex.Logging.Extensions.Database;

using System;
using System.Collections.Generic;
using System.Linq;

internal class ConnectionStringBuilder(string connectionString)
{
    private const int SplitIndex = 2;
    internal const string DefaultServerKeyword = "data source";
    internal const string DefaultUserIdKeyword = "user id";

    internal static readonly string[] ServerAliases = [DefaultServerKeyword, "server", "host", "datasource", "address", "addr", "network address"
    ];
    internal static readonly string[] UserIdAliases = [DefaultUserIdKeyword, "uid", "username", "user name", "user", "userid"
    ];

    private readonly IDictionary<string, string> connectionStringKeyValuePairs = BuildConnectionStringKeyValues(connectionString
        ?? throw new ArgumentNullException(nameof(connectionString)));

    private static IDictionary<string, string> BuildConnectionStringKeyValues(string connectionString)
        => connectionString.Split(';')
            .Where(kvp => kvp.Contains('='))
            .Select(kvp => kvp.Split(['='], SplitIndex))
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
            if (connectionStringKeyValuePairs.TryGetValue(keyword, out var value))
            {
                return value;
            }
        }

        return "N/A";
    }
}