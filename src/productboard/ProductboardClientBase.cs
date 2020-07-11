using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace productboard
{
    /// <summary>
    /// Abstraction for client used to make HTTP calls to productboard's APIs
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    public abstract class ProductboardClientBase<TOptions> where TOptions : ProductboardClientOptionsBase
    {
        /// <summary>
        /// Creates an instance if <see cref="ProductboardClientBase{TOptions}"/>
        /// </summary>
        /// <param name="httpClient">The client for making HTTP requests</param>
        /// <param name="options">The options for configuring the client</param>
        protected ProductboardClientBase(TOptions options, HttpClient httpClient = null)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            HttpClient = httpClient ?? new HttpClient();

            if (options.BaseUrl == null)
            {
                throw new ArgumentNullException(nameof(options.BaseUrl));
            }

            // populate the User-Agent header
            var productVersion = typeof(ProductboardClient).Assembly.GetName().Version.ToString();
            var userAgent = new ProductInfoHeaderValue("productboard-dotnet", productVersion);
            this.HttpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);
        }

        /// <summary>
        /// The client for making HTTP requests
        /// </summary>
        protected HttpClient HttpClient { get; }

        /// <summary>
        /// The options for configuring the client
        /// </summary>
        protected TOptions Options { get; }
    }
}
