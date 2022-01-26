using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Links for a resource on productboard.</summary>
public record ResourceLinks
{
    /// <summary>Link of the resource on the API.</summary>
    [JsonPropertyName("self")]
    public string? Self { get; set; }

    /// <summary>Link of the resource in the productboard application.</summary>
    /// <example>https://space.productboard.com/inbox/notes/123456</example>
    [JsonPropertyName("html")]
    public string? Html { get; set; }
}