﻿using productboard;
using System;
using System.Threading.Tasks;

Console.WriteLine("Hello World!");
var options = new ProductboardGdprClientOptions
{
    Token = "your-token-here"
};
var client = new ProductboardGdprClient(options);

var response = await client.DeleteAllClientDataAsync("customer@example.com");
var result = response.Resource!;
Console.WriteLine(result.Message);
