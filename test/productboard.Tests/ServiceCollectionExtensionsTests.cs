using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace productboard.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void TestAddProductboardWithoutApiKey()
    {
        // Arrange
        var services = new ServiceCollection().AddProductboard(options => { }).Services.BuildServiceProvider();

        // Act && Assert
        Assert.Throws<OptionsValidationException>(() => services.GetRequiredService<ProductboardClient>());
    }

    [Fact]
    public void TestAddProductboardReturnHttpClientBuilder()
    {
        // Arrange
        var collection = new ServiceCollection();

        // Act
        var builder = collection.AddProductboard(options => options.Token = "FAKE_TOKEN");

        // Assert
        Assert.NotNull(builder);
        Assert.IsAssignableFrom<IHttpClientBuilder>(builder);
    }

    [Fact]
    public void TestAddProductboardRegisteredWithTransientLifeTime()
    {
        // Arrange
        var collection = new ServiceCollection();

        // Act
        var builder = collection.AddProductboard(options => options.Token = "FAKE_TOKEN");

        // Assert
        var serviceDescriptor = collection.FirstOrDefault(x => x.ServiceType == typeof(ProductboardClient));
        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Transient, serviceDescriptor!.Lifetime);
    }

    [Fact]
    public void TestAddProductboardCanResolveProductboardClientOptions()
    {
        // Arrange
        var services = new ServiceCollection().AddProductboard(options => options.Token = "FAKE_TOKEN").Services.BuildServiceProvider();

        // Act
        var productboardClientOptions = services.GetService<IOptions<ProductboardClientOptions>>();

        // Assert
        Assert.NotNull(productboardClientOptions);
    }

    [Fact]
    public void TestAddProductboardCanResolveProductboardClient()
    {
        // Arrange
        var services = new ServiceCollection().AddProductboard(options => options.Token = "FAKE_TOKEN").Services.BuildServiceProvider();

        // Act
        var productboard = services.GetService<ProductboardClient>();

        // Assert
        Assert.NotNull(productboard);
    }
}
