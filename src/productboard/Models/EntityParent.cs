using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Reference to an entity parent.</summary>
public record EntityParent
{
    /// <summary>Entity identifier</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    ///
    [JsonPropertyName("links")]
    public ResourceLinks? Links { get; set; }
}
