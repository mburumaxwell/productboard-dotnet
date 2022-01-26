using Microsoft.Extensions.DependencyInjection;
using productboard;
using productboard.Models;

Console.WriteLine("Hello World!");

var token = "your-token-here";

var services = new ServiceCollection();
services.AddProductboard(token);

var provider = services.BuildServiceProvider();

var client = provider.GetRequiredService<ProductboardClient>();
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
var response = await client.CreateNoteAsync(note);
var created = response.Resource!;
Console.Write("Note created!");
Console.Write($" Id = {created.Data!.Id}");
Console.WriteLine($"Url: {created.Links!.Html}");
