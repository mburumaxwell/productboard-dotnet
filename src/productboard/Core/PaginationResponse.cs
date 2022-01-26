using System.Text.Json.Serialization;

namespace productboard.Core;

/// <summary>Represents a paginated response.</summary>
/// <typeparam name="T">The type fo data contained.</typeparam>
public class PaginationResponse<T> : DataResource<T> where T : class
{
    /// <summary>Link to the next page.</summary>
    [JsonPropertyName("links")]
    public PaginationLinks? Next { get; set; }
}
