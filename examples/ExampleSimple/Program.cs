using productboard;
using productboard.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleSimple
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

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
            var created = response.Resource!;
            Console.Write("Note created!");
            Console.Write($" Id = {created.Data!.Id}");
            Console.WriteLine($"Url: {created.Links!.Html}");
        }
    }
}
