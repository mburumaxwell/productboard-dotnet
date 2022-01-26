namespace productboard.Core;

/// <summary>Represents a paginated response.</summary>
/// <typeparam name="T">The type fo data contained.</typeparam>
public class ResourceWithPagination<T> : ResourceWithData<T> where T : class
{
    /// <summary>Link to the next page.</summary>
    public PaginationLinks? Next { get; set; }
}
