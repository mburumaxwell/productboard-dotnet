using productboard.Core;
using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>
/// The result returned after creating a note
/// </summary>
public class NoteCreationResult : DataResource<NoteData>, IHasResourceLinks
{
    /// <summary>Links for accessing the created note.</summary>
    [JsonPropertyName("links")]
    public ResourceLinks? Links { get; set; }
}
