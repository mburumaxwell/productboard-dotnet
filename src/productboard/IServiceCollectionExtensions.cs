using Microsoft.Extensions.Options;
using productboard;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="IHttpClientFactory"/> with <see cref="ProductboardClient"/> and
    /// related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> in which to register the services.</param>
    /// <param name="configureOptions">A delegate that is used to configure a <see cref="ProductboardClientOptions"/>.</param>
    /// <returns>An <see cref="IHttpClientBuilder" /> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddProductboard(this IServiceCollection services,
                                                     Action<ProductboardClientOptions>? configureOptions = null)
    {
        // if we have a configuration action, add it
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        services.AddSingleton<IValidateOptions<ProductboardClientOptions>, ProductboardClientValidateOptions>();

        return services.AddHttpClient<ProductboardClient>();
    }

    /// <summary>
    /// Adds the <see cref="IHttpClientFactory"/> with <see cref="ProductboardGdprClient"/> and
    /// related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> in which to register the services.</param>
    /// <param name="configureOptions">A delegate that is used to configure a <see cref="ProductboardGdprClientOptions"/>.</param>
    /// <returns>An <see cref="IHttpClientBuilder" /> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddProductboardGdpr(this IServiceCollection services,
                                                         Action<ProductboardGdprClientOptions>? configureOptions = null)
    {
        // if we have a configuration action, add it
        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        services.AddSingleton<IValidateOptions<ProductboardGdprClientOptions>, ProductboardGdprClientValidateOptions>();

        return services.AddHttpClient<ProductboardGdprClient>();
    }

    /// <summary>
    /// Adds the <see cref="IHttpClientFactory"/> with <see cref="ProductboardClient"/> and
    /// related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> in which to register the services.</param>
    /// <param name="token">
    /// The token used to access the productboard API.
    /// This value maps to <see cref="ProductboardClientOptions.Token"/>
    /// </param>
    /// <returns>An <see cref="IHttpClientBuilder" /> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddProductboard(this IServiceCollection services, string token)
    {
        return services.AddProductboard(o => o.Token = token);
    }

    /// <summary>
    /// Adds the <see cref="IHttpClientFactory"/> with <see cref="ProductboardGdprClient"/> and
    /// related services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> in which to register the services.</param>
    /// <param name="token">
    /// The token used to access the productboard API.
    /// This value maps to <see cref="ProductboardGdprClientOptions.Token"/>
    /// </param>
    /// <returns>An <see cref="IHttpClientBuilder" /> that can be used to configure the client.</returns>
    public static IHttpClientBuilder AddProductboardGdpr(this IServiceCollection services, string token)
    {
        return services.AddProductboardGdpr(o =>
        {
            o.Token = token;
        });
    }
}
