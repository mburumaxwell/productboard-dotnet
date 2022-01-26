using System.Text.Json.Serialization;

namespace productboard.Core;

/// <summary>Represents a response with <c>data</c> node.</summary>
/// <typeparam name="T">The type contained in the <c>data</c> node.</typeparam>
public class DataResource<T> where T : class
{
    ///
    public DataResource() { } // required for deserialization

    ///
    public DataResource(T data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }

    ///
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}
