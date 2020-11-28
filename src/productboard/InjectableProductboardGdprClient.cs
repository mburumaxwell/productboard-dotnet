using Microsoft.Extensions.Options;
using System.Net.Http;

namespace productboard
{
    /// <summary>
    /// A wrapped <see cref="ProductboardGdprClient"/> with single constructor to inject an <see cref="HttpClient"/>
    /// whose lifetime is managed externally, e.g. by an DI container.
    /// </summary>
    internal class InjectableProductboardGdprClient : ProductboardGdprClient
    {
        public InjectableProductboardGdprClient(HttpClient httpClient, IOptions<ProductboardGdprClientOptions> optionsAccessor)
            : base(optionsAccessor.Value, httpClient) { }
    }
}
