using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Represents a product on productboard.</summary>
public record Product : IHasResourceLinks
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

    /// <summary>Links for accessing the product.</summary>
    [JsonPropertyName("links")]
    public ResourceLinks? Links { get; set; }
}
