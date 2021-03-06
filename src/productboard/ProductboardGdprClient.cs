﻿using productboard.Models;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace productboard
{
    /// <summary>
    /// A client, used to make requests to productboard's GDPR API and deserialize responses.
    /// </summary>
    public class ProductboardGdprClient : ProductboardClientBase<ProductboardGdprClientOptions>
    {
        /// <summary>
        /// Creates an instance if <see cref="ProductboardGdprClient"/>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="options">The options for configuring the client</param>
        public ProductboardGdprClient(ProductboardGdprClientOptions options, HttpClient httpClient = null)
            : base(options, httpClient) { }

        /// <summary>
        /// Delete data associated with a particular customer.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken">the token to cancel the request</param>
        /// <returns></returns>
        public async Task<ProductboardResponse<GdprDeletionResult>> DeleteAllClientDataAsync(string email, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var emailEncoded = Uri.EscapeDataString(email);
            var url = new Uri(Options.BaseUrl, $"/v1/customers/delete_all_data?email={emailEncoded}");
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            return await SendAsync<GdprDeletionResult>(request, cancellationToken);
        }

        /// <inheritdoc/>
        protected override void Authenticate(HttpRequestMessage request)
        {
            request.Headers.TryAddWithoutValidation("Private-Token", Options.Token);
        }
    }
}
