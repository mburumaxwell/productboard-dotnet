using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Options for updating a feature.</summary>
public class FeatureUpdateOptions
{
    /// <summary>
    /// New feature name or null if it should not be changed.
    /// Note that empty string is allowed and actually means to set the name to an empty value as opposed to null.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// New feature description or null if it should not be changed.
    /// The format is limited to a subset of HTML as defined
    /// by <see href="https://developer.productboard.com/files/schema.xsd">productboard's XML schema</see>.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// A flag denoting if the feature is archived.
    /// If null, a default value will be filled (false).
    /// </summary>
    [JsonPropertyName("archived")]
    public bool? Archived { get; set; }

    /// <summary>
    /// Feature status.
    /// It is not possible to specify both <see cref="FeatureStatus.Id"/>
    /// and <see cref="FeatureStatus.Name"/>, only one of them can be used.
    /// </summary>
    [JsonPropertyName("status")]
    public FeatureStatus? Status { get; set; }

    // TODO: handle parent (AnyOf: Product/Component/Feature)

    /// <summary>
    /// Feature timeframe.
    /// If null, the timeframe will not be set.
    /// </summary>
    [JsonPropertyName("timeframe")]
    public FeatureTimeframe? Timeframe { get; set; }
}
