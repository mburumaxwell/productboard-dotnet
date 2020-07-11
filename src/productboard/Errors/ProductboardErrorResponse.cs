using Newtonsoft.Json;

namespace productboard.Errors
{
    /// <summary>
    /// The result sent incase of an error from the Public APIs
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
