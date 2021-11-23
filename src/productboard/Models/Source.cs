using System.Text.Json.Serialization;

namespace productboard;

/// <summary>
/// Represents the source of the Note
/// </summary>
/// <remarks>
/// For entities that originated in external systems and entered
/// productboard via the API or integrations, the source keeps track of the
/// original source entity in that origin system(s)
/// </remarks>
public class Source
{
    /// <summary>
    /// A unique string identifying the external system from which the data came
    /// </summary>
    /// <example>deskdesk</example>
    [JsonPropertyName("origin")]
    public string? Origin { get; set; }

    /// <summary>
    /// The unique id of the record in the origin system
    /// </summary>
    /// <example>deskdesk</example>
    [JsonPropertyName("record_id")]
    public string? RecordId { get; set; }
}
