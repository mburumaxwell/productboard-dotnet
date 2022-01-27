using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Represents the parent of a component on productboard.</summary>
public record ComponentParent
{
    ///
    [JsonPropertyName("product")]
    public EntityParent? Product { get; set; }

    ///
    [JsonPropertyName("component")]
    public EntityParent? Component { get; set; }
}
