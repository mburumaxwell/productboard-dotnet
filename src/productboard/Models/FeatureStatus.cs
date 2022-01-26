using System.Text.Json.Serialization;

namespace productboard.Models;

///
public record FeatureStatus
{
    /// <summary>Entity identifier</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Human readable representation of the status.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
