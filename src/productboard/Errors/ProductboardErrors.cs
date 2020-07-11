using Newtonsoft.Json;

namespace productboard.Errors
{
    /// <summary>
    /// The detailed errors used with <see cref="ProductboardErrorResponse"/>
    /// </summary>
    public class ProductboardErrors
    {
        /// 
        [JsonProperty("source")]
        public object Source { get; set; } // sometimes it is an array, othertimes it is an object
    }
}
