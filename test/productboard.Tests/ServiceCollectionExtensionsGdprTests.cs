using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Xunit;

namespace productboard.Tests;

public class ServiceCollectionExtensionsGdprTests
{
    [Fact]
    public void TestAddProductboardGdprWithoutApiKey()
    {
        // Arrange
        var services = new ServiceCollection().AddProductboardGdpr(options => { }).Services.BuildServiceProvider();

        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => services.GetRequiredService<ProductboardGdprClient>());
    }

    [Fact]
    public void TestAddProductboardGdprReturnHttpClientBuilder()
    {
        // Arrange
        var collection = new ServiceCollection();

        // Act
        var builder = collection.AddProductboardGdpr(options => options.Token = "FAKE_TOKEN");

        // Assert
        Assert.NotNull(builder);
        Assert.IsAssignableFrom<IHttpClientBuilder>(builder);
    }

    [Fact]
    public void TestAddProductboardGdprRegisteredWithTransientLifeTime()
    {
        // Arrange
        var collection = new ServiceCollection();

        // Act
        var builder = collection.AddProductboardGdpr(options => options.Token = "FAKE_TOKEN");

        // Assert
        var serviceDescriptor = collection.FirstOrDefault(x => x.ServiceType == typeof(ProductboardGdprClient));
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor!.Lifetime);
    }

    [Fact]
    public void TestAddProductboardGdprCanResolveProductboardGdprClientOptions()
    {
        // Arrange
        var services = new ServiceCollection().AddProductboardGdpr(options => options.Token = "FAKE_TOKEN").Services.BuildServiceProvider();

        // Act
        var ProductboardGdprClientOptions = services.GetService<IOptions<ProductboardGdprClientOptions>>();

        // Assert
        Assert.NotNull(ProductboardGdprClientOptions);
    }

    [Fact]
    public void TestAddProductboardGdprCanResolveProductboardGdprClient()
    {
        // Arrange
        var services = new ServiceCollection().AddProductboardGdpr(options => options.Token = "FAKE_TOKEN").Services.BuildServiceProvider();

        // Act
        var productboard = services.GetService<ProductboardGdprClient>();

        // Assert
        Assert.NotNull(productboard);
    }
}
