using Newtonsoft.Json;
using System.Collections.Generic;

namespace productboard
{
    /// <summary>
    /// The detailed errors used with <see cref="ProductboardErrorResponse"/>
    /// </summary>
    public class ProductboardErrors
    {
        /// <summary>
        /// Example: already exists
        /// </summary>
        [JsonProperty("source")]
        public object Source { get; set; } // sometimes it is an array, othertimes it is an object
    }
}
