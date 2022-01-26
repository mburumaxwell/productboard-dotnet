using productboard;

Console.WriteLine("Hello World!");
var options = new ProductboardClientOptions
{
    Token = "your-token-here"
};
var client = new ProductboardClient(options);

var response = await client.DeleteAllClientDataAsync("customer@example.com");
var result = response.Resource!;
Console.WriteLine(result.Message);
