namespace productboard.Core;

/// <summary>Represents a response with <c>data</c> node.</summary>
/// <typeparam name="T">The type contained in the <c>data</c> node.</typeparam>
public class ResourceWithData<T> where T : class
{
    /// <inheritdoc/>
    public T? Data { get; set; }
}
