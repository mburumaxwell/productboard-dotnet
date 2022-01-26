using productboard.Core;
using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>Response for creating a note.</summary>
public class NoteCreationResponse : DataResource<NoteData>, IHasResourceLinks
{
    /// <summary>Links for accessing the created note.</summary>
    [JsonPropertyName("links")]
    public ResourceLinks? Links { get; set; }
}
