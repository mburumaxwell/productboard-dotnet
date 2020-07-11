using Newtonsoft.Json;

namespace productboard.Models
{
    /// <summary>
    /// The result returned after deleting customer data via GDPR API
    /// </summary>
    public class GdprDeletionResult
    {
        /// <summary>
        /// The success message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
