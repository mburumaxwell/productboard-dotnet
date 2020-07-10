using Newtonsoft.Json;

namespace productboard
{
    /// <summary>
    /// The result returned after creating a note
    /// </summary>
    public class CreatedNoteResult
    {
        ///
        [JsonProperty("links")]
        public NoteLinks Links { get; set; }

        ///
        [JsonProperty("data")]
        public CreatedNote Data { get; set; }
    }
}
