namespace productboard;

/// <summary>
/// Options for configuring the <see cref="ProductboardGdprClient"/>
/// </summary>
public class ProductboardGdprClientOptions : ProductboardClientOptionsBase
{
    /// <summary>
    /// The token for authenticating to the productboard workspace.
    /// </summary>
    /// <remarks>
    /// To get a token:
    /// <list type="number">
    /// <item>Log in to the productboard app in a web browser (https://app.productboard.com)</item>
    /// <item>Go to <b>Workspace Settings</b> > <b>Integrations</b> > <b>Public API</b> > <b>GDPR Public API</b></item>
    /// <item>Click <b>+</b> to generate a new token</item>
    /// </list>
    /// </remarks>
    public string? Token { get; set; }
}
