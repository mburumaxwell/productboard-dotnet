using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Represents a component on productboard.</summary>
public record Component : IHasResourceLinks
{
    /// <summary>Entity identifier</summary>
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

    /// <summary>
    /// Parent of the component.
    /// Can be either a component or a product.
    /// Exactly one of these has to be present.
    /// </summary>
    [JsonPropertyName("parent")]
    public ComponentParent? Parent { get; set; }
}
