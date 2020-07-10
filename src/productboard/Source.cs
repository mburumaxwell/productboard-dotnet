using Newtonsoft.Json;

namespace productboard
{
    /// <summary>
    /// For entities that originated in external systems and entered
    /// productboard via the API or integrations, the source keeps track of the
    /// original source entity in that origin system(s)
    /// </summary>
    public class Source
    {
        /// <summary>
        /// A unique string identifying the external system from which the data came
        /// </summary>
        /// <example>deskdesk</example>
        [JsonProperty("origin")]
        public string Origin { get; set; }

        /// <summary>
        /// The unique id of the record in the origin system
        /// </summary>
        /// <example>deskdesk</example>
        [JsonProperty("record_id")]
        public string RecordId { get; set; }
    }
}
