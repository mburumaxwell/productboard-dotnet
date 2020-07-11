using Newtonsoft.Json;
using productboard.Errors;
using productboard.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace productboard
{
    /// <summary>
    /// A client, used to make requests to productboard's Public APIs and deserialize responses.
    /// </summary>
    public class ProductboardClient : ProductboardClientBase<ProductboardClientOptions>
    {
        /// <summary>
        /// Creates an instance if <see cref="ProductboardClient"/>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="options">The options for configuring the client</param>
        public ProductboardClient(ProductboardClientOptions options, HttpClient httpClient = null)
            : base(options, httpClient) { }

        /// <summary>
        /// Creates a note.
        /// </summary>
        /// <param name="note">The note's payload</param>
        /// <returns></returns>
        public async Task<ProductboardResponse<NoteCreationResult>> CreateNoteAsync(Note note)
        {
            var json = JsonConvert.SerializeObject(note, Options.SerializerSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = new Uri(Options.BaseUrl, "/notes");
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            return await SendAsync<NoteCreationResult>(request);
        }

        private async Task<ProductboardResponse<T>> SendAsync<T>(HttpRequestMessage request)
            where T : class, new()
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Options.Token);

            using (var response = await HttpClient.SendAsync(request))
            {
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

                            var serializer = JsonSerializer.Create(Options.SerializerSettings);
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
