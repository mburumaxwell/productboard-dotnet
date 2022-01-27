using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Represents the parent of a feature on productboard.</summary>
public record FeatureParent
{
    ///
    [JsonPropertyName("product")]
    public EntityParent? Product { get; set; }

    ///
    [JsonPropertyName("component")]
    public EntityParent? Component { get; set; }

    ///
    [JsonPropertyName("feature")]
    public EntityParent? Feature { get; set; }
}
