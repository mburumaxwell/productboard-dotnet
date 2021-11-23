using System.Text.Json.Serialization;

namespace productboard.Models;

/// <summary>
/// The result returned after deleting customer data via GDPR API
/// </summary>
public class GdprDeletionResult
{
    /// <summary>
    /// The success message
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
