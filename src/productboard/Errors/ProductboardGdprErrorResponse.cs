using Newtonsoft.Json;

namespace productboard.Errors
{
    /// <summary>
    /// The result sent incase of an error from the GDPR API
    /// </summary>
    public class ProductboardGdprErrorResponse
    {
        /// <summary>
        /// The error message
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
