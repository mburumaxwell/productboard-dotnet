using productboard.Errors;
using System.Net;

namespace productboard;

/// <summary>
/// The response from productboard Public APIs
/// </summary>
/// <typeparam name="TResource">The type of resource</typeparam>
public class ProductboardResponse<TResource>
{
    /// <summary>
    /// Create an instance of <see cref="ProductboardResponse{TResource}"/>
    /// </summary>
    /// <param name="response">The original HTTP response</param>
    /// <param name="resource">The extracted resource</param>
    /// <param name="error">The extracted error</param>
    public ProductboardResponse(HttpResponseMessage response,
                                TResource? resource = default,
                                ProductboardErrorResponse? error = default)
    {
        Response = response;
        Resource = resource;
        Error = error;
    }

    /// <summary>
    /// The original HTTP response
    /// </summary>
    public HttpResponseMessage Response { get; }

    /// <summary>
    /// The response status code gotten from <see cref="Response"/>
    /// </summary>
    public HttpStatusCode StatusCode => Response.StatusCode;

    /// <summary>
    /// Determines if the request was successful. Value is true if <see cref="StatusCode"/> is in the 200 to 299 range
    /// </summary>
    public virtual bool IsSuccessful => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);

    /// <summary>
    /// The resource extracted from the response body
    /// </summary>
    public TResource? Resource { get; set; }

    /// <summary>
    /// The error extracted from the response body
    /// </summary>
    public ProductboardErrorResponse? Error { get; set; }
}
