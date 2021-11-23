using System.Text.Json.Serialization;

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
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        /// <summary>
        /// The errors reported
        /// </summary>
        [JsonPropertyName("errors")]
        public ProductboardErrors Errors { get; set; }

        /// <summary>
        /// The error message
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; } // used by GDPR only
    }
}
