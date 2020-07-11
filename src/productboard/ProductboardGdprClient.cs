using Newtonsoft.Json;
using productboard.Errors;
using productboard.Models;
using System;
using System.IO;
using System.Net.Http;
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
        /// <returns></returns>
        public async Task<ProductboardGdprResponse<GdprDeletionResult>> DeleteAllClientDataAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var emailEncoded = Uri.EscapeDataString(email);
            var url = new Uri(Options.BaseUrl, $"/v1/customers/delete_all_data?email={emailEncoded}");
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            return await SendAsync<GdprDeletionResult>(request);
        }


        private async Task<ProductboardGdprResponse<T>> SendAsync<T>(HttpRequestMessage request)
            where T : class, new()
        {
            request.Headers.TryAddWithoutValidation("Private-Token", Options.Token);

            using (var response = await HttpClient.SendAsync(request))
            {
                var str = await response.Content.ReadAsStringAsync(); // remove
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        using (var jsonReader = new JsonTextReader(streamReader))
                        {
                            var result = new ProductboardGdprResponse<T>
                            {
                                StatusCode = response.StatusCode,
                                IsSuccessful = response.IsSuccessStatusCode
                            };

                            var serializer = JsonSerializer.Create(Options.SerializerSettings);
                            if (!response.IsSuccessStatusCode)
                            {
                                result.Error = serializer.Deserialize<ProductboardGdprErrorResponse>(jsonReader);
                            }
                            else
                            {
                                result.Resource = serializer.Deserialize<T>(jsonReader);
                            }

                            return result;
                        }
                    }
                }
            }
        }

    }
}
