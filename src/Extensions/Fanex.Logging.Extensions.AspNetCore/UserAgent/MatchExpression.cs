namespace Fanex.Logging.Extensions.AspNetCore.UserAgent;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MatchExpression
{
    public List<Regex> Regexes { get; set; }

    public Action<Match, object> Action { get; set; }
}