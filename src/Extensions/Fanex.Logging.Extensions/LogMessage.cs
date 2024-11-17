namespace Fanex.Logging.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LogMessage
{
    private readonly StringBuilder contentBuilder = new();

    public LogMessage() : this(string.Empty)
    {
    }

    public LogMessage(string message)
    {
        AddMessage(message);
    }

    private void AddSection(string section)
    {
        contentBuilder
            .AppendLine()
            .AppendLine(section.ToUpperInvariant());
    }

    public LogMessage AddSection(string section,
        Func<IDictionary<string, object>> dataGetter)
    {
        if (!ReferenceEquals(null, dataGetter))
        {
            AddSection(section, dataGetter());
        }

        return this;
    }

    public LogMessage AddSection(string section,
        IDictionary<string, object> data)
    {
        if (string.IsNullOrWhiteSpace(section)
            || ReferenceEquals(null, data))
        {
            return this;
        }

        AddSection(section);

        if (data.Keys.Any())
        {
            AddData(data);
        }
        else
        {
            AddMessage(Constants.NoInformation);
        }

        return this;
    }

    public LogMessage AddData(IDictionary<string, object> data)
    {
        if (ReferenceEquals(data, null))
        {
            return this;
        }

        foreach (var item in data)
        {
            contentBuilder.AppendLine($"\t{item.Key}: {item.Value}");
        }

        return this;
    }

    public LogMessage AddData(Func<IDictionary<string, object>> dataGetter)
    {
        if (!ReferenceEquals(null, dataGetter))
        {
            AddData(dataGetter());
        }

        return this;
    }

    public LogMessage AddMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            contentBuilder.AppendLine(message);
        }

        return this;
    }

    public override string ToString() => contentBuilder.ToString();
}