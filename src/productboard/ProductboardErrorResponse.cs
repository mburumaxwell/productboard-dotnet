using Newtonsoft.Json;

namespace productboard
{
    /// <summary>
    /// The result send incase of an error
    /// </summary>
    public class ProductboardErrorResponse
    {
        /// <summary>
        /// Indicated if the request was successful
        /// </summary>
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        /// <summary>
        /// The errors reported
        /// </summary>
        [JsonProperty("errors")]
        public ProductboardErrors Errors { get; set; }
    }
}
