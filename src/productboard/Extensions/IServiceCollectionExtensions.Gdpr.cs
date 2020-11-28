using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using productboard;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> with <see cref="ProductboardGdprClient"/> and
        /// related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> in which to register the services.</param>
        /// <param name="configuration">A configuration object with values for a <see cref="ProductboardGdprClientOptions"/>.</param>
        /// <param name="configureOptions">A delegate that is used to configure a <see cref="ProductboardGdprClientOptions"/>.</param>
        /// <returns>An <see cref="IHttpClientBuilder" /> that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddProductboardGdpr(this IServiceCollection services,
                                                               IConfiguration configuration = null,
                                                               Action<ProductboardGdprClientOptions> configureOptions = null)
        {
            // if we have a configuration, add it
            if (configuration != null)
            {
                services.Configure<ProductboardGdprClientOptions>(configuration);
            }

            // if we have a configuration action, add it
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            services
                 .PostConfigure<ProductboardGdprClientOptions>(o =>
                 {
                     if (string.IsNullOrWhiteSpace(o.Token))
                     {
                         throw new ArgumentNullException(nameof(o.Token));
                     }

                     if (o.BaseUrl == null)
                     {
                         throw new ArgumentNullException(nameof(o.BaseUrl));
                     }

                 });

            services.TryAddTransient<ProductboardGdprClient>(resolver => resolver.GetRequiredService<InjectableProductboardGdprClient>());

            return services.AddHttpClient<InjectableProductboardGdprClient>();
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> with <see cref="ProductboardGdprClient"/> and
        /// related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> in which to register the services.</param>
        /// <param name="configureOptions">A delegate that is used to configure a <see cref="ProductboardGdprClientOptions"/>.</param>
        /// <returns>An <see cref="IHttpClientBuilder" /> that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddProductboardGdpr(this IServiceCollection services,
                                                               Action<ProductboardGdprClientOptions> configureOptions)
        {
            return services.AddProductboardGdpr(null, configureOptions);
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> with <see cref="ProductboardGdprClient"/> and
        /// related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> in which to register the services.</param>
        /// <param name="configuration">A configuration object with values for a <see cref="ProductboardGdprClientOptions"/>.</param>
        /// <returns>An <see cref="IHttpClientBuilder" /> that can be used to configure the client.</returns>
        public static IHttpClientBuilder AddProductboardGdpr(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddProductboardGdpr(configuration, null);
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
        public static IHttpClientBuilder AddProductboardGdpr(this IServiceCollection services,
                                                               string token)
        {
            return services.AddProductboardGdpr(o =>
            {
                o.Token = token;
            });
        }
    }
}
