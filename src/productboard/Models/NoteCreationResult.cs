using System.Text.Json.Serialization;

namespace productboard.Models
{
    /// <summary>
    /// The result returned after creating a note
    /// </summary>
    public class NoteCreationResult
    {
        /// <summary>
        /// Links for accessing the created note
        /// </summary>
        [JsonPropertyName("links")]
        public NoteLinks Links { get; set; }

        /// <summary>
        /// Data about the created note
        /// </summary>
        [JsonPropertyName("data")]
        public NoteData Data { get; set; }
    }
}
