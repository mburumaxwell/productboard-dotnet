using Newtonsoft.Json;

namespace productboard
{
    ///
    public class NoteLinks
    {
        /// <summary>
        /// Note is accessible via this URL in the productboard application
        /// </summary>
        /// <example>https://space.productboard.com/inbox/notes/123456</example>
        [JsonProperty("html")]
        public string Html { get; set; }
    }
}
