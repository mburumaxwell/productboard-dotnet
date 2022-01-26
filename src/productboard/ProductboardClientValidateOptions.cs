using Microsoft.Extensions.Options;
using productboard;

namespace Microsoft.Extensions.DependencyInjection;

internal class ProductboardClientValidateOptions : IValidateOptions<ProductboardClientOptions>
{
    public ValidateOptionsResult Validate(string name, ProductboardClientOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Token))
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.Token)}' must be provided.");
        }

        if (options.BaseUrl == null)
        {
            return ValidateOptionsResult.Fail($"'{nameof(options.BaseUrl)}' must be provided.");
        }

        return ValidateOptionsResult.Success;
    }
}
