using System.Text.Json.Serialization;

namespace productboard.Models
{
    /// <summary>
    /// Represents data of a created note
    /// </summary>
    public class NoteData
    {
        /// <summary>
        /// The unique identifier of the created note
        /// </summary>
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}
