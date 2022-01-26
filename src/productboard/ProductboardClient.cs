using Microsoft.Extensions.Options;
using productboard.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace productboard;

/// <summary>
/// A client, used to make requests to productboard's Public APIs and deserialize responses.
/// </summary>
public class ProductboardClient : ProductboardClientBase<ProductboardClientOptions>
{
    /// <summary>
    /// Creates an instance if <see cref="ProductboardClient"/>
    /// </summary>
    /// <param name="options">The options for configuring the client</param>
    public ProductboardClient(ProductboardClientOptions options)
        : base(options) { }

    /// <summary>
    /// Creates an instance if <see cref="ProductboardClient"/>
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="optionsAccessor">The options for configuring the client</param>
    public ProductboardClient(HttpClient? httpClient, IOptions<ProductboardClientOptions> optionsAccessor)
        : base(httpClient, optionsAccessor) { }

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

        var json = JsonSerializer.Serialize(note, SerializerOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, "/notes")
        {
            Content = content,
        };

        return await SendAsync<NoteCreationResult>(request, cancellationToken);
    }

    /// <inheritdoc/>
    protected override void Authenticate(HttpRequestMessage request, ProductboardClientOptions options)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);
        request.Headers.TryAddWithoutValidation("X-Version", "1");
    }
}
