﻿using Microsoft.Extensions.Options;
using productboard.Core;
using productboard.Errors;
using productboard.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace productboard;

/// <summary>
/// A client, used to make requests to productboard's Public APIs and deserialize responses.
/// </summary>
public class ProductboardClient
{
    // no need to create this options every time a client is created
    private static readonly JsonSerializerOptions serializerOptions = new(JsonSerializerDefaults.Web)
    {
        // Some content contains HTML which need special handling in JSON
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    private readonly HttpClient httpClient;
    private readonly ProductboardClientOptions options;

    /// <summary>
    /// Creates an instance if <see cref="ProductboardClient"/>
    /// </summary>
    /// <param name="options">The options for configuring the client</param>
    public ProductboardClient(ProductboardClientOptions options)
        : this(null, Options.Create(options)) { }

    /// <summary>
    /// Creates an instance if <see cref="ProductboardClient"/>
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="optionsAccessor">The options for configuring the client</param>
    public ProductboardClient(HttpClient? httpClient, IOptions<ProductboardClientOptions> optionsAccessor)
    {
        options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        this.httpClient = httpClient ?? new HttpClient();

        // set the base address
        this.httpClient.BaseAddress = options.Endpoint ?? throw new InvalidOperationException($"'{nameof(options.Endpoint)}' must be provided in the options.");

        // populate the User-Agent header
        var productVersion = typeof(ProductboardClient).Assembly.GetName().Version!.ToString();
        var userAgent = new ProductInfoHeaderValue("productboard-dotnet", productVersion);
        this.httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);
    }

    #region Notes

    /// <summary>
    /// Creates a note.
    /// </summary>
    /// <param name="note">The note's payload</param>
    /// <param name="cancellationToken">the token to cancel the request</param>
    /// <returns></returns>
    public async Task<ProductboardResponse<NoteCreationResult>> CreateNoteAsync(Note note, CancellationToken cancellationToken = default)
    {
        // ensure note is not null
        if (note == null) throw new ArgumentNullException(nameof(note));

        var request = new HttpRequestMessage(HttpMethod.Post, "/notes")
        {
            Content = MakeJsonHttpContent(note),
        };

        return await SendAsync<NoteCreationResult>(request, cancellationToken: cancellationToken);
    }

    #endregion

    #region Feature Statuses

    public async Task<ProductboardResponse<ResourceWithPagination<FeatureStatus>>> GetFeatureStatusesAsync(BasicListOptions? pagination = null,
                                                                                                           CancellationToken cancellationToken = default)
    {
        var url = MakePathWithQuery("/feature-statuses", pagination);
        return await GetAsync<ResourceWithPagination<FeatureStatus>>(url, cancellationToken: cancellationToken);
    }

    #endregion

    #region Products

    public async Task<ProductboardResponse<ResourceWithPagination<Product>>> GetProductsAsync(BasicListOptions? pagination = null,
                                                                                              CancellationToken cancellationToken = default)
    {
        var url = MakePathWithQuery("/products", pagination);
        return await GetAsync<ResourceWithPagination<Product>>(url, cancellationToken: cancellationToken);
    }

    public async Task<ProductboardResponse<ResourceWithData<Product>>> GetProductAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

        var path = $"/products/{id}";
        return await GetAsync<ResourceWithData<Product>>(path, cancellationToken: cancellationToken);
    }

    #endregion

    #region GDPR

    /// <summary>Delete data associated with a particular customer for GDPR compliance.</summary>
    /// <param name="email">Email address of the customer</param>
    /// <param name="token">
    /// The token to use for the request.
    /// If not provided, the value from <see cref="ProductboardClientOptions.GdprToken"/> is used.
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ProductboardResponse<GdprDeletionResult>> DeleteAllClientDataAsync(string email,
                                                                                         string? token = null,
                                                                                         CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

        // ensure we have a token
        token ??= options.GdprToken ?? throw new ArgumentNullException(nameof(token));

        var emailEncoded = Uri.EscapeDataString(email);
        var path = $"/v1/customers/delete_all_data?email={emailEncoded}";
        var request = new HttpRequestMessage(HttpMethod.Delete, path);
        request.Headers.TryAddWithoutValidation("Private-Token", token);
        return await SendAsync<GdprDeletionResult>(request, authenticate: false, cancellationToken);
    }

    #endregion

    #region Helpers

    /// <summary>Combine path and query.</summary>
    /// <param name="path">The path to add before the query.</param>
    /// <param name="options">The options to generate keys and values for the query.</param>
    /// <returns>The path and query combined.</returns>
    protected virtual string MakePathWithQuery(string? path, BasicListOptions? options)
    {
        var args = new QueryValues();
        options?.Populate(args);

        var query = args.ToString();
        return MakePathWithQuery(path: path, query: query);
    }

    /// <summary>Combine path and query.</summary>
    /// <param name="path">The path to add before the query.</param>
    /// <param name="query">The query to append.</param>
    /// <returns>The path and query combined.</returns>
    protected virtual string MakePathWithQuery(string? path, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException($"'{nameof(query)}' cannot be null or whitespace.", nameof(query));
        }

        if (!query.StartsWith("?")) query = $"?{query}";
        return $"{path}{query}";
    }

    /// <summary>Send a GET request and extract the response.</summary>
    /// <typeparam name="TResource">The type or resource to be extracted.</typeparam>
    /// <param name="path">The path to send to.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<ProductboardResponse<TResource>> GetAsync<TResource>(string path, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await SendAsync<TResource>(request, authenticate: false, cancellationToken);
    }

    /// <summary>Send a request and extract the response.</summary>
    /// <typeparam name="TResource">The type or resource to be extracted.</typeparam>
    /// <param name="request">The request to be sent.</param>
    /// <param name="authenticate">Whether to include the default authentication information.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<ProductboardResponse<TResource>> SendAsync<TResource>(HttpRequestMessage request,
                                                                                       bool authenticate = true,
                                                                                       CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(request, authenticate, cancellationToken);
        (var resource, var error) = await ExtractResponseAsync<TResource, ProductboardErrorResponse>(response, cancellationToken);
        return new ProductboardResponse<TResource>(response: response, resource: resource, error: error);
    }

    /// <summary>
    /// Extracts the resource and error from the response message.
    /// The resource is only extracted for successful requests according
    /// to <see cref="HttpResponseMessage.IsSuccessStatusCode"/>,
    /// otherwise the error is extracted.
    /// </summary>
    /// <typeparam name="TResource">The type or resource to be extracted.</typeparam>
    /// <typeparam name="TError">The type of error to be extracted.</typeparam>
    /// <param name="response">The response message to extract from.</param>
    /// <param name="cancellationToken"></param>
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

    /// <summary>Sends a request and gets a response.</summary>
    /// <param name="request">The request to be sent</param>
    /// <param name="authenticate">Whether to include the default authentication information.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, bool authenticate = true, CancellationToken cancellationToken = default)
    {
        // ensure request is not null
        if (request == null) throw new ArgumentNullException(nameof(request));

        // setup authentication
        if (authenticate)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);
            request.Headers.TryAddWithoutValidation("X-Version", "1");
        }

        // set the partner header
        var partnerId = options.PartnerId;
        if (!string.IsNullOrEmpty(partnerId))
        {
            request.Headers.TryAddWithoutValidation("Productboard-Partner-Id", partnerId);
        }

        // execute the request
        return await httpClient.SendAsync(request, cancellationToken);
    }

    /// <summary>Make an <see cref="HttpContent"/> with <paramref name="o"/> in JSON.</summary>
    /// <param name="o">the object to to write</param>
    /// <param name="encoding">the encoding to use</param>
    /// <returns></returns>
    protected static HttpContent MakeJsonHttpContent(object o, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var json = JsonSerializer.Serialize(o, serializerOptions);
        return new StringContent(json, encoding, "application/json");
    }

    #endregion

}
