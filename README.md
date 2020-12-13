# productboard

## Introduction

productboard makes it easier to manage customer feedback, and effectively build features that customers actually want.
More about productboard on the [website](https://productboard.com).

The productboard dotnet NuGet package makes it easier to use the productboard Public API from your dotnet (netstandard2.0+)
projects without having to build your own API calls.

You can get a Public API token from the portal:

1. Log in to the productboard app in a web browser
2. Go to **Workspace Settings** > **Integrations** > **Public API** > **Access Token**
3. Click **+** to generate a new token

You can get a GDPR token from the portal:

1. Log in to the productboard app in a web browser
2. Go to **Workspace Settings** > **Integrations** > **Public API** > **GDPR Public API**
3. Click **+** to generate a new token

### Important

The APIs used to create Notes and the one used to delete all customer data are different even though they share the same base address (https://api.productboard.com). They use different tokens. Using one for the other will result in 401 (Unauthorized). Ensure you use the correct token for the usage you have.

This library is build using the publicly available documentation. The one for the Public API is available at [https://developer.productboard.com](https://developer.productboard.com). Whereas the one for the GDPR Public API is available in the help docs at [https://help.productboard.com/en/articles/1947849-delete-customer-data-from-productboard](https://help.productboard.com/en/articles/1947849-delete-customer-data-from-productboard).

### Naming

The product and company name is productboard. For more information on how to spell see the specific FAQ [https://help.productboard.com/en/articles/2705293-how-to-spell-productboard](https://help.productboard.com/en/articles/2705293-how-to-spell-productboard). However, in .NET it is recommended that names of types, methods and properties start with a capital letter. This is why I chose to change the first letter for the types but retained everything else as is.

### Installation

To install using Package Manager Console use:
> Install-Package productboard

To install using dotnet cli use:
> dotnet add productboard

Alternatively, you can use the NuGet package manager in Visual Studio by searching for `productboard` and then click install.

### Usage

To create a note, use the `ProductboardClient` and `ProductboardClientOptions` types, and the `services.AddProductBoard(...)` extension method.

```csharp
var options = new ProductboardClientOptions
{
    Token = "your-token-here"
};
var client = new ProductboardClient(options);
var note = new Note
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
var response = await client.CreateNoteAsync(note);
var created = response.Resource;
Console.Write("Note created!");
Console.Write($" Id = {created.Data.Id}");
Console.WriteLine($"Url: {created.Links.Html}");
```

To delete all customer data, use the `ProductboardGdprClient` and `ProductboardGdprClientOptions` types, and the `services.AddProductBoardGdpr(...)` extension method.

```csharp
var options = new ProductboardGdprClientOptions
{
    Token = "your-token-here"
};
var client = new ProductboardGdprClient(options);

var response = await client.DeleteAllClientDataAsync("customer@example.com");
var result = response.Resource;
Console.WriteLine(result.Message);
```

See [examples](./examples/) for more.

### Issues &amp; Comments

Please leave all comments, bugs, requests, and issues on the Issues page. We'll respond to your request ASAP!

### License

The Library is licensed under the [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form") license. Refer to the [LICENSE](./LICENSE) file for more information.
