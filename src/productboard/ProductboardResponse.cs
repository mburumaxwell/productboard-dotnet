using productboard.Errors;
using System.Net;

namespace productboard
{
    /// <summary>
    /// Abstractions for productboard API Response
    /// </summary>
    /// <typeparam name="TResource">The type of resource</typeparam>
    /// <typeparam name="TError">The type of error</typeparam>
    public class ProductboardResponseBase<TResource, TError>
        where TResource : class, new()
        where TError : class, new()
    {
        /// <summary>
        /// The resource extracted from the response body
        /// </summary>
        public TResource Resource { get; set; }

        /// <summary>
        /// The error extracted from the response body
        /// </summary>
        public TError Error { get; set; }

        /// <summary>
        /// Status code response from the API
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Indicates whether a request has succeeded
        /// </summary>
        public bool IsSuccessful { get; set; }
    }

    /// <summary>
    /// The response from productboard Public APIs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProductboardResponse<T> : ProductboardResponseBase<T, ProductboardErrorResponse> where T : class, new()
    {
    }

    /// <summary>
    /// The response from productboard Public APIs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProductboardGdprResponse<T> : ProductboardResponseBase<T, ProductboardGdprErrorResponse> where T : class, new()
    {
    }
}
