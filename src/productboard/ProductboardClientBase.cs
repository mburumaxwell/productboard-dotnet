using Microsoft.Extensions.Options;
using productboard.Errors;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace productboard;

/// <summary>
/// Abstraction for client used to make HTTP calls to productboard's APIs
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public abstract class ProductboardClientBase<TOptions> where TOptions : ProductboardClientOptionsBase
{
    // no need to create this options every time a client is created
    private static readonly JsonSerializerOptions serializerOptions = new(JsonSerializerDefaults.Web)
    {
        // Some content contains HTML which need special handling in JSON
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    private readonly HttpClient httpClient;
    private readonly TOptions options;

    /// <summary>
    /// Creates an instance if <see cref="ProductboardClientBase{TOptions}"/>
    /// </summary>
    /// <param name="options">The options for configuring the client</param>
    protected ProductboardClientBase(TOptions options)
        : this(null, Options.Create(options)) { }

    /// <summary>
    /// Creates an instance if <see cref="ProductboardClientBase{TOptions}"/>
    /// </summary>
    /// <param name="httpClient">The client for making HTTP requests</param>
    /// <param name="optionsAccessor">The options for configuring the client</param>
    protected ProductboardClientBase(HttpClient? httpClient, IOptions<TOptions> optionsAccessor)
    {
        options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        this.httpClient = httpClient ?? new HttpClient();

        // set the base address
        this.httpClient.BaseAddress = options.BaseUrl ?? throw new InvalidOperationException($"'{nameof(options.BaseUrl)}' must be provided in the options.");

        // populate the User-Agent header
        var productVersion = typeof(ProductboardClient).Assembly.GetName().Version!.ToString();
        var userAgent = new ProductInfoHeaderValue("productboard-dotnet", productVersion);
        this.httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);
    }

    /// <summary>
    /// Authenticate a request before it is sent
    /// </summary>
    /// <param name="request">The request to be authenticated</param>
    /// <param name="options">The client options</param>
    protected abstract void Authenticate(HttpRequestMessage request, TOptions options);

    /// <summary>
    /// Send a request and extract the response
    /// </summary>
    /// <typeparam name="TResource">the type or resource to be extracted</typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken">the token to cancel the request</param>
    /// <returns></returns>
    protected virtual async Task<ProductboardResponse<TResource>> SendAsync<TResource>(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(request, cancellationToken);
        (var resource, var error) = await ExtractResponseAsync<TResource, ProductboardErrorResponse>(response, cancellationToken);
        return new ProductboardResponse<TResource>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = response.StatusCode,
            Resource = resource,
            Error = error,
        };
    }

    /// <summary>
    /// Send a request and extract the response
    /// </summary>
    /// <typeparam name="TResource">the type or resource to be extracted</typeparam>
    /// <typeparam name="TError">the type of error to be extracted</typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken">the token to cancel the request</param>
    /// <returns></returns>
    protected virtual async Task<ProductboardResponse<TResource, TError>> SendAsync<TResource, TError>(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(request, cancellationToken);
        (var resource, var error) = await ExtractResponseAsync<TResource, TError>(response, cancellationToken);
        return new ProductboardResponse<TResource, TError>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = response.StatusCode,
            Resource = resource,
            Error = error,
        };
    }

    /// <summary>
    /// Extracts the resource and error from the response message. The resource is only extracted for successful requests according
    /// to <see cref="HttpResponseMessage.IsSuccessStatusCode"/>, otherwise the error is extracted.
    /// </summary>
    /// <typeparam name="TResource">the type or resource to be extracted</typeparam>
    /// <typeparam name="TError">the type of error to be extracted</typeparam>
    /// <param name="response">the response message to be used for extraction</param>
    /// <param name="cancellationToken">the token to cancel the request</param>
    /// <returns></returns>
    protected virtual async Task<(TResource?, TError?)> ExtractResponseAsync<TResource, TError>(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        // extract the response
#if NET5_0_OR_GREATER
        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
#else
        using var stream = await response.Content.ReadAsStreamAsync();
#endif

        var resource = default(TResource);
        var error = default(TError);

        // get the content type
        var contentType = response.Content.Headers?.ContentType;

        // get the encoding and always default to UTF-8
        var encoding = Encoding.GetEncoding(contentType?.CharSet ?? Encoding.UTF8.BodyName);

        if (response.IsSuccessStatusCode)
        {
            resource = await JsonSerializer.DeserializeAsync<TResource>(stream, serializerOptions, cancellationToken);
        }
        else
        {
            error = await JsonSerializer.DeserializeAsync<TError>(stream, serializerOptions, cancellationToken);
        }

        return (resource, error);
    }

    /// <summary>
    /// Sends a request and gets a response.
    /// Before the request is made, the authorization provider is invoked to set authorization details for the request.
    /// All operations are asynchronous.
    /// </summary>
    /// <param name="request">the request to be sent</param>
    /// <param name="cancellationToken">the token to cancel the request</param>
    /// <returns></returns>
    protected virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        // ensure request is not null
        if (request == null) throw new ArgumentNullException(nameof(request));

        // setup authentication
        Authenticate(request, options);

        // execute the request
        return await httpClient.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// Make an instance of <see cref="StreamContent"/> with JSON content from the provided object
    /// </summary>
    /// <param name="o">the object to to write</param>
    /// <param name="encoding">the encoding to use</param>
    /// <returns></returns>
    protected HttpContent MakeJsonHttpContent(object o, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var json = JsonSerializer.Serialize(o, serializerOptions);
        return new StringContent(json, encoding, "application/json");
    }
}
