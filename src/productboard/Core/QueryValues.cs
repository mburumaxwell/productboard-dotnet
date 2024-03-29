﻿using System.Collections;
using System.Text;
using System.Text.Encodings.Web;

namespace productboard.Core;

/// <summary>Helper for handling query values.</summary>
internal class QueryValues : IEnumerable<KeyValuePair<string, string>>
{
    private readonly Dictionary<string, string> values;

    ///
    public QueryValues(Dictionary<string, string>? values = null)
    {
        // keys are case insensitive
        this.values = values == null
            ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string>(values, StringComparer.OrdinalIgnoreCase);
    }

    ///
    public QueryValues Add(string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            values.Add(key, value!);
        }

        return this;
    }

    ///
    public QueryValues Add(string key, object? value)
    {
        if (value is null) return this;

        return value switch
        {
            bool b => Add(key, b.ToString().ToLowerInvariant()),
            DateTimeOffset dto => Add(key, dto.ToString("O")),
            DateTime dt => Add(key, dt.ToString("O")),
            int i => Add(key, i.ToString()),
            long l => Add(key, l.ToString()),
            string s when !string.IsNullOrWhiteSpace(s) => Add(key, s),
            _ => throw new InvalidOperationException($"'{value.GetType().FullName}' objects are not supported"),
        };
    }

    ///
    internal Dictionary<string, string> ToDictionary() => values;

    private string Generate()
    {
        var first = true;
        var builder = new StringBuilder();
        foreach (var parameter in values)
        {
            if (parameter.Value is null) continue;

            builder.Append(first ? '?' : '&');
            builder.Append(UrlEncoder.Default.Encode(parameter.Key));
            builder.Append('=');
            builder.Append(UrlEncoder.Default.Encode(parameter.Value));
            first = false;
        }

        return builder.ToString();
    }

    /// <inheritdoc/>
    public override string ToString() => Generate();

    #region IEnumerable

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

}