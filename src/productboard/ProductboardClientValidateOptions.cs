using Microsoft.Extensions.Options;
using productboard;

namespace Microsoft.Extensions.DependencyInjection;

internal class ProductboardClientValidateOptions : IValidateOptions<ProductboardClientOptions>
{
    public ValidateOptionsResult Validate(string name, ProductboardClientOptions options)
    {
        if (options.Endpoint is null)
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.Endpoint)}' must be provided.");
        }

        if (string.IsNullOrWhiteSpace(options.Token))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.Token)}' must be provided.");
        }

        return ValidateOptionsResult.Success;
    }
}
