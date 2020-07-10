using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace productboard
{
    /// <summary>
    /// A client, used to make requests to productboard's API and deserialize responses.
    /// </summary>
    public class ProductboardClient
    {
        private readonly ProductboardClientOptions options;
        private readonly HttpClient httpClient;
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings();

        /// <summary>
        /// Creates an instance if <see cref="ProductboardClient"/>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="options">The options for configuring the client</param>
        public ProductboardClient(ProductboardClientOptions options, HttpClient httpClient = null)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.httpClient = httpClient ?? new HttpClient();

            if (options.BaseUrl == null)
            {
                throw new ArgumentNullException(nameof(options.BaseUrl));
            }

            // populate the User-Agent header
            var productVersion = typeof(ProductboardClient).Assembly.GetName().Version.ToString();
            var userAgent = new ProductInfoHeaderValue("productboard-dotnet", productVersion);
            this.httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);
        }

        /// <summary>
        /// Creates a Note
        /// </summary>
        /// <param name="note">The note's payload</param>
        /// <returns></returns>
        public async Task<ProductboardResponse<NoteCreationResult>> CreateNoteAsync(Note note)
        {
            var json = JsonConvert.SerializeObject(note, serializerSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = new Uri(options.BaseUrl, "/notes");
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            return await SendAsync<NoteCreationResult>(request);
        }

        private async Task<ProductboardResponse<T>> SendAsync<T>(HttpRequestMessage request)
            where T : class, new()
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Token);

            using (var response = await httpClient.SendAsync(request))
            {
                var str = await response.Content.ReadAsStringAsync();
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        using (var jsonReader = new JsonTextReader(streamReader))
                        {
                            var result = new ProductboardResponse<T>
                            {
                                StatusCode = response.StatusCode,
                                IsSuccessful = response.IsSuccessStatusCode
                            };

                            var serializer = JsonSerializer.Create(serializerSettings);
                            if (!response.IsSuccessStatusCode)
                            {
                                result.Error = serializer.Deserialize<ProductboardErrorResponse>(jsonReader);
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
