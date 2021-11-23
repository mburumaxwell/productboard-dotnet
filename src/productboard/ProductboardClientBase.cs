using productboard.Errors;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace productboard
{
    /// <summary>
    /// Abstraction for client used to make HTTP calls to productboard's APIs
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public abstract class ProductboardClientBase<TOptions> where TOptions : ProductboardClientOptionsBase
    {
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        /// <summary>
        /// Creates an instance if <see cref="ProductboardClientBase{TOptions}"/>
        /// </summary>
        /// <param name="httpClient">The client for making HTTP requests</param>
        /// <param name="options">The options for configuring the client</param>
        protected ProductboardClientBase(TOptions options, HttpClient httpClient = null)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            BackChannel = httpClient ?? new HttpClient();

            if (options.BaseUrl == null)
            {
                throw new ArgumentNullException(nameof(options.BaseUrl));
            }

            // populate the User-Agent header
            var productVersion = typeof(ProductboardClient).Assembly.GetName().Version.ToString();
            var userAgent = new ProductInfoHeaderValue("productboard-dotnet", productVersion);
            BackChannel.DefaultRequestHeaders.UserAgent.Add(userAgent);

            // prepare options for serialization
            serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
        }

        /// <summary>
        /// The client for making HTTP requests
        /// </summary>
        protected HttpClient BackChannel { get; }

        /// <summary>
        /// The options for configuring the client
        /// </summary>
        protected TOptions Options { get; }

        /// <summary>
        /// The settings used for serialization
        /// </summary>
        protected JsonSerializerOptions SerializerOptions => serializerOptions;

        /// <summary>
        /// Authenticate a request before it is sent
        /// </summary>
        /// <param name="request">The request to be authenticated</param>
        protected abstract void Authenticate(HttpRequestMessage request);

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
            (TResource resource, TError error) = await ExtractResponseAsync<TResource, TError>(response, cancellationToken);
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
        protected virtual async Task<(TResource, TError)> ExtractResponseAsync<TResource, TError>(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            var resource = default(TResource);
            var error = default(TError);

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                // get the content type
                var contentType = response.Content.Headers?.ContentType;

                // get the encoding and always default to UTF-8
                var encoding = Encoding.GetEncoding(contentType?.CharSet ?? Encoding.UTF8.BodyName);

                if (response.IsSuccessStatusCode)
                {
                    resource = await JsonSerializer.DeserializeAsync<TResource>(stream, SerializerOptions, cancellationToken);
                }
                else
                {
                    error = await JsonSerializer.DeserializeAsync<TError>(stream, SerializerOptions, cancellationToken);
                }
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
            Authenticate(request);

            // execute the request
            return await BackChannel.SendAsync(request, cancellationToken);
        }
    }
}
