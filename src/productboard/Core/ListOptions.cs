namespace productboard.Core;

/// <summary>
/// Standard options for pagination in list/get operations.
/// </summary>
public record ListOptions
{
    private static readonly System.Text.RegularExpressions.Regex pageLimitRegex = new("pageLimit=([0-9]{1,})");
    private static readonly System.Text.RegularExpressions.Regex pageOffsetRegex = new("pageOffset=([0-9]{1,})");

    ///
    public int PageLimit { get; set; }

    ///
    public int PageOffset { get; set; }

    ///
    public void FromPaginationLinks(PaginationLinks links)
    {
        if (links == null) throw new ArgumentNullException(nameof(links));

        var next = links.Next;
        if (next is null) return;

        // Parse from url's query parameters
        // Example: https://api.productboard.com/entity?pageLimit=100&pageOffset=100
        var pageLimitMatch = pageLimitRegex.Match(next);
        var pageOffsetMatch = pageOffsetRegex.Match(next);
        if (pageLimitMatch.Success && pageOffsetMatch.Success)
        {
            PageLimit = int.Parse(pageLimitMatch.Groups[1].Value);
            PageOffset = int.Parse(pageOffsetMatch.Groups[1].Value);
        }

        return;
    }

    internal virtual void Populate(QueryValues values)
    {
        if (values is null) throw new ArgumentNullException(nameof(values));

        values.Add("pageLimit", PageLimit)
              .Add("pageOffset", PageOffset);
    }
}
