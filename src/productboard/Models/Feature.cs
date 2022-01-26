using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Represents a feature on productboard.</summary>
public record Feature : IHasResourceLinks
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

    /// <summary>Feature type. Can be either <c>feature</c> or <c>subFeature</c>.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>A flag denoting if the feature is archived</summary>
    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    /// <summary>Basic feature status. Please query feature statuses API for more details.</summary>
    [JsonPropertyName("status")]
    public FeatureStatus? Status { get; set; }

    // TODO: handle parent (AnyOf: Product/Component/Feature)

    /// <summary>Links for accessing the component.</summary>
    [JsonPropertyName("links")]
    public ResourceLinks? Links { get; set; }

    /// <summary>Feature timeframe.</summary>
    [JsonPropertyName("timeframe")]
    public FeatureTimeframe? Timeframe { get; set; }
}
