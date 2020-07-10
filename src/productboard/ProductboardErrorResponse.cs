using Newtonsoft.Json;

namespace productboard
{
    /// <summary>
    /// The result send incase of an error
    /// </summary>
    public class ProductboardErrorResponse
    {
        ///
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        ///
        [JsonProperty("errors")]
        public ProductboardErrors Errors { get; set; }
    }
}
