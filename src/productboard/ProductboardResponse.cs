using System.Net;

namespace productboard
{
    /// <summary>
    /// productboard API Response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProductboardResponse<T> where T : class, new()
    {
        /// <summary>
        /// The resource extracted from the response body
        /// </summary>
        public T Resource { get; set; }

        /// <summary>
        /// The error extracted from the response body
        /// </summary>
        public ProductboardErrorResponse Error { get; set; }

        /// <summary>
        /// Status code response from the API
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Indicates whether a request has succeeded
        /// </summary>
        public bool IsSuccessful { get; set; }

    }
}
