using System.Text.Json.Serialization;

namespace productboard.Core;

/// <summary>Links for pagination.</summary>
public record PaginationLinks
{
    /// <summary>Link to the next page or null if this is the last one.</summary>
    /// <example>https://api.productboard.com/entity?pageLimit=100&amp;pageOffset=100</example>
    [JsonPropertyName("next")]
    public string? Next { get; set; }
}
