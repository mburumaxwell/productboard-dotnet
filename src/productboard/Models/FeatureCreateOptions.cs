using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Options for creating a feature.</summary>
public class FeatureCreateOptions
{
    /// <summary>New feature name. It cannot be empty string.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Feature description. The format is limited to a subset of HTML as defined
    /// by <see href="https://developer.productboard.com/files/schema.xsd">productboard's XML schema</see>.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Feature type. Can be either <c>feature</c> or <c>subFeature</c>.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Feature status.
    /// It is not possible to specify both <see cref="FeatureStatus.Id"/>
    /// and <see cref="FeatureStatus.Name"/>, only one of them can be used.
    /// </summary>
    [JsonPropertyName("status")]
    public FeatureStatus? Status { get; set; }

    // TODO: handle parent (AnyOf: Product/Component/Feature)

    /// <summary>
    /// A flag denoting if the feature is archived.
    /// If null, a default value will be filled (false).
    /// </summary>
    [JsonPropertyName("archived")]
    public bool? Archived { get; set; }

    /// <summary>
    /// Feature timeframe.
    /// If null, the timeframe will not be set.
    /// </summary>
    [JsonPropertyName("timeframe")]
    public FeatureTimeframe? Timeframe { get; set; }
}
