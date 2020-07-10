using Newtonsoft.Json;

namespace productboard
{
    /// <summary>
    /// The result returned after creating a note
    /// </summary>
    public class NoteCreationResult
    {
        /// <summary>
        /// Links for accessing the created note
        /// </summary>
        [JsonProperty("links")]
        public NoteLinks Links { get; set; }

        /// <summary>
        /// Data about the created note
        /// </summary>
        [JsonProperty("data")]
        public NoteData Data { get; set; }
    }
}
