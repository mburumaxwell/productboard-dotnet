using System.Net;
using System.Text;
using Xunit;

namespace productboard.Tests;

public class ProductboardGdprClientTests
{
    private const string TestEmail = "king@kong.com";
    private const string TestEmailEncoded = "king%40kong.com";
    private const string Json202Response = "{\"message\":\"The customer data will be deleted\"}";
    private const string Json410Response = "{\"error\":\"The customer does not exist\"}";

    [Fact]
    public async Task RequestIsSerializedCorrectly()
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
        var httpClient = new HttpClient(handler);
        var options = new ProductboardGdprClientOptions { Token = token };
        var client = new ProductboardGdprClient(httpClient, options);
        var response = await client.DeleteAllClientDataAsync(TestEmail);
        Assert.NotNull(response);
    }

    [Fact]
    public async Task ResponseIsDeserializedCorrectly_202()
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
        var httpClient = new HttpClient(handler);
        var options = new ProductboardGdprClientOptions { Token = token };
        var client = new ProductboardGdprClient(httpClient, options);
        var response = await client.DeleteAllClientDataAsync(TestEmail);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.Null(response.Error);
        Assert.NotNull(response.Resource);
        Assert.Equal("The customer data will be deleted", response.Resource!.Message);
    }

    [Fact]
    public async Task ResponseIsDeserializedCorrectly_422()
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
        var httpClient = new HttpClient(handler);
        var options = new ProductboardGdprClientOptions { Token = token };
        var client = new ProductboardGdprClient(httpClient, options);
        var response = await client.DeleteAllClientDataAsync(TestEmail);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Gone, response.StatusCode);
        Assert.False(response.IsSuccessful);
        Assert.Null(response.Resource);
        Assert.NotNull(response.Error);
        Assert.Equal("The customer does not exist", response.Error!.Error);
    }
}
