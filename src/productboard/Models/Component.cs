using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Represents a component on productboard.</summary>
public record Component : IHasResourceLinks
{
    /// <summary>Entity id</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Entity name</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Entity description in a limited subset of HTML as defined
    /// by <see href="https://developer.productboard.com/files/schema.xsd">productboard's XML schema</see>.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Links for accessing the component.</summary>
    [JsonPropertyName("links")]
    public ResourceLinks? Links { get; set; }

    // TODO: handle parent (AnyOf: Product/Component)
}
