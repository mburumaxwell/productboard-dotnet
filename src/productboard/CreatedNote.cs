using Newtonsoft.Json;

namespace productboard
{
    ///
    public class CreatedNote
    {
        /// <example>d290f1ee-6c54-4b01-90e6-d701748f0851</example>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
