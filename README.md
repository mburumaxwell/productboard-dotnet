# productboard

## Introduction

productboard makes it easier to manage customer feedback, and effectively build features that customers actually want.
More about productboard on the [website](https://productboard.com).

The productboard dotnet NuGet package makes it easier to use the productboard Public API from your dotnet (netstandard2.0+)
projects without having to build your own API calls.

You can get your free API token from the portal:

1. Log in to the productboard app in a web browser
2. Go to **Workspace Settings** > **Integrations** > **Public API** > **Access Token**
3. Click **+** to generate a new token

The documentation that this Client is built on is available at https://developer.productboard.com

### Usage

```csharp
var options = new ProductboardClientOptions {
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

See [examples](./examples/) for more.

### Issues &amp; Comments

Please leave all comments, bugs, requests, and issues on the Issues page. We'll respond to your request ASAP!

### License

The Library is licensed under the [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form") license. Refere to the [LICENSE](./LICENSE.md) file for more information.
