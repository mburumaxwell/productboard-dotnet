﻿namespace productboard;

/// <summary>
/// Abstract options for configuring an implementation of <see cref="ProductboardClientBase{TOptions}"/>
/// </summary>
public abstract class ProductboardClientOptionsBase
{
    /// <summary>
    /// The base URL for making requests to productboard
    /// </summary>
    public Uri BaseUrl { get; set; } = new Uri("https://api.productboard.com/");
}
