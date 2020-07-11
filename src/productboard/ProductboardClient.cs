using Newtonsoft.Json;
using productboard.Models;
using System;
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

        /// <inheritdoc/>
        protected override void Authenticate(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Options.Token);
        }
    }
}
