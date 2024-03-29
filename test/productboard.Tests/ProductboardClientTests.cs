using Microsoft.Extensions.DependencyInjection;
using productboard.Models;
using System.Net;
using System.Text;
using Xunit;

namespace productboard.Tests;

public class ProductboardClientTests
{
    private const string TestEmail = "king@kong.com";
    private const string TestEmailEncoded = "king%40kong.com";

    private const string JsonRequest = "{\"title\":\"Note title\",\"content\":\"Here is some <b>exciting</b> content\",\"customer_email\":\"customer@example.com\",\"display_url\":\"https://www.example.com/deskdesk/notes/123\",\"source\":{\"origin\":\"deskdesk\",\"record_id\":\"123\"},\"tags\":[\"3.0\",\"important\",\"experimental\"]}";
    private const string Json201Response = "{\"links\": {\"html\": \"https://space.productboard.com/inbox/notes/123456\"},\"data\": {\"id\": \"d290f1ee-6c54-4b01-90e6-d701748f0851\"}}";
    private const string Json202Response = "{\"message\":\"The customer data will be deleted\"}";
    private const string Json410Response = "{\"error\":\"The customer does not exist\"}";
    private const string Json422Response = "{\"ok\": false,\"errors\":{\"source\":[\"already exists\"]}}";

    [Fact]
    public async Task CreateNote_Works()
    {
        const string token = "header.body.footer";

        var handler = new DynamicHttpMessageHandler(async (req, ct) =>
        {
            Assert.Equal(HttpMethod.Post, req.Method);

            Assert.NotNull(req.Headers.Authorization);
            Assert.Equal($"Bearer {token}", req.Headers.Authorization!.ToString());

            Assert.NotNull(req.Headers.UserAgent);
            var ua = Assert.Single(req.Headers.UserAgent);
            Assert.StartsWith("productboard-dotnet/", ua.ToString());

            Assert.Equal("/notes", req.RequestUri!.AbsolutePath);

            var body = await req.Content!.ReadAsStringAsync(ct);
            Assert.Equal(JsonRequest, body);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(Json201Response, Encoding.UTF8, "application/json")
            };
        });

        var note = new CreateNoteOptions
        {
            Title = "Note title",
            Content = "Here is some <b>exciting</b> content",
            CustomerEmail = "customer@example.com",
            DisplayUrl = "https://www.example.com/deskdesk/notes/123",
            Source = new Source
            {
                Origin = "deskdesk",
                RecordId = "123",
            },
            Tags = new List<string>
            {
                "3.0",
                "important",
                "experimental",
            },
        };

        var client = GetClient(token, handler);
        var response = await client.CreateNoteAsync(note);
        Assert.NotNull(response);
    }

    [Fact]
    public async Task DeleteAllClientDataAsync_Works()
    {
        const string token = "1234567890";

        var handler = new DynamicHttpMessageHandler((req, ct) =>
        {
            Assert.Equal(HttpMethod.Delete, req.Method);

            Assert.Null(req.Headers.Authorization);
            Assert.True(req.Headers.TryGetValues("Private-Token", out var headerValues));
            var x = Assert.Single(headerValues);
            Assert.Equal(token, x);

            Assert.NotNull(req.Headers.UserAgent);
            var ua = Assert.Single(req.Headers.UserAgent);
            Assert.StartsWith("productboard-dotnet/", ua.ToString());

            Assert.Equal("/v1/customers/delete_all_data", req.RequestUri!.AbsolutePath);
            Assert.Equal($"?email={TestEmailEncoded}", req.RequestUri.Query);

            Assert.Null(req.Content);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted,
                Content = new StringContent(Json202Response, Encoding.UTF8, "application/json")
            };
        });
        var client = GetClient(token, handler);
        var response = await client.DeleteAllClientDataAsync(TestEmail, token);
        Assert.NotNull(response);
    }

    [Fact]
    public async Task ResponseIsDeserializedCorrectly_201()
    {
        const string token = "header.body.footer";

        var handler = new DynamicHttpMessageHandler(async (req, ct) =>
        {
            Assert.Equal(HttpMethod.Post, req.Method);

            Assert.NotNull(req.Headers.Authorization);
            Assert.Equal($"Bearer {token}", req.Headers.Authorization!.ToString());

            Assert.NotNull(req.Headers.UserAgent);
            var ua = Assert.Single(req.Headers.UserAgent);
            Assert.StartsWith("productboard-dotnet/", ua.ToString());

            Assert.Equal("/notes", req.RequestUri!.AbsolutePath);

            var body = await req.Content!.ReadAsStringAsync(ct);
            Assert.Equal(JsonRequest, body);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(Json201Response, Encoding.UTF8, "application/json")
            };
        });

        var note = new CreateNoteOptions
        {
            Title = "Note title",
            Content = "Here is some <b>exciting</b> content",
            CustomerEmail = "customer@example.com",
            DisplayUrl = "https://www.example.com/deskdesk/notes/123",
            Source = new Source
            {
                Origin = "deskdesk",
                RecordId = "123",
            },
            Tags = new List<string>
            {
                "3.0",
                "important",
                "experimental",
            },
        };

        var client = GetClient(token, handler);
        var response = await client.CreateNoteAsync(note);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.Null(response.Error);
        Assert.NotNull(response.Resource);
        Assert.NotNull(response.Resource!.Links);
        Assert.Equal("https://space.productboard.com/inbox/notes/123456", response.Resource.Links!.Html);
        Assert.NotNull(response.Resource.Data);
        Assert.Equal("d290f1ee-6c54-4b01-90e6-d701748f0851", response.Resource.Data!.Id);
    }

    [Fact]
    public async Task ResponseIsDeserializedCorrectly_410()
    {
        const string token = "1234567890";

        var handler = new DynamicHttpMessageHandler((req, ct) =>
        {
            Assert.Equal(HttpMethod.Delete, req.Method);

            Assert.Null(req.Headers.Authorization);
            Assert.True(req.Headers.TryGetValues("Private-Token", out var headerValues));
            var x = Assert.Single(headerValues);
            Assert.Equal(token, x);

            Assert.NotNull(req.Headers.UserAgent);
            var ua = Assert.Single(req.Headers.UserAgent);
            Assert.StartsWith("productboard-dotnet/", ua.ToString());

            Assert.Equal("/v1/customers/delete_all_data", req.RequestUri!.AbsolutePath);
            Assert.Equal($"?email={TestEmailEncoded}", req.RequestUri.Query);

            Assert.Null(req.Content);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Gone,
                Content = new StringContent(Json410Response, Encoding.UTF8, "application/json")
            };
        });
        var client = GetClient(token, handler);
        var response = await client.DeleteAllClientDataAsync(TestEmail, token);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Gone, response.StatusCode);
        Assert.False(response.IsSuccessful);
        Assert.Null(response.Resource);
        Assert.NotNull(response.Error);
        Assert.Equal("The customer does not exist", response.Error!.Error);
    }

    [Fact]
    public async Task ResponseIsDeserializedCorrectly_422()
    {
        const string token = "header.body.footer";

        var handler = new DynamicHttpMessageHandler(async (req, ct) =>
        {
            Assert.Equal(HttpMethod.Post, req.Method);

            Assert.NotNull(req.Headers.Authorization);
            Assert.Equal($"Bearer {token}", req.Headers.Authorization!.ToString());

            Assert.NotNull(req.Headers.UserAgent);
            var ua = Assert.Single(req.Headers.UserAgent);
            Assert.StartsWith("productboard-dotnet/", ua.ToString());

            Assert.Equal("/notes", req.RequestUri!.AbsolutePath);

            var body = await req.Content!.ReadAsStringAsync(ct);
            Assert.Equal(JsonRequest, body);
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Content = new StringContent(Json422Response, Encoding.UTF8, "application/json")
            };
        });

        var note = new CreateNoteOptions
        {
            Title = "Note title",
            Content = "Here is some <b>exciting</b> content",
            CustomerEmail = "customer@example.com",
            DisplayUrl = "https://www.example.com/deskdesk/notes/123",
            Source = new Source
            {
                Origin = "deskdesk",
                RecordId = "123",
            },
            Tags = new List<string>
            {
                "3.0",
                "important",
                "experimental",
            },
        };

        var client = GetClient(token, handler);
        var response = await client.CreateNoteAsync(note);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        Assert.False(response.IsSuccessful);
        Assert.Null(response.Resource);
        Assert.NotNull(response.Error);
        Assert.False(response.Error!.Ok);
        Assert.NotNull(response.Error.Errors);
        Assert.NotNull(response.Error.Errors!.Source);
    }

    private static ProductboardClient GetClient(string token, HttpMessageHandler handler)
    {
        var services = new ServiceCollection();
        services.AddProductboard(options => options.Token = token)
                .ConfigurePrimaryHttpMessageHandler(() => handler);

        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ProductboardClient>();
    }
}
