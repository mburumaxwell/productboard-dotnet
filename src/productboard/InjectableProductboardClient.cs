using Microsoft.Extensions.Options;
using System.Net.Http;

namespace productboard;

/// <summary>
/// A wrapped <see cref="ProductboardClient"/> with single constructor to inject an <see cref="HttpClient"/>
/// whose lifetime is managed externally, e.g. by an DI container.
/// </summary>
internal class InjectableProductboardClient : ProductboardClient
{
    public InjectableProductboardClient(HttpClient httpClient, IOptions<ProductboardClientOptions> optionsAccessor)
        : base(optionsAccessor.Value, httpClient) { }
}
