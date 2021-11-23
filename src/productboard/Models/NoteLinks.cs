using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>
/// Represents linkds for a created note
/// </summary>
public class NoteLinks
{
    /// <summary>
    /// Note is accessible via this URL in the productboard application
    /// </summary>
    /// <example>https://space.productboard.com/inbox/notes/123456</example>
    [JsonPropertyName("html")]
    public string? Html { get; set; }
}
